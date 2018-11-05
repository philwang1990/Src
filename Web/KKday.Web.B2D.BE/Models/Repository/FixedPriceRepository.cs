using System;
using System.Collections.Generic;
using KKday.Web.B2D.BE.AppCode.DAL.FixedPrice;
using KKday.Web.B2D.BE.Models.Model.Common;
using KKday.Web.B2D.BE.Models.Model.FixedPrice;
using Resources;

namespace KKday.Web.B2D.BE.Models.Repository
{
    public class FixedPriceRepository
    {
        private readonly ILocalizer _localizer;

        public FixedPriceRepository(ILocalizer localizer)
        {
            _localizer = localizer;
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

        public List<FixedProduct> GetFixedPriceProds(Int64 comp_xid, string filter, int skip, int size, string sorting)
        {
            var _filter = string.Empty; // GetFieldFiltering(filter);
            var _sorting = string.Empty; // GetFieldSorting(sorting);

            var prod_list = FixedPriceDAL.GetBlacklistProds(comp_xid, _filter, skip, size, _sorting);
            return prod_list;
        }
    }
}
