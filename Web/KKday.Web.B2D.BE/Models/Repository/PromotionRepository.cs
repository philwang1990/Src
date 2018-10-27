using System;
using System.Collections.Generic;
using KKday.Web.B2D.BE.AppCode.DAL.ListPrice;
using KKday.Web.B2D.BE.AppCode.DAL.Promotion;
using KKday.Web.B2D.BE.Models.Model.Common;
using KKday.Web.B2D.BE.Models.Model.Promotion;
using Newtonsoft.Json.Linq;
using Resources;

namespace KKday.Web.B2D.BE.Models.Repository
{
    public class PromotionRepository
    {
        private readonly ILocalizer _localizer;

        public PromotionRepository(ILocalizer localizer)
        {
            _localizer = localizer;
        }

        //取得查詢,排序與分頁資料
        public QueryParamsModel GetQueryParamModel(string filter, string sorting, int size, int current_page)
        {
            var rec_count = GetDiscountMstCount(filter);
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

        //取得折扣項目總筆數
        public int GetDiscountMstCount(string filter)
        {
            var _filter = GetFieldFiltering(filter);

            return DiscountDAL.GetDiscountMstCount(_filter);
        }

        //取得折扣項目清單 
        public List<B2dDiscountMst> GetDiscountMsts(string filter, int skip, int size, string sorting)
        {
            var _filter = GetFieldFiltering(filter);
            var _sorting = GetFieldSorting(sorting);

            var disc_list = DiscountDAL.GetDiscountMsts(_filter, skip, size, _sorting);
            disc_list.ForEach(d => {
                d.STATUS_DESC = d.STATUS.Equals("01") ? _localizer.Text.Enable : _localizer.Text.Disable;
            });

            return disc_list;
        }
        #region Fields Mapping

        private string GetFieldFiltering(string filter)
        {
            var jObjFilter = string.IsNullOrEmpty(filter) ? new JObject() : JObject.Parse(filter);
            var _filter = "";

            // Discount No
            if (!string.IsNullOrEmpty((string)jObjFilter["no"]))
                _filter += $" AND LOWER(disc_no) LIKE '%{jObjFilter["no"]}%' ";

            // Disount Name
            if (!string.IsNullOrEmpty((string)jObjFilter["name"]))
                _filter += $" AND LOWER(disc_name) LIKE '%{jObjFilter["name"]}%' ";

            // Date range
            var s_date = (string)jObjFilter["s_date"];
            var e_date = (string)jObjFilter["e_date"]; 
            if (!string.IsNullOrEmpty(s_date) && !string.IsNullOrEmpty(e_date))
            {
                _filter += $" AND (TO_CHAR(s_date,'yyyyMMdd')<='{ s_date }' AND TO_CHAR(e_date,'yyyyMMdd')>='{ e_date }')";
            }
            else if (!string.IsNullOrEmpty(s_date) && string.IsNullOrEmpty(e_date))
            {
                _filter += $" AND (TO_CHAR(s_date,'yyyyMMdd')<='{ s_date }')";
            }
            else if (string.IsNullOrEmpty(s_date) && !string.IsNullOrEmpty(e_date))
            {
                _filter += $" AND (TO_CHAR(e_date,'yyyyMMdd')>='{ e_date }')";
            }


            return _filter;
        }

        private string GetFieldSorting(string sorting)
        {
            var _sorting = "";

            switch (sorting)
            {
                case "name": _sorting = "prod_name"; break;
                case "no": _sorting = "prod_no"; break;
               
                default: break;
            }

            return _sorting;
        }

        #endregion Fields Mapping


        public void Insert(string prod_no, string prod_name, string crt_user)
        {
            BlacklistProdDAL.InsertBlacklistProd(prod_no, prod_name, crt_user);
        }

        public void Update(Int64 xid, string prod_no, string prod_name, string upd_user)
        {
            BlacklistProdDAL.UpdateBlacklistProd(xid, prod_no, prod_name, upd_user);
        }

        public void Remvoe(Int64 xid, string del_user)
        {
            BlacklistProdDAL.DeleteBlacklistProd(xid, del_user);
        }
    }
}
