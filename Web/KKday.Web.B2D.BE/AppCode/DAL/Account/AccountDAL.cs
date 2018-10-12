using System;
using System.Collections.Generic;
using System.Data;
using KKday.Web.B2D.BE.App_Code;
using KKday.Web.B2D.BE.Models.Account;
using Npgsql;

namespace KKday.Web.B2D.BE.AppCode.DAL.Account
{
    public class AccountDAL
    {
        // 取得所有分銷商使用者列表　
        public static List<B2dAccount> GetAccounts()
        {
            List<B2dAccount> accounts = new List<B2dAccount>();

            try
            {
                string sqlStmt = @"SELECT  a.xid, a.user_uuid, a.email, a.account_type,
 a.name_first, a.name_last, a.department, a.gender_title, a.job_title, a.enable,
 b.xid AS comp_xid, b.comp_name, b.comp_language AS comp_locale, b.comp_currency
FROM b2b.b2d_account a
JOIN b2b.b2d_company b ON a.company_xid=b.xid
";

                DataSet ds = NpgsqlHelper.ExecuteDataset(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        accounts.Add(new B2dAccount()
                        {
                            XID = dr.ToInt64("xid"),
                            UUID = dr.ToStringEx("user_uuid"),
                            EMAIL = dr.ToStringEx("email"),
                            NAME_FIRST = dr.ToStringEx("name_first"),
                            NAME_LAST = dr.ToStringEx("name_last"),
                            COMPANY_XID = dr.ToInt64("comp_xid"),
                            COMPANY_NAME = dr.ToStringEx("comp_name"),
                            ACCOUNT = dr.ToStringEx("email"), //與Email相同
                            DEPARTMENT = dr.ToStringEx(""),
                            ENABLE = dr.ToBoolean("enable"),
                            GENDER_TITLE = dr.ToStringEx("gender_title"),
                            JOB_TITLE = dr.ToStringEx("job_title"),
                            CURRENCY = dr.ToStringEx("comp_currency"),
                            LOCALE = dr.ToStringEx("comp_locale"),
                            TEL = dr.ToStringEx("comp_tel"),
                            USER_TYPE = dr.ToStringEx("account_type").Equals("01") ? "ADM" : "USER"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Website.Instance._log.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }

            return accounts;
        }

        // 取得所有分銷商使用者列表　
        public static B2dAccount GetAccount(string account)
        {
            B2dAccount b2dAccount = null;

            try
            {
                string sqlStmt = @"SELECT a.xid, a.user_uuid, a.email, a.account_type,
 a.name_first, a.name_last, a.department, a.gender_title, a.job_title, a.enable,
 b.xid AS comp_xid, b.comp_name, b.comp_language AS comp_locale, b.comp_currency
FROM b2b.b2d_account a
JOIN b2b.b2d_company b ON a.company_xid=b.xid
WHERE LOWER(email)=LOWER(:ACCOUNT)
";

                DataSet ds = NpgsqlHelper.ExecuteDataset(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    b2dAccount = new B2dAccount()
                    {
                        XID = dr.ToInt64("xid"),
                        UUID = dr.ToStringEx("user_uuid"),
                        EMAIL = dr.ToStringEx("email"),
                        NAME_FIRST = dr.ToStringEx("name_first"),
                        NAME_LAST = dr.ToStringEx("name_last"),
                        COMPANY_XID = dr.ToInt64("comp_xid"),
                        COMPANY_NAME = dr.ToStringEx("comp_name"),
                        ACCOUNT = dr.ToStringEx("email"), //與Email相同
                        DEPARTMENT = dr.ToStringEx(""),
                        ENABLE = dr.ToBoolean("enable"),
                        GENDER_TITLE = dr.ToStringEx("gender_title"),
                        JOB_TITLE = dr.ToStringEx("job_title"),
                        CURRENCY = dr.ToStringEx("comp_currency"),
                        LOCALE = dr.ToStringEx("comp_locale"),
                        TEL = dr.ToStringEx("comp_tel"),
                        USER_TYPE = dr.ToStringEx("account_type").Equals("01") ? "ADM" : "USER"
                    };
                }
            }
            catch (Exception ex)
            {
                Website.Instance._log.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }

            return b2dAccount;
        }

        // 取得個別分銷商使用者內容
        public static B2dUserProfile GetProfile(string account)
        {
            B2dUserProfile profile = null;

            try
            {
                string sqlStmt = @"SELECT a.xid, a.user_uuid, a.email, a.account_type,
 a.name_first, a.name_last, a.department, a.gender_title, a.job_title, a.enable,
 b.xid AS comp_xid, b.comp_name, b.comp_tel, b.comp_url, b.comp_language AS comp_locale,
 b.comp_currency, b.comp_invoice, b.comp_country_code, b.comp_address
FROM b2b.b2d_account a
JOIN b2b.b2d_company b ON a.company_xid=b.xid
WHERE LOWER(email)=LOWER(:ACCOUNT)
";

                DataSet ds = NpgsqlHelper.ExecuteDataset(Website.Instance.SqlConnectionString,
                                   CommandType.Text, sqlStmt, new NpgsqlParameter("ACCOUNT", account));
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    profile = new B2dUserProfile()
                    {
                        XID = dr.ToInt64("xid"),
                        UUID = dr.ToStringEx("user_uuid"),
                        EMAIL = dr.ToStringEx("email"),
                        NAME_FIRST = dr.ToStringEx("name_first"),
                        NAME_LAST = dr.ToStringEx("name_last"),
                        COMPANY_XID = dr.ToInt64("comp_xid"),
                        COMPANY_NAME = dr.ToStringEx("comp_name"),
                        ACCOUNT = dr.ToStringEx("email"), //與Email相同
                        DEPARTMENT = dr.ToStringEx("department"),
                        ENABLE = dr.ToBoolean("enable"),
                        GENDER_TITLE = dr.ToStringEx("gender_title"),
                        JOB_TITLE = dr.ToStringEx("job_title"),
                        CURRENCY = dr.ToStringEx("comp_currency"),
                        LOCALE = dr.ToStringEx("comp_locale"),
                        ADDRESS = dr.ToStringEx("comp_address"),
                        COUNTRY_CODE = dr.ToStringEx("comp_country_code"),
                        URL = dr.ToStringEx("comp_url"),
                        INVOICE_NO = dr.ToStringEx("comp_invoice"),
                        TEL = dr.ToStringEx("comp_tel"),
                        USER_TYPE = dr.ToStringEx("account_type").Equals("01") ? "ADM" : "USER"
                    };
                }
            }
            catch (Exception ex)
            {
                Website.Instance._log.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }

            return profile;
        }
    }
}
