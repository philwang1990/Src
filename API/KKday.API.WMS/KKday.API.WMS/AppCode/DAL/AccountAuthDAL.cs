﻿using System;
using System.Data;
using KKday.API.WMS.Models.DataModel.User;
using Npgsql;
namespace KKday.API.WMS.AppCode.DAL
{
    public class AccountAuthDAL
    {
        public static UserAccount UserAuth(string email, string password)
        {
            Npgsql.NpgsqlConnection conn = new Npgsql.NpgsqlConnection(Website.Instance.SqlConnectionString);
            UserAccount _account = null;

            try
            {
                // 連接Posgresql
                conn.Open();

                string sqlStmt = @"SELECT *, name_first || name_last AS Name 
FROM b2b.b2d_account_kkday 
WHERE enable=true AND LOWER(email)=LOWER(:email) AND password=:password";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[]{
                    new NpgsqlParameter("email", email),
                    new NpgsqlParameter("password", password)
                };

                var ds = NpgsqlHelper.ExecuteDataset(conn, CommandType.Text, sqlStmt, sqlParams);
                // 檢查是否為有效KKday使用者
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    _account = new KKdayAccount()
                    {
                        result ="00",
                        result_msg="OK",
                        ACCOUNT_TYPE = "KKDAY",
                        XID = dr.ToInt64("xid"),
                        UUID = dr.ToStringEx("user_uuid"),
                        EMAIL = dr.ToStringEx("email"),
                        NAME = dr.ToStringEx("name"),
                        NAME_FIRST = dr.ToStringEx("name_first"),
                        NAME_LAST = dr.ToStringEx("name_last"),
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
                else
                {
                    sqlStmt = @"SELECT a.xid, a.user_uuid, a.email, a.name_first, a.name_last,
 a.name_first || a.name_last AS name, a.department, a.job_title, a.enable, a.gender_title,
 b.xid as comp_xid, b.comp_name, b.comp_locale AS locale, b.comp_currency AS currency
FROM b2b.b2d_account a
JOIN b2b.b2d_company b ON a.company_xid=b.xid AND b.status='03' --已核准
WHERE enable=true AND LOWER(email)=LOWER(:account) AND password=:password";

                    sqlParams = new NpgsqlParameter[]{
                        new NpgsqlParameter("email", email),
                        new NpgsqlParameter("password", password)
                    };

                    ds = NpgsqlHelper.ExecuteDataset(conn, CommandType.Text, sqlStmt, sqlParams);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];

                        _account = new B2dAccount()
                        {
                            result = "00",
                            result_msg = "OK",
                            ACCOUNT_TYPE ="B2D",
                            XID = dr.ToInt64("xid"),
                            UUID = dr.ToStringEx("user_uuid"),
                            EMAIL = dr.ToStringEx("email"),
                            NAME = dr.ToStringEx("name"),
                            NAME_FIRST = dr.ToStringEx("name_first"),
                            NAME_LAST = dr.ToStringEx("name_last"),
                            COMPANY_XID = dr.ToInt64("comp_xid"),
                            COMPANY_NAME = dr.ToStringEx("comp_name"),
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
            catch (Exception ex)
            {
                if (conn.State != ConnectionState.Closed) conn.Close();
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                throw ex;
            }

            return _account;
        }

        //B2D API分銷商
        public static UserAccount UserApiAuth(string email)
        {
            Npgsql.NpgsqlConnection conn = new Npgsql.NpgsqlConnection(Website.Instance.SqlConnectionString);
            UserAccount _account = null;

            try
            {
                // 連接Posgresql
                conn.Open();

                String sqlStmt = @"SELECT B.comp_name,comp_locale,B.comp_currency,
                    B.contact_user_email, B.payment_type, A.*
                    FROM b2b.b2d_account_api A
                    JOIN b2b.b2d_company B ON A.company_xid = B.xid
                    WHERE A.enable = TRUE                      
                    AND A.email = :email
                    AND B.status = '01'";


                NpgsqlParameter[] sqlParams = new NpgsqlParameter[]{
                     new NpgsqlParameter("email",email)
                    };

                var ds = NpgsqlHelper.ExecuteDataset(conn, CommandType.Text, sqlStmt, sqlParams);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    _account = new B2dAccount()
                    {
                        result = "00",
                        result_msg = "OK",
                        ACCOUNT_TYPE = "API",
                        XID = dr.ToInt64("xid"),
                        UUID = dr.ToStringEx("user_uuid"),
                        EMAIL = dr.ToStringEx("email"),
                        NAME = dr.ToStringEx("name"),
                        NAME_FIRST = dr.ToStringEx("name_first"),
                        NAME_LAST = dr.ToStringEx("name_last"),
                        COMPANY_XID = dr.ToInt64("comp_xid"),
                        COMPANY_NAME = dr.ToStringEx("comp_name"),
                        DEPARTMENT = dr.ToStringEx("department"),
                        ENABLE = dr.ToBoolean("enable"),
                        GENDER_TITLE = dr.ToStringEx("gender_title"),
                        JOB_TITLE = dr.ToStringEx("job_title"),
                        CURRENCY = dr.ToStringEx("currency"),
                        LOCALE = dr.ToStringEx("locale")

                    };
                }
               
                conn.Close();
            }
            catch (Exception ex)
            {
                if (conn.State != ConnectionState.Closed) conn.Close();
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                throw ex;
            }

            return _account;
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
                Website.Instance.logger.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }

            return profile;
        }
    }
}
