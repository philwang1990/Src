using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using KKday.Web.B2D.BE.App_Code;
using KKday.Web.B2D.EC.Models.Model.Account;
using Npgsql;

namespace KKday.Web.B2D.BE.AppCode.DAL.Account
{
    public class AccountDAL
    {
        // 取得分銷商帳號筆數
        public static int GetAccountCount(string filter, Int64? comp_xid = 0)
        {
            try
            {
                string sqlStmt = "";
                if (comp_xid == 0)
                {
                    sqlStmt = @"SELECT COUNT(*)
FROM b2b.b2d_account a
JOIN b2b.b2d_company b ON a.company_xid=b.xid
WHERE 1=1 {FILTER}";
                }
                else
                {
                    sqlStmt = @"SELECT COUNT(*)
FROM b2b.b2d_account a
JOIN b2b.b2d_company b ON a.company_xid=b.xid
WHERE 1=1 AND b.xid=" + comp_xid + "{FILTER}";

                }

                sqlStmt = sqlStmt.Replace("{FILTER}", !string.IsNullOrEmpty(filter) ? filter : string.Empty);

                int total_count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt));
                return total_count;
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        // 取得所有分銷商使用者列表　
        public static List<B2dAccount> GetAccounts(string filter, int skip, int size, string sorting, Int64? comp_xid = 0)
        {
            List<B2dAccount> accounts = new List<B2dAccount>();

            try
            {
                string sqlStmt = "";
                if (comp_xid == 0)
                {
                    sqlStmt = @"SELECT a.xid, a.user_uuid, a.email, a.account_type,
 a.name_first, a.name_last, a.name_last || a.name_first AS name, a.department, a.gender_title, 
 a.job_title, a.tel, a.enable, b.xid AS comp_xid, b.comp_name, b.comp_locale, b.comp_currency,
 b.comp_tel_country_code
FROM b2b.b2d_account a
JOIN b2b.b2d_company b ON a.company_xid=b.xid
WHERE 1=1 {FILTER}
{SORTING}
LIMIT :Size OFFSET :Skip";
                }
                else
                {
                    sqlStmt = @"SELECT a.xid, a.user_uuid, a.email, a.account_type,
 a.name_first, a.name_last, a.name_last || a.name_first AS name, a.department, a.gender_title, 
 a.job_title, a.tel, a.enable, b.xid AS comp_xid, b.comp_name, b.comp_locale, b.comp_currency,
 b.comp_tel_country_code
FROM b2b.b2d_account a
JOIN b2b.b2d_company b ON a.company_xid=b.xid
WHERE 1=1 AND company_xid=:company_xid{FILTER}
{SORTING}
LIMIT :Size OFFSET :Skip";
                }

                sqlStmt = sqlStmt.Replace("{FILTER}", !string.IsNullOrEmpty(filter) ? filter : string.Empty);
                sqlStmt = sqlStmt.Replace("{SORTING}", !string.IsNullOrEmpty(sorting) ? "ORDER BY " + sorting : string.Empty);

                List<NpgsqlParameter> sqlParams = new List<NpgsqlParameter>
                {
                    new NpgsqlParameter("Size", size),
                    new NpgsqlParameter("Skip", skip),
                    new NpgsqlParameter("company_xid", comp_xid)
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
                Website.Instance.logger.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }

            return accounts;
        }

        // 取得個別分銷商使用者資訊
        public static B2dUserProfile GetAccount(Int64 xid)
        {
            try
            {
                string sqlStmt = @"SELECT a.xid, a.user_uuid, a.email, a.account_type,
 a.name_first, a.name_last, a.name_last || a.name_first AS name, a.department, a.gender_title, 
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

                    B2dUserProfile b2dAccount = new B2dUserProfile()
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
                Website.Instance.logger.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }

            return null;
        }

        // 取得分銷商「我的帳號」資訊
        public static B2dUserProfile GetProfile(string email)
        {
            try
            {
                string sqlStmt = @"SELECT a.xid, a.user_uuid, a.email, a.account_type,
 a.name_first, a.name_last, a.name_last || a.name_first AS name, a.department, a.gender_title, 
 a.job_title, a.tel, a.enable, b.xid AS comp_xid, b.comp_name, b.comp_tel_country_code, b.comp_invoice,
 b.comp_url, b.comp_address
FROM b2b.b2d_account a
JOIN b2b.b2d_company b ON a.company_xid=b.xid
WHERE a.email=:email";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("email", email)
                };

                DataSet ds = NpgsqlHelper.ExecuteDataset(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    B2dUserProfile b2dAccount = new B2dUserProfile()
                    {
                        XID = dr.ToInt64("xid"),
                        UUID = dr.ToStringEx("user_uuid"),
                        EMAIL = dr.ToStringEx("email"),
                        USER_TYPE = dr.ToStringEx("account_type"),
                        NAME_FIRST = dr.ToStringEx("name_first"),
                        NAME_LAST = dr.ToStringEx("name_last"),
                        NAME = dr.ToStringEx("name"),
                        DEPARTMENT = dr.ToStringEx("department"),
                        GENDER_TITLE = dr.ToStringEx("gender_title"),
                        JOB_TITLE = dr.ToStringEx("job_title"),
                        TEL = dr.ToStringEx("tel"),
                        ENABLE = dr.ToBoolean("enable"),
                        COMPANY_XID = dr.ToInt64("comp_xid"),
                        COMPANY_NAME = dr.ToStringEx("comp_name"),
                        TEL_AREA = dr.ToStringEx("comp_tel_country_code"),
                        INVOICE_NO = dr.ToStringEx("comp_invoice"),
                        URL = dr.ToStringEx("comp_url"),
                        ADDRESS = dr.ToStringEx("comp_address")
                    };

                    return b2dAccount;
                }

            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }

            return null;
        }

        // 修改使用者
        public static void UpdateAccount(B2dAccount account, string upd_user)
        {
            try
            {
                string sqlStmt = @"UPDATE b2b.b2d_account SET xid=:xid, name_last=:name_last, 
name_first=:name_first, account_type=:account_type, enable=:enable, job_title=:job_title, tel=:tel,
gender_title=:gender_title, department=:department, upd_user=:upd_user, upd_datetime=now()
WHERE xid=:xid ";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("xid", account.XID),
                    new NpgsqlParameter("name_first", account.NAME_FIRST),
                    new NpgsqlParameter("name_last", account.NAME_LAST),
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
                Website.Instance.logger.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        // 新增使用者
        public static void InsertAccount(B2dAccount account, string ins_user)
        {
            try
            {
                string sqlStmt = @"INSERT INTO b2b.b2d_account(
company_xid, account_type, enable, password, name_last, name_first, gender_title, department, email, tel, 
crt_user, crt_datetime, job_title, user_uuid)
VALUES (:company_xid, :account_type, :enable, :password, :name_last, :name_first, :gender_title,
:department, :email, :tel, :crt_user, now(), :job_title, :user_uuid);";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("company_xid", account.COMPANY_XID),
                    new NpgsqlParameter("account_type", account.USER_TYPE),
                    new NpgsqlParameter("enable", account.ENABLE),
                    new NpgsqlParameter("password", account.PASSWORD),
                    new NpgsqlParameter("name_first", account.NAME_FIRST),
                    new NpgsqlParameter("name_last", account.NAME_LAST),
                    new NpgsqlParameter("gender_title", account.GENDER_TITLE),
                    new NpgsqlParameter("department", account.DEPARTMENT),
                    new NpgsqlParameter("email", account.EMAIL),
                    new NpgsqlParameter("tel", (account.TEL_AREA + account.TEL)),
                    new NpgsqlParameter("job_title", account.JOB_TITLE),
                    new NpgsqlParameter("user_uuid", account.UUID),
                    new NpgsqlParameter("crt_user", ins_user)
                };

                NpgsqlHelper.ExecuteNonQuery(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);

            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        // 關閉使用者
        public static void CloseAccount(Int64 xid, string upd_user)
        {
            try
            {
                string sqlStmt = @"UPDATE b2b.b2d_account
SET enable = false,upd_user=:upd_user
WHERE xid=:xid";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("xid", xid),
                    new NpgsqlParameter("upd_user", upd_user)
                };

                NpgsqlHelper.ExecuteNonQuery(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);

            }

            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        // 更改密碼
        public static void UpdatePassword(string email, string psw)
        {
            try
            {
                SHA256 sha256 = new SHA256CryptoServiceProvider();//建立一個SHA256
                byte[] source = Encoding.Default.GetBytes(psw);//將字串轉為Byte[]
                byte[] crypto = sha256.ComputeHash(source);//進行SHA256加密
                var chiperPasswod = Convert.ToBase64String(crypto);//把加密後的字串從Byte[]轉為字串

                string sqlStmt = @"UPDATE b2b.b2d_account SET password=:password
WHERE LOWER(email)=LOWER(:email) ";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("email", email),
                    new NpgsqlParameter("password", chiperPasswod)
                };

                NpgsqlHelper.ExecuteNonQuery(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);

            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }
    }
}
