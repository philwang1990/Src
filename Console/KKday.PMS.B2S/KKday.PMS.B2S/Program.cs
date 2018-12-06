﻿using System;
using KKday.PMS.B2S.ProductRepository;
using KKday.PMS.B2S.PackageRepository;
using KKday.PMS.B2S.ModuleRepository;
using log4net;
using KKday.PMS.B2S.AppCode;
using KKday.PMS.B2S.Models.Shared;
using KKday.PMS.B2S.Models.Product;

namespace KKday.PMS.B2S
{
    class Program
    {
        private readonly static ILog _log = LogManager.GetLogger(typeof(Program));
        private static ProductRepository.ProductRepository product = new ProductRepository.ProductRepository();
        private static PackageRepository.PackageRepository package = new PackageRepository.PackageRepository();
        private static ModuleRepository.ModuleRepository module = new ModuleRepository.ModuleRepository();

        static void Main(string[] args)
        {
            try
            {
                //待補
                long prodOid = 20442;
                SupplierLoginRSModel supplierLoginRSModel = new SupplierLoginRSModel();
                RezdyProductModel rezdyProductModel = new RezdyProductModel();
                RSModel getProductRSModel = new RSModel();
                RSModel createProductRSModel = new RSModel();

                //initial log4net
                CommonTool.LoadLog4netConfig();

                var accounts = new[] { new { id = 1, name = "thisisname" } };
                var suppliers = new[] { new { id = 1, suppliername = "rezdy" } };

                foreach (var account in accounts)
                {
                    foreach (var supplier in suppliers)
                    {
                        //prodOid = 0;

                        // 設定參數
                        supplierLoginRSModel = product.setParameters("AAT Kings Tours", "op-llh@kkday.com", "123456");
                        if(supplierLoginRSModel.msg == "0000")
                        {
                            //抓取商品
                            getProductRSModel = product.getProduct(ref rezdyProductModel);
                            if (getProductRSModel.msg == "0000")
                            {
                                //建立SCM商品
                                createProductRSModel = product.createProduct(supplierLoginRSModel, ref prodOid, rezdyProductModel);
                                if (createProductRSModel.msg == "0000")
                                {
                                    //商品明細
                                    //product.Main();

                                    //套餐
                                    package.Main(
                                    Models.Shared.Enum.PMSSourse.Rezdy,
                                    prodOid,
                                    supplierLoginRSModel.supplierOid,
                                    "PVVRFE",
                                    supplierLoginRSModel.supplierUserUuid,
                                    supplierLoginRSModel.deviceId,
                                    supplierLoginRSModel.tokenKey);

                                    //旅規
                                    //module.Main();
                                }
                                else
                                {
                                    Console.WriteLine("建立商品錯誤:" + createProductRSModel.result);
                                }
                            }
                            else
                            {
                                Console.WriteLine("抓取商品錯誤:"+ getProductRSModel.result);
                            }
                        }
                        else
                        {
                            Console.WriteLine("設定參數錯誤:"+ supplierLoginRSModel.result);
                        }


                    }
                }

                Console.WriteLine("Done!");
            }
            catch (Exception ex)
            {
                //error log
                _log.Error(ex.ToString());
            }
        }
    }
}
