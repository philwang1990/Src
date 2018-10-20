using System;
using System.Collections.Generic;
using KKday.Web.B2D.BE.AppCode.DAL.Account;
using KKday.Web.B2D.BE.Models.Model.Account;
using KKday.Web.B2D.BE.Models.Model.Common;
using Newtonsoft.Json.Linq;
using Resources;

namespace KKday.Web.B2D.BE.Models.Repository
{

    public class B2dAccountRepository
    {
        protected readonly ILocalizer _localizer;

        public B2dAccountRepository(ILocalizer localizer)
        {
            _localizer = localizer;
        }

        // 取得 QueryParamModel
        public QueryParamsModel GetQueryParamModel(string filter, string sorting, int size, int current_page)
        {
            var rec_count = GetAccountsCount(filter);
            var total_pages = (int)(rec_count / size) + ((rec_count % size != 0) ? 1 : 0);
            return new QueryParamsModel()
            {
                Filter = filter,
                Sorting = sorting,
                Paging = new Pagination()
                {
                    current_page = current_page,
                    total_pages = total_pages,
                    page_size = size
                }
            };
        }

        public int GetAccountsCount(string filter)
        {
            try
            {
                var _filter = GetFieldFiltering(filter);

                return AccountDAL.GetB2dAccountCount(_filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<B2dAccount> GetAccounts(string filter, int skip, int size, string sorting)
        {
            var _filter = GetFieldFiltering(filter);
            var _sorting = GetFieldSorting(sorting);

            var account_list = AccountDAL.GetB2dAccounts(_filter, skip, size, _sorting);

            return account_list;
        }

        public UserAccount GetAccount(Int64 xid)
        {
            return AccountDAL.GetB2dAccount(xid);
        }


        #region Fields Mapping

        private string GetFieldFiltering(string filter)
        { 
            var jObjFilter = string.IsNullOrEmpty(filter) ? new JObject() : JObject.Parse(filter);
            var _filter = "";

            // Full Name
            if (!string.IsNullOrEmpty((string)jObjFilter["name"]))
                _filter += $" AND LOWER(name_first || name_last) LIKE '%{jObjFilter["name"]}%' ";
            // Company Name
            if (!string.IsNullOrEmpty((string)jObjFilter["comp_name"])) 
                _filter += $" AND LOWER(comp_name) LIKE '%{jObjFilter["comp_name"]}%' ";
            // Email
            if (!string.IsNullOrEmpty((string)jObjFilter["comp_name"]))
                _filter += $" AND LOWER(email) LIKE '%{jObjFilter["comp_name"]}%' ";
            // enable
            if (!string.IsNullOrEmpty((string)jObjFilter["status"]))
                _filter += $" AND enable = '{jObjFilter["status"]}' ";

            return _filter;
        }

        private string GetFieldSorting(string sorting)
        {
            var _sorting = "";

            switch (sorting)
            {
                case "name": _sorting = "name"; break;
                case "email": _sorting = "email"; break;
                case "comp_name": _sorting = "comp_name"; break;
                case "enable": _sorting = "enable"; break;
                case "xid": _sorting = "xid"; break;
                
                default: break;
            }

            return _sorting;
        }

        #endregion

        public bool SetNewPassword(string account, string password)
        {
            try
            {
                // 呼叫WMS-API設定使用者新密碼

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
