using System;
using System.Collections.Generic;
using System.Data;
using KKday.Web.B2D.BE.App_Code;
using KKday.Web.B2D.BE.Models.Account;
using Npgsql;

namespace KKday.Web.B2D.BE.AppCode.DAL.Account
{
    public class AccountAuthDAL
    {
        public UserAccount UserAuth(string account, string password)
        {
            Npgsql.NpgsqlConnection conn = new NpgsqlConnection(Website.Instance.SqlConnectionString);
            UserAccount _account = null;
            
            try
            {
                // 連接Posgresql
                conn.Open();

                string sqlStmt = @"SELECT * FROM b2b.b2d_account_kkday WHERE enable=true AND LOWER(email)=LOWER(:ACCOUNT)";
                var ds = NpgsqlHelper.ExecuteDataset(conn, CommandType.Text, sqlStmt, new NpgsqlParameter("ACCOUNT", account));
                // 檢查是否為有效KKday使用者
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    _account = new KKdayAccount()
                    {
                        XID = dr.ToInt64("xid"),
                        UUID = dr.ToStringEx("user_uuid"),
                        EMAIL = dr.ToStringEx("user_email"),
                        NAME_FIRST = dr.ToStringEx("name_first"),
                        NAME_LAST = dr.ToStringEx("name_last"),
                        ACCOUNT = dr.ToStringEx("user_email"),
                        DEPARTMENT = dr.ToStringEx(""),
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
                    sqlStmt = @"SELECT * FROM b2b.b2d_account WHERE enable=true AND LOWER(email)=LOWER(:ACCOUNT)";
                    ds = NpgsqlHelper.ExecuteDataset(conn, CommandType.Text, sqlStmt, new NpgsqlParameter("ACCOUNT", account));
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];

                        _account = new B2dAccount()
                        {
                            XID = dr.ToInt64("xid"),
                            UUID = dr.ToStringEx("user_uuid"),
                            EMAIL = dr.ToStringEx("user_email"), 
                            NAME_FIRST = dr.ToStringEx("name_first"),
                            NAME_LAST = dr.ToStringEx("name_last"),
                            COMPANY_XID = dr.ToInt64("company_xid"),
                            COMPANY_NAME = dr.ToStringEx("company_name"),
                            ACCOUNT = dr.ToStringEx("user_email"), //與Email相同
                            DEPARTMENT = dr.ToStringEx(""),
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
