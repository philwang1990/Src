using System;
using System.Collections.Generic;
using KKday.Web.B2D.BE.AppCode.DAL.ListPrice;
using KKday.Web.B2D.BE.AppCode.DAL.PriceSetting;
using KKday.Web.B2D.BE.Models.Model.Common;
using KKday.Web.B2D.BE.Models.Model.PriceSetting;
using Newtonsoft.Json.Linq;
using Resources;

namespace KKday.Web.B2D.BE.Models.Repository
{
    public class PriceSettingRepository
    {
        private readonly ILocalizer _localizer;
        private readonly CommonRepository _common;

        public PriceSettingRepository(ILocalizer localizer, CommonRepository common)
        {
            _localizer = localizer;
            _common = common;
        }

        #region DiscountMst Methods

        //取得查詢,排序與分頁資料
        public QueryParamsModel GetMstQueryParamModel(string filter, string sorting, int size, int current_page)
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

        #region *** Fields Mapping ***

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

        #endregion *** Fields Mapping ***

        public B2dDiscountMst GetDiscountMst(Int64 xid) 
        {
            return DiscountDAL.GetDiscountMst(xid);
        }

        public void InsertMst(B2dDiscountMst mst, string crt_user)
        {
            DiscountDAL.InsertDiscountMst(mst, crt_user);
        }

        public void UpdateMst(B2dDiscountMst mst, string upd_user)
        {
            DiscountDAL.UpdateDiscountMst(mst, upd_user);
        }

        public void RemvoeMst(Int64 xid, string del_user)
        {
            DiscountDAL.DeleteDiscountMst(xid, del_user);
        }

        #endregion DiscountMst Methods

        ////////////////

        #region DiscountDTL Methods

        //取得查詢,排序與分頁資料
        public QueryParamsModel GetDtlQueryParamModel(Int64 mst_xid, string filter, string sorting, int size, int current_page)
        {
            var rec_count = GetDiscountDtlCount(mst_xid, filter);
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
        public int GetDiscountDtlCount(Int64 mst_xid, string filter)
        {
            var _filter = string.Empty;
            return DiscountDAL.GetDiscountDtlCount(mst_xid, _filter);
        }

        //取得明細清單 
        public List<B2dDiscountDtl> GetDiscountDtls(Int64 mst_xid, string filter, int skip, int size, string sorting)
        {
            var _filter = string.Empty;
            var _sorting = string.Empty;

            var dtl_list = DiscountDAL.GetDiscountDtls(mst_xid, _filter, skip, size, _sorting);
            return dtl_list;
        }

        public void InsertDtl(B2dDiscountDtl dtl, string crt_user)
        {
            DiscountDAL.InsertDiscountDtl(dtl, crt_user);
        }

        public void UpdateDtl(B2dDiscountDtl dtl, string upd_user)
        {
            DiscountDAL.UpdateDiscountDtl(dtl, upd_user);
        }

        public void RemvoeDtl(Int64 xid, string del_user)
        {
            DiscountDAL.DeleteDiscountDtl(xid, del_user);
        }

        #endregion DiscountDTL Methods

        ////////////////

        #region DiscountCurrAmt Methods 

        //取得查詢,排序與分頁資料
        public QueryParamsModel GetCurrAmtQueryParamModel(Int64 mst_xid, string filter, string sorting, int size, int current_page)
        {
            var rec_count = GetDiscountDtlCount(mst_xid, filter);
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

        //取得語系價格總筆數
        public int GetDiscountCurrAmtCount(Int64 mst_xid, string filter)
        {
            var _filter = string.Empty;
            return DiscountDAL.GetDiscountCurrAmtCount(mst_xid, _filter);
        }

        //取得語系價格清單 
        public List<B2dDiscountCurrAmt> GetDiscountCurrAmts(Int64 mst_xid, string locale, string filter, int skip, int size, string sorting)
        {
            var _filter = string.Empty;
            var _sorting = string.Empty; 

            var curr_locales = _common.GetCurrencyLocale(locale);
            var curramt_list = DiscountDAL.GetDiscountCurrAmts(mst_xid, _filter, skip, size, _sorting);
            curramt_list.ForEach(c =>
            {
                c.CURRENCY_DESC = curr_locales[c.CURRENCY];
            });

            return curramt_list;
        }

        public void InsertCurrAmt(B2dDiscountCurrAmt curr_amt, string crt_user)
        {
            DiscountDAL.InsertDiscountCurrAmnt(curr_amt, crt_user);
        }

        public void UpdateCurrAmt(B2dDiscountCurrAmt curr_amt, string upd_user)
        {
            DiscountDAL.UpdateDiscountCurrAmnt(curr_amt, upd_user);
        }

        public void RemvoeCurrAmt(Int64 xid, string del_user)
        {
            DiscountDAL.DeleteDiscountCurrAmnt(xid, del_user);
        }

        #endregion DiscountCurrAmt Methods 
    }
}
