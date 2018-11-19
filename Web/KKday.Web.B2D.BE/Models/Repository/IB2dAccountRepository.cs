using System;
using System.Collections.Generic;
using KKday.Web.B2D.BE.Models.Model.Account;
using KKday.Web.B2D.BE.Models.Model.Common;

namespace KKday.Web.B2D.BE.Models.Repository
{
    public interface IB2dAccountRepository
    {
        //取得查詢,排序與分頁資料
        QueryParamsModel GetQueryParamModel(string filter, string sorting, int size, int current_page);
        QueryParamsModel GetQueryParamModel(Int64 comp_xid, string filter, string sorting, int size, int current_page);

        //取得分銷商會員清單總筆數
        int GetAccountsCount(string filter);
        int GetAccountsCount(Int64 comp_xid, string filter);

        //取得分銷商會員清單
        List<B2dAccount> GetAccounts(string filter, int skip, int size, string sorting);
        List<B2dAccount> GetAccounts(Int64 comp_xid, string filter, int skip, int size, string sorting);

        //取得會員資料
        B2dUserProfile GetAccount(Int64 xid);
        B2dUserProfile GetProfile(string email);

        //更新使用者資料
        void UpdateAccount(B2dAccount acct, string upd_user);
        //新增帳號資料
        void InsertAccount(B2dAccount acct, string crt_user);
        //關閉帳號資料
        void CloseAccount(Int64 xid, string upd_user);


        //更新使用者密碼
        bool SetNewPassword(string acct, string password);
        bool SetNewPassword(string acct, string password,Int64 from);
    }
}
