using System;
using KKday.PMS.B2S.AppCode;
using KKday.PMS.B2S.AppCode.DAL;
using KKday.PMS.B2S.Models.Product;
using KKday.PMS.B2S.Models.Shared;
using KKday.PMS.B2S.Models.Shared.Enum;
using log4net;

namespace KKday.PMS.B2S
{
    class Program
    {
        private readonly static ILog _log = LogManager.GetLogger(typeof(Program));
        private static ProductRepository.ProductRepository product = new ProductRepository.ProductRepository();
        //private static PackageRepository.PackageRepository package = new PackageRepository.PackageRepository();
        private static ModuleRepository.ModuleRepository module = new ModuleRepository.ModuleRepository();

        static void Main(string[] args)
        {
            try
            {
                //Startup startup = new Startup();
                //startup.Initial();
                //Website.Instance.Init(configuration);

                Startup.Instance.Initial();

                //initial log4net
                CommonTool.LoadLog4netConfig();
                Console.WriteLine("Log4net initial..");

                //待補
                long prodOid = 20451;
                SupplierLoginRSModel supplierLoginRSModel = new SupplierLoginRSModel();
                RezdyProductListModel rezdyProductListModel = new RezdyProductListModel();
                RezdyProductModel rezdyProductModel = new RezdyProductModel();
                PackageRepository packageRepository = new PackageRepository();
                RSModel getProductRSModel = new RSModel();
                RSModel getProductListRSModel = new RSModel();
                RSModel createProductRSModel = new RSModel();
                RSModel setScmProductRSModel = new RSModel();
                int offset = 0; // 抓取productList會限定一次只抓一筆 故 offset從0開始抓

                var accounts = new[] { new { id = 1, name = "thisisname" } };
                var pmsList = new[] { new { id = 1, pmsname = "Rezdy" } };
                //var suppliers  ;

                foreach (var account in accounts)
                {
                    foreach (var pms in pmsList)
                    {
                        //prodOid = 0;
                        //抓取所有supplier清單
                        var supplierList = SupplierDAL.GetSupplierList(pms.pmsname);


                        foreach (var supplier in supplierList["Table"])
                        {
                            // 設定參數

                            supplierLoginRSModel = product.setParameters(PMSSourse.Rezdy, supplier["pms_supplier_name"].ToString(), supplier["kkday_supplier_oid"].ToString(), supplier["scm_account"].ToString(), supplier["scm_password"].ToString());
                            if (supplierLoginRSModel.result == "0000")
                            {
                                offset = 0;
                                getProductListRSModel = product.getProductList(PMSSourse.Rezdy, ref rezdyProductListModel, supplier["pms_supplier_id"].ToString(), offset);
                                //foreach (var prod in productList)
                                while (getProductListRSModel.result == "0000" && rezdyProductListModel.Products.Count > 0)
                                {
                                    rezdyProductModel.RequestStatus = rezdyProductListModel.RequestStatus;
                                    rezdyProductModel.Product = rezdyProductListModel.Products[0]; // rezdyProductListModel.Product 每次只取一筆 把取到資料給 rezdyProductModel.Product

                                    //抓取商品
                                    //getProductRSModel = product.getProduct(PMSSourse.Rezdy, ref rezdyProductModel, "PSSPVU");
                                    if (rezdyProductModel.Product != null)
                                    {
                                        //建立SCM商品
                                        //createProductRSModel = product.createProduct(supplierLoginRSModel, ref prodOid, rezdyProductModel);
                                        //if (createProductRSModel.result == "0000")
                                        {
                                            //商品明細
                                            setScmProductRSModel = product.setScmProduct(supplierLoginRSModel, prodOid, rezdyProductModel);

                                            //套餐
                                            //PackageRepository packageRepository = new PackageRepository();
                                            packageRepository.Main(
                                            Models.Shared.Enum.PMSSourse.Rezdy,
                                            prodOid,
                                            supplierLoginRSModel.supplierOid,
                                            rezdyProductModel.Product.productCode,
                                            rezdyProductModel.Product.currency,
                                            supplierLoginRSModel.supplierUserUuid,
                                            supplierLoginRSModel.deviceId,
                                            supplierLoginRSModel.tokenKey);

                                            //旅規
                                            //module.Main();
                                        }
                                        //else
                                        //{
                                        //    Console.WriteLine("建立商品錯誤:" + createProductRSModel.result);
                                        //}
                                    }
                                    else
                                    {
                                        Console.WriteLine("抓取商品錯誤:" + getProductRSModel.result);
                                    }

                                    offset++;
                                    getProductListRSModel = product.getProductList(PMSSourse.Rezdy, ref rezdyProductListModel, supplier["pms_supplier_id"].ToString(), offset);
                                }
                            }
                            else
                            {
                                Console.WriteLine("設定參數錯誤:" + supplierLoginRSModel.result);
                            }

                        }
                    }
                }

                Console.WriteLine("Done!");
            }
            catch (Exception ex)
            {
                //error log
                _log.Error(ex.ToString());
                throw ex;
            }
        }




    }


}
