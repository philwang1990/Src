using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using KKday.Web.B2D.BE.App_Code;
using KKday.Web.B2D.BE.Areas.KKday.Models.DataModel.Account;
using KKday.Web.B2D.BE.Models.Model.Account;
using Npgsql;

namespace KKday.Web.B2D.BE.AppCode.DAL.Account
{
    public class AccountDAL
    {
        public static int GetB2dAccountCount(string filter)
        {
            try
            {
                string sqlStmt = @"SELECT COUNT(*)
FROM b2b.b2d_account a
JOIN b2b.b2d_company b ON a.company_xid=b.xid
WHERE 1=1 {FILTER}";

                sqlStmt = sqlStmt.Replace("{FILTER}", !string.IsNullOrEmpty(filter) ? filter : string.Empty);

                int total_count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt));
                return total_count;
            }
            catch (Exception ex)
            {
                Website.Instance._log.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        // 取得所有分銷商使用者列表　
        public static List<B2dAccount> GetB2dAccounts(string filter, int skip, int size, string sorting)
        {
            List<B2dAccount> accounts = new List<B2dAccount>();

            try
            {
                string sqlStmt = @"SELECT a.xid, a.user_uuid, a.email, a.account_type,
 a.name_first, a.name_last, a.name_first || a.name_last AS name, a.department, a.gender_title, 
 a.job_title, a.tel, a.enable, b.xid AS comp_xid, b.comp_name, b.comp_locale, b.comp_currency,
 b.comp_tel_country_code
FROM b2b.b2d_account a
JOIN b2b.b2d_company b ON a.company_xid=b.xid
WHERE 1=1 {FILTER}
{SORTING}
LIMIT :Size OFFSET :Skip";

                sqlStmt = sqlStmt.Replace("{FILTER}", !string.IsNullOrEmpty(filter) ? filter : string.Empty);
                sqlStmt = sqlStmt.Replace("{SORTING}", !string.IsNullOrEmpty(sorting) ? "ORDER BY " + sorting : string.Empty);

                List<NpgsqlParameter> sqlParams = new List<NpgsqlParameter>
                {
                    new NpgsqlParameter("Size", size),
                    new NpgsqlParameter("Skip", skip)
                };

                DataSet ds = NpgsqlHelper.ExecuteDataset(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams.ToArray());
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
                            NAME = dr.ToStringEx("name"),
                            COMPANY_XID = dr.ToInt64("comp_xid"),
                            COMPANY_NAME = dr.ToStringEx("comp_name"), 
                            DEPARTMENT = dr.ToStringEx("department"),
                            ENABLE = dr.ToBoolean("enable"),
                            GENDER_TITLE = dr.ToStringEx("gender_title"),
                            JOB_TITLE = dr.ToStringEx("job_title"),
                            CURRENCY = dr.ToStringEx("comp_currency"),
                            LOCALE = dr.ToStringEx("comp_locale"),
                            TEL_AREA = dr.ToStringEx("comp_tel_country_code"),
                            TEL = dr.ToStringEx("tel"),
                            USER_TYPE = dr.ToStringEx("account_type")
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
         
        public static B2dAccount GetB2dAccount(Int64 xid)
        { 
            try
            {
                string sqlStmt = @"SELECT a.xid, a.user_uuid, a.email, a.account_type,
 a.name_first, a.name_last, a.name_first || a.name_last AS name, a.department, a.gender_title, 
 a.job_title, a.tel, a.enable, b.xid AS comp_xid, b.comp_name, b.comp_locale, b.comp_currency,
 b.comp_tel_country_code
FROM b2b.b2d_account a
JOIN b2b.b2d_company b ON a.company_xid=b.xid
WHERE a.xid=:xid";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("xid", xid)
                };

                DataSet ds = NpgsqlHelper.ExecuteDataset(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    B2dAccount b2dAccount = new B2dAccount()
                    {
                        XID = dr.ToInt64("xid"),
                        UUID = dr.ToStringEx("user_uuid"),
                        EMAIL = dr.ToStringEx("email"),
                        NAME_FIRST = dr.ToStringEx("name_first"),
                        NAME_LAST = dr.ToStringEx("name_last"),
                        NAME = dr.ToStringEx("name"),
                        COMPANY_XID = dr.ToInt64("comp_xid"),
                        COMPANY_NAME = dr.ToStringEx("comp_name"),
                        DEPARTMENT = dr.ToStringEx("department"),
                        ENABLE = dr.ToBoolean("enable"),
                        GENDER_TITLE = dr.ToStringEx("gender_title"),
                        JOB_TITLE = dr.ToStringEx("job_title"),
                        CURRENCY = dr.ToStringEx("comp_currency"),
                        LOCALE = dr.ToStringEx("comp_locale"),
                        TEL_AREA = dr.ToStringEx("comp_tel_country_code"),
                        TEL = dr.ToStringEx("tel"),
                        USER_TYPE = dr.ToStringEx("account_type")
                    };

                    return b2dAccount;
                }

            }
            catch (Exception ex)
            {
                Website.Instance._log.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }

            return null;
        }

        // 取得個別分銷商使用者內容
        public static B2dUserProfile GetB2dProfile(string email)
        {
            B2dUserProfile profile = null;

            try
            {
                string sqlStmt = @"SELECT a.xid, a.user_uuid, a.email, a.account_type,
 a.name_first, a.name_last, a.name_first || a.name_last AS name, a.department, a.gender_title, 
 a.job_title, a.tel, a.enable, b.xid AS comp_xid, b.comp_name, b.comp_tel, b.comp_url, b.comp_locale, 
 b.comp_currency, b.comp_invoice, b.comp_country_code, b.comp_address, b.comp_tel_country_code
FROM b2b.b2d_account a
JOIN b2b.b2d_company b ON a.company_xid=b.xid
WHERE LOWER(email)=LOWER(:email) ";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("email", email)
                };

                DataSet ds = NpgsqlHelper.ExecuteDataset(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);
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
                        NAME = dr.ToStringEx("name"),
                        COMPANY_XID = dr.ToInt64("comp_xid"),
                        COMPANY_NAME = dr.ToStringEx("comp_name"),
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
                        TEL_AREA = dr.ToStringEx("comp_tel_country_code"),
                        TEL = dr.ToStringEx("tel"),
                        USER_TYPE = dr.ToStringEx("account_type")
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

        public static void Update(B2dAccoutUpdModel account, string upd_user)
        {
            try
            { 
                string sqlStmt = @"UPDATE b2b.b2d_account SET xid=:xid, name_last=:name_last, 
name_first=:name_first, account_type=:account_type, enable=:enable, job_title=:job_title, tel=:tel,
gender_title=:gender_title, department=:department, upd_user=:upd_user, upd_datetime=now()
WHERE xid=:xid ";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("xid", account.XID),
                    new NpgsqlParameter("name_last", account.NAME_FIRST),
                    new NpgsqlParameter("name_first", account.NAME_LAST),
                    new NpgsqlParameter("account_type", account.USER_TYPE),
                    new NpgsqlParameter("enable", account.ENABLE),
                    new NpgsqlParameter("job_title", account.JOB_TITLE),
                    new NpgsqlParameter("tel", account.TEL),
                    new NpgsqlParameter("gender_title", (object)account.GENDER_TITLE ?? DBNull.Value),
                    new NpgsqlParameter("department", (object)account.DEPARTMENT ?? DBNull.Value), 
                    new NpgsqlParameter("upd_user", upd_user)
                };

                NpgsqlHelper.ExecuteNonQuery(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);

            }
            catch (Exception ex)
            {
                Website.Instance._log.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static void UpdatePassword(string email, string psw)
        {
            try
            {
                SHA256 sha256 = new SHA256CryptoServiceProvider();//建立一個SHA256
                byte[] source = Encoding.Default.GetBytes(psw);//將字串轉為Byte[]
                byte[] crypto = sha256.ComputeHash(source);//進行SHA256加密
                var chiperPasswod = Convert.ToBase64String(crypto);//把加密後的字串從Byte[]轉為字串

                string sqlStmt = @"UPDATE b2b.b2d_company SET password=:password
WHERE LOWER(email)=LOWER(:email) ";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("email", email),
                    new NpgsqlParameter("password", chiperPasswod)
                };

                NpgsqlHelper.ExecuteNonQuery(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);
               
            }
            catch (Exception ex)
            {
                Website.Instance._log.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }
    }
}
