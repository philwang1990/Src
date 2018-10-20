using System;
using System.Collections.Generic;
using System.Data;
using KKday.Web.B2D.BE.App_Code;
using KKday.Web.B2D.BE.Models.Model.Account;
using Npgsql;

namespace KKday.Web.B2D.BE.AppCode.DAL.Account
{
    public class AccountAuthDAL
    {
        public static UserAccount UserAuth(string account, string password)
        {
            Npgsql.NpgsqlConnection conn = new NpgsqlConnection(Website.Instance.SqlConnectionString);
            UserAccount _account = null;
            
            try
            {
                // 連接Posgresql
                conn.Open();

                string sqlStmt = @"SELECT *, name_first || name_last AS Name 
FROM b2b.b2d_account_kkday 
WHERE enable=true AND LOWER(email)=LOWER(:account) AND password=:password";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[]{
                    new NpgsqlParameter("account", account),
                    new NpgsqlParameter("password", password)
                };

                var ds = NpgsqlHelper.ExecuteDataset(conn, CommandType.Text, sqlStmt, sqlParams);
                // 檢查是否為有效KKday使用者
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    _account = new KKdayAccount()
                    {
                        XID = dr.ToInt64("xid"),
                        UUID = dr.ToStringEx("user_uuid"),
                        EMAIL = dr.ToStringEx("email"),
                        NAME = dr.ToStringEx("name"),
                        NAME_FIRST = dr.ToStringEx("name_first"),
                        NAME_LAST = dr.ToStringEx("name_last"),
                        ACCOUNT = dr.ToStringEx("email"),
                        DEPARTMENT = dr.ToStringEx("department"),
                        ENABLE = dr.ToBoolean("enable"),
                        //GENDER_TITLE = dr.ToStringEx("gender_title"),
                        //JOB_TITLE = dr.ToStringEx("job_title"),
                        STAFF_NO = dr.ToStringEx("staff_no"), 
                        ROLES = dr.ToStringEx("roles"),
                        LOCALE = dr.ToStringEx("locale") 
                        
                    };
                }
                // 檢查是否為分銷商使用者
                else {
                    sqlStmt = @"SELECT a.xid, a.user_uuid, a.email, a.name_first, a.name_last,
 a.name_first || a.name_last AS name, a.department, a.job_title, a.enable, a.gender_title,
 b.xid as comp_xid, b.comp_name, b.comp_locale AS locale, b.comp_currency AS currency
FROM b2b.b2d_account a
JOIN b2b.b2d_company b ON a.company_xid=b.xid
WHERE enable=true AND LOWER(email)=LOWER(:account) AND password=:password";

                    sqlParams = new NpgsqlParameter[]{
                        new NpgsqlParameter("account", account),
                        new NpgsqlParameter("password", password)
                    };

                    ds = NpgsqlHelper.ExecuteDataset(conn, CommandType.Text, sqlStmt, sqlParams);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];

                        _account = new B2dAccount()
                        {
                            XID = dr.ToInt64("xid"),
                            UUID = dr.ToStringEx("user_uuid"),
                            EMAIL = dr.ToStringEx("email"),
                            NAME = dr.ToStringEx("name"),
                            NAME_FIRST = dr.ToStringEx("name_first"),
                            NAME_LAST = dr.ToStringEx("name_last"),
                            COMPANY_XID = dr.ToInt64("comp_xid"),
                            COMPANY_NAME = dr.ToStringEx("comp_name"),
                            ACCOUNT = dr.ToStringEx("email"), //與Email相同
                            DEPARTMENT = dr.ToStringEx("department"),
                            ENABLE = dr.ToBoolean("enable"),
                            GENDER_TITLE = dr.ToStringEx("gender_title"),
                            JOB_TITLE = dr.ToStringEx("job_title"),
                            CURRENCY = dr.ToStringEx("currency"),
                            LOCALE = dr.ToStringEx("locale")
                        };
                    }
                }

                conn.Close();
            }
            catch(Exception ex)
            {
                if (conn.State != ConnectionState.Closed) conn.Close();

                throw ex;
            }

            return _account;
        }
    }
}
