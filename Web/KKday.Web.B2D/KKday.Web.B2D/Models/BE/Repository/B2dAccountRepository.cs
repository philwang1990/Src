using System;
using System.Collections.Generic;
using KKday.Web.B2D.BE.AppCode;
using KKday.Web.B2D.BE.AppCode.DAL.Account;
using KKday.Web.B2D.EC.Models.Model.Account;
using KKday.Web.B2D.BE.Models.Model.Common;
using KKday.Web.B2D.EC.AppCode;
using Newtonsoft.Json.Linq;
using Resources;

namespace KKday.Web.B2D.BE.Models.Repository
{
    public class B2dAccountRepository : IB2dAccountRepository
    {
        protected readonly ILocalizer _localizer;

        public B2dAccountRepository(ILocalizer localizer)
        {
            _localizer = localizer;
        }

        // [共用]取得各分銷商單一帳號資訊
        public B2dUserProfile GetAccount(Int64 xid)
        {
            return AccountDAL.GetAccount(xid);
        }

        // [共用]修改分銷商單一帳號資訊
        public void UpdateAccount(B2dAccount acct, string upd_user)
        {
            AccountDAL.UpdateAccount(acct, upd_user);
        }

        // [共用]新增帳號資訊
        public void InsertAccount(B2dAccount acct, string crt_user)
        {
            if (acct.PASSWORD != null)
            {
                acct.PASSWORD = Sha256Helper.Gethash(acct.PASSWORD);
                acct.UUID = Guid.NewGuid().ToString();
                AccountDAL.InsertAccount(acct, crt_user);
            }
        }

        // [共用]關閉帳號
        public void CloseAccount(Int64 xid, string upd_user)
        {
            AccountDAL.CloseAccount(xid, upd_user);
        }


        #region KKday使用者區塊

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

        // 取得分銷商共有多少筆帳號[分頁用]
        public int GetAccountsCount(string filter)
        {
            try
            {
                var _filter = GetFieldFiltering(filter);

                return AccountDAL.GetAccountCount(_filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // 取得各分銷商的帳號清單
        public List<B2dAccount> GetAccounts(string filter, int skip, int size, string sorting)
        {
            var _filter = GetFieldFiltering(filter);
            var _sorting = GetFieldSorting(sorting);

            var account_list = AccountDAL.GetAccounts(_filter, skip, size, _sorting);
            account_list.ForEach(a =>
            {
                a.USER_TYPE_DESC = a.USER_TYPE.Equals("01") ? _localizer.Text.UserRole_01 : _localizer.Text.UserRole_00;
            });

            return account_list;
        }

        #endregion KKday使用者區塊

        #region 分銷商使用者區塊

        // 取得我的帳號
        public B2dUserProfile GetProfile(string email)
        {
            return AccountDAL.GetProfile(email);
        }

        // 取得 QueryParamModel
        public QueryParamsModel GetQueryParamModel(Int64 comp_xid, string filter, string sorting, int size, int current_page)
        {
            var rec_count = GetAccountsCount(comp_xid, filter);
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

        // 取得分銷商共有多少筆帳號[分頁用]
        public int GetAccountsCount(Int64 comp_xid, string filter)
        {
            try
            {
                var _filter = GetFieldFiltering(filter);

                return AccountDAL.GetAccountCount(_filter, comp_xid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // 取得分銷商的帳號清單 WebUserList
        public List<B2dAccount> GetAccounts(Int64 comp_xid, string filter, int skip, int size, string sorting)
        {
            var _filter = GetFieldFiltering(filter);
            var _sorting = GetFieldSorting(sorting);

            var account_list = AccountDAL.GetAccounts(_filter, skip, size, _sorting, comp_xid);
            account_list.ForEach(a =>
            {
                a.USER_TYPE_DESC = a.USER_TYPE.Equals("01") ? _localizer.Text.UserRole_01 : _localizer.Text.UserRole_00;
            });

            return account_list;
        }

        #endregion 分銷商使用者區塊

        #region Fields Mapping

        private string GetFieldFiltering(string filter)
        {
            var jObjFilter = string.IsNullOrEmpty(filter) ? new JObject() : JObject.Parse(filter);
            var _filter = "";

            // Full Name
            if (!string.IsNullOrEmpty((string)jObjFilter["name"]))
                _filter += $" AND LOWER(name_last || name_first) LIKE '%{jObjFilter["name"]}%' ";
            // Company Name
            if (!string.IsNullOrEmpty((string)jObjFilter["comp_name"]))
                _filter += $" AND LOWER(comp_name) LIKE '%{jObjFilter["comp_name"]}%' ";
            // Email
            if (!string.IsNullOrEmpty((string)jObjFilter["email"]))
                _filter += $" AND LOWER(email) LIKE '%{jObjFilter["email"]}%' ";
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

        #endregion Fields Mapping

        public bool SetNewPassword(string account, string password)
        {
            try
            {
                AccountDAL.UpdatePassword(account, password);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
