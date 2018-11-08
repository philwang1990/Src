using System;
using System.Collections.Generic;
using System.Linq;
using KKday.Web.B2D.BE.AppCode.DAL.FixedPrice;
using KKday.Web.B2D.BE.Models.Model.Common;
using KKday.Web.B2D.BE.Models.Model.FixedPrice;
using KKday.Web.B2D.BE.Proxy;
using Microsoft.AspNetCore.Http;
using Resources;

namespace KKday.Web.B2D.BE.Models.Repository
{
    public class FixedPriceRepository
    {
        private readonly ILocalizer _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CommonRepository _commRepos;

        public FixedPriceRepository(ILocalizer localizer, IHttpContextAccessor httpContextAccessor, 
             CommonRepository commRepos)
        {
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
            _commRepos = commRepos;
        }

        // 取得 QueryParamModel
        public QueryParamsModel GetQueryParamModel(Int64 comp_xid, string filter, string sorting, int size, int current_page)
        {
            var rec_count = GetFixedPriceProdCount(comp_xid, filter);
            var total_pages = (int)(rec_count / size) + ((rec_count % size != 0) ? 1 : 0);
            return new QueryParamsModel()
            {
                Filter = filter,
                Sorting = sorting,
                Paging = new Pagination()
                {
                    current_page = current_page,
                    total_count = rec_count,
                    total_pages = total_pages,
                    page_size = size
                }
            };
        }

        public int GetFixedPriceProdCount(Int64 comp_xid, string filter)
        {
            try
            {
                var _filter = string.Empty; // GetFieldFiltering(filter);
                return FixedPriceDAL.GetFixedPriceProdCount(comp_xid, _filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<FixedPriceProductEx> GetFixedPriceProds(Int64 comp_xid, string filter, int skip, int size, string sorting)
        {
            var _filter = string.Empty; // GetFieldFiltering(filter);
            var _sorting = string.Empty; // GetFieldSorting(sorting);

            var locale = _httpContextAccessor.HttpContext.User.FindFirst("Locale").Value;
            var countries = _commRepos.GetCountryAreas(locale);
          
            var prod_list = FixedPriceDAL.GetFixedPriceProds(comp_xid, _filter, skip, size, _sorting);
            prod_list.ForEach(p => {
                var country = countries.FirstOrDefault(c => c.countryCode.Equals(p.STATE));
                p.STATE_NAME = (country == null) ? string.Empty : country.countryName; 
            });
            return prod_list;
        }

        public FixedPriceProductEx GetFixedPriceProduct(Int64 prod_xid)
        {
            return FixedPriceDAL.GetFixedPriceProd(prod_xid);
        }

        public void InsertProd(FixedPriceProduct prod, string crt_user) 
        {
            FixedPriceDAL.InsertFixedPriceProduct(prod, crt_user);
        }

        /////////////

        public List<FixedPricePackageEx> GetFixedPricePackages(Int64 prod_xid)
        {
            List<FixedPricePackageEx> pkgs = new List<FixedPricePackageEx>();
            return pkgs;
        }

        public List<FixedPricePackageEx> SyncPackage(FixedPriceProductEx prod)
        {
            List<FixedPricePackageEx> pkgs = new List<FixedPricePackageEx>();

            // 登入者的語系
            var locale = _httpContextAccessor.HttpContext.User.FindFirst("Locale").Value;

            var KKdayPackages = PackageProxy.GetProdPackages(prod.COMPANY_XID, locale, prod.CURRENCY, prod.PROD_NO, prod.STATE);

            int _pkg_seq = 1;
            KKdayPackages.ForEach(p =>
            {
                var prices = new List<FixedPricePackageDtl>();

                prices.Add(new FixedPricePackageDtlEx()
                {
                    PKG_SEQ_NO = _pkg_seq,
                    PRICE_COND = "price_1",
                    PRICE = p.price1
                });

                prices.Add(new FixedPricePackageDtlEx()
                {
                    PKG_SEQ_NO = _pkg_seq,
                    PRICE_COND = "price_2",
                    PRICE = p.price2
                });

                prices.Add(new FixedPricePackageDtlEx()
                {
                    PKG_SEQ_NO = _pkg_seq,
                    PRICE_COND = "price_3",
                    PRICE = p.price3
                });

                prices.Add(new FixedPricePackageDtlEx()
                {
                    PKG_SEQ_NO = _pkg_seq,
                    PRICE_COND = "price_4",
                    PRICE = p.price4
                });

                pkgs.Add(new FixedPricePackageEx()
                {
                    SEQ_NO = _pkg_seq,
                    PACKAGE_NO = p.pkg_no,
                    PACKAGE_NAME = p.pkg_name,
                    PROD_XID = prod.XID,
                    Prices = prices
                });

                _pkg_seq++;
            });

            return pkgs;
        }
    }
}
