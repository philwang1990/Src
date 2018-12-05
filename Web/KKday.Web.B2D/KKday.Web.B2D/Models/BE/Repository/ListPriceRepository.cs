using System;
using System.Collections.Generic;
using KKday.Web.B2D.BE.AppCode.DAL.ListPrice; 
using KKday.Web.B2D.BE.Models.Model.Common;
using KKday.Web.B2D.BE.Models.Model.ListPrice;
using Newtonsoft.Json.Linq;
using Resources;

namespace KKday.Web.B2D.BE.Models.Repository
{
    public class ListPriceRepository
    {
        private readonly ILocalizer _localizer;

        public ListPriceRepository(ILocalizer localizer)
        {
            _localizer = localizer;
        }

        //取得查詢,排序與分頁資料
        public QueryParamsModel GetQueryParamModel(string filter, string sorting, int size, int current_page)
        {
            var rec_count = GetBlacklistProdCount(filter);
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

        //取得黑名單商品清單總筆數
        public int GetBlacklistProdCount(string filter)
        {
            var _filter = GetFieldFiltering(filter);

            return BlacklistProdDAL.GetBlacklistProdCount(_filter);
        }

        //取得黑名單商品清單 
        public List<B2dBlacklistProduct> GetBlacklistProds(string filter, int skip, int size, string sorting) 
        {
            var _filter = GetFieldFiltering(filter);
            var _sorting = GetFieldSorting(sorting);

            return BlacklistProdDAL.GetBlacklistProds(_filter, skip, size, _sorting);
        }
        #region Fields Mapping

        private string GetFieldFiltering(string filter)
        {
            var jObjFilter = string.IsNullOrEmpty(filter) ? new JObject() : JObject.Parse(filter);
            var _filter = "";

            // Prod No
            if (!string.IsNullOrEmpty((string)jObjFilter["no"]))
                _filter += $" AND LOWER(prod_no) LIKE '%{jObjFilter["no"]}%' ";
            // Prod Name
            if (!string.IsNullOrEmpty((string)jObjFilter["name"]))
                _filter += $" AND LOWER(prod_name) LIKE '%{jObjFilter["name"]}%' ";
             
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
