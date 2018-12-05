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
    public class ApiAccountDAL
    {
        // 取得API帳號筆數
        public static int GetAccountCount_Api(string filter, Int64? comp_xid = 0)
        {
            try
            {
                string sqlStmt = "";
                if (comp_xid == 0)
                {
                    sqlStmt = @"SELECT COUNT(*)
FROM b2b.b2d_account_api a
JOIN b2b.b2d_company b ON a.company_xid=b.xid
WHERE 1=1 {FILTER}";
                }
                else
                {
                    sqlStmt = @"SELECT COUNT(*)
FROM b2b.b2d_account_api a
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

        // 取得所有API使用者列表　
        public static List<B2dAccount> GetAccounts_Api(string filter, int skip, int size, string sorting, Int64? comp_xid = 0)
        {
            List<B2dAccount> accounts = new List<B2dAccount>();

            try
            {
                string sqlStmt = "";
                if (comp_xid == 0)
                {
                    sqlStmt = @"SELECT a.xid, a.user_uuid, a.email, a.account_type, a.gender_title,
 a.name_first, a.name_last, a.name_first || a.name_last AS name, a.department, a.job_title,
 a.tel, a.enable, b.xid AS comp_xid, b.comp_name, b.comp_locale, b.comp_currency,
 b.comp_tel_country_code
FROM b2b.b2d_account_api a
JOIN b2b.b2d_company b ON a.company_xid=b.xid
WHERE 1=1 {FILTER}
{SORTING}
LIMIT :Size OFFSET :Skip";
                }

                else
                {
                    sqlStmt = @"SELECT a.xid, a.user_uuid, a.email, a.account_type, a.gender_title,
 a.name_first, a.name_last, a.name_first || a.name_last AS name, a.department, a.job_title,
 a.tel, a.enable, b.xid AS comp_xid, b.comp_name, b.comp_locale, b.comp_currency,
 b.comp_tel_country_code
FROM b2b.b2d_account_api a
JOIN b2b.b2d_company b ON a.company_xid=b.xid
WHERE 1=1 AND b.xid=" + comp_xid + @"{FILTER}
{SORTING}
LIMIT :Size OFFSET :Skip";
                }

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
                            GENDER_TITLE = dr.ToStringEx("gender_title"),
                            NAME_FIRST = dr.ToStringEx("name_first"),
                            NAME_LAST = dr.ToStringEx("name_last"),
                            NAME = dr.ToStringEx("name"),
                            COMPANY_XID = dr.ToInt64("comp_xid"),
                            COMPANY_NAME = dr.ToStringEx("comp_name"),
                            DEPARTMENT = dr.ToStringEx("department"),
                            JOB_TITLE = dr.ToStringEx("job_title"),
                            ENABLE = dr.ToBoolean("enable"),
                            CURRENCY = dr.ToStringEx("comp_currency"),
                            LOCALE = dr.ToStringEx("comp_locale"),
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

        // 取得個別API使用者資訊
        public static B2dUserProfile GetAccount_Api(Int64 xid)
        {
            try
            {
                string sqlStmt = @"SELECT a.xid, a.user_uuid, a.email, a.account_type, a.gender_title,
 a.name_first, a.name_last, a.name_last || a.name_first AS name, a.department, a.job_title,a.password,
 a.tel, a.enable, b.xid AS comp_xid, b.comp_name, b.comp_locale, b.comp_currency,
 b.comp_tel_country_code
FROM b2b.b2d_account_api a
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
                        PASSWORD=dr.ToStringEx("password"),
                        GENDER_TITLE = dr.ToStringEx("gender_title"),
                        NAME_FIRST = dr.ToStringEx("name_first"),
                        NAME_LAST = dr.ToStringEx("name_last"),
                        NAME = dr.ToStringEx("name"),
                        COMPANY_XID = dr.ToInt64("comp_xid"),
                        COMPANY_NAME = dr.ToStringEx("comp_name"),
                        DEPARTMENT = dr.ToStringEx("department"),
                        JOB_TITLE = dr.ToStringEx("job_title"),
                        ENABLE = dr.ToBoolean("enable"),
                        CURRENCY = dr.ToStringEx("comp_currency"),
                        LOCALE = dr.ToStringEx("comp_locale"),
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

        // 修改API使用者
        public static void UpdateAccount_Api(B2dAccount account, string upd_user)
        {
            try
            {
                string sqlStmt = @"UPDATE b2b.b2d_account_api SET xid=:xid, name_last=:name_last, 
name_first=:name_first, account_type=:account_type, gender_title=:gender_title, enable=:enable, 
tel=:tel, department=:department, job_title=:job_title,upd_user=:upd_user, upd_datetime=now()
WHERE xid=:xid ";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("xid", account.XID),
                    new NpgsqlParameter("gender_title", account.GENDER_TITLE),
                    new NpgsqlParameter("name_last", account.NAME_LAST),
                    new NpgsqlParameter("name_first", account.NAME_FIRST),
                    new NpgsqlParameter("account_type", account.USER_TYPE),
                    new NpgsqlParameter("enable", account.ENABLE),
                    new NpgsqlParameter("tel", account.TEL),
                    new NpgsqlParameter("department", account.DEPARTMENT),
                    new NpgsqlParameter("job_title", account.JOB_TITLE),
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

        // 新增API使用者
        public static void InsertApiAccount_Api(B2dAccount account, string crt_user)
        {
            try
            {
                string sqlStmt = @"INSERT INTO b2b.b2d_account_api(
user_uuid, source, company_xid, account_type, enable, password, name_last, name_first, gender_title, 
department, job_title, email, tel, crt_user, crt_datetime, api_token)
VALUES (:user_uuid, :source, :company_xid, :account_type, :enable, :password, :name_last, :name_first, 
:gender_title, :department, :job_title, :email, :tel, :crt_user, now(), :api_token);";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("user_uuid", account.UUID),
                    new NpgsqlParameter("source", ""),
                    new NpgsqlParameter("company_xid", account.COMPANY_XID),
                    new NpgsqlParameter("account_type", account.USER_TYPE),
                    new NpgsqlParameter("enable", account.ENABLE),
                    new NpgsqlParameter("password", account.PASSWORD),
                    new NpgsqlParameter("name_last", account.NAME_LAST),
                    new NpgsqlParameter("name_first", account.NAME_FIRST),
                    new NpgsqlParameter("gender_title", account.GENDER_TITLE),
                    new NpgsqlParameter("department", account.DEPARTMENT),
                    new NpgsqlParameter("job_title", account.JOB_TITLE),
                    new NpgsqlParameter("email", account.EMAIL),
                    new NpgsqlParameter("tel", account.TEL),
                    new NpgsqlParameter("crt_user", crt_user),
                    new NpgsqlParameter("api_token", "")
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
        public static void CloseAccount_Api(Int64 xid, string upd_user)
        {
            try
            {
                string sqlStmt = @"UPDATE b2b.b2d_account_api
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
        public static void UpdatePassword_Api(string email, string psw)
        {
            try
            {
                SHA256 sha256 = new SHA256CryptoServiceProvider();//建立一個SHA256
                byte[] source = Encoding.Default.GetBytes(psw);//將字串轉為Byte[]
                byte[] crypto = sha256.ComputeHash(source);//進行SHA256加密
                var chiperPasswod = Convert.ToBase64String(crypto);//把加密後的字串從Byte[]轉為字串

                string sqlStmt = @"UPDATE b2b.b2d_account_api SET password=:password
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

        // 從DB取現有token
        public static string GetToken(Int64 xid)
        {
            var token = "";

            var sqlStmt = @"SELECT api_token
FROM b2b.b2d_account_api
WHERE xid=" + xid;

            DataSet ds= NpgsqlHelper.ExecuteDataset(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt);
            if(ds!=null)
            {
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    token = dr.ToStringEx("api_token");
                }

            }
            return token;
        }

        // 把新token寫入DB
        public static void SetApiToken(string acc, string token)
        {
            try
            {
                var sqlStmt = @"UPDATE b2b.b2d_account_api
SET api_token=:api_token
WHERE email=:email";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("api_token", token),
                    new NpgsqlParameter("email", acc)
                    };
                NpgsqlHelper.ExecuteNonQuery(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // 取得現有快取時間
        public static Int64 GetCacheTime(Int64 comp_xid)
        {
            var sqlStmt = @"select cache_ttl from b2b.b2d_company
            where xid=" + comp_xid;

            Int64 cache_time= Convert.ToInt32(NpgsqlHelper.ExecuteScalar(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt));
            return cache_time;
        }

        // 更改快取時間
        public static void UpdCacheTime(Int64 time, Int64 comp_xid)
        {
            var sqlStmt = @"Update b2b.b2d_company
SET cache_ttl=" + time +
"where xid=" + comp_xid;

            NpgsqlHelper.ExecuteScalar(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt);
        }


    }

}
