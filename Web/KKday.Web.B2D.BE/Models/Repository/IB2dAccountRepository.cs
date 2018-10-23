using System;
using System.Collections.Generic;
using KKday.Web.B2D.BE.Areas.KKday.Models.DataModel.Account;
using KKday.Web.B2D.BE.Models.Model.Account;
using KKday.Web.B2D.BE.Models.Model.Common;
  
namespace KKday.Web.B2D.BE.Models.Repository
{
    public interface IB2dAccountRepository
    {
        //取得查詢,排序與分頁資料
        QueryParamsModel GetQueryParamModel(string filter, string sorting, int size, int current_page);
        //取得分銷商會員清單總筆數
        int GetAccountsCount(string filter);
        //取得分銷商會員清單
        List<B2dAccount> GetAccounts(string filter, int skip, int size, string sorting);
        //取得會員資料
        B2dAccount GetAccount(Int64 xid);
        //更新使用者資料
        void UpdateAccount(B2dAccoutUpdModel account, string upd_user);
    }
}
