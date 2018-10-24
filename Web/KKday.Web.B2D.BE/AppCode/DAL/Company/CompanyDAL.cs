using System;
using System.Collections.Generic;
using System.Data;
using KKday.Web.B2D.BE.App_Code;
using KKday.Web.B2D.BE.Areas.KKday.Models.DataModel;
using KKday.Web.B2D.BE.Models.Model.Company;
using Npgsql;

namespace KKday.Web.B2D.BE.AppCode.DAL.Company
{
    public class CompanyDAL
    {
        public static int GetCompanyCount(string filter)
        {
            try
            {
                string sqlStmt = @"SELECT COUNT(*) FROM b2b.b2d_company
WHERE 1=1 {FILTER}";

                sqlStmt = sqlStmt.Replace("{FILTER}", !string.IsNullOrEmpty(filter) ? filter : string.Empty);

                int total_count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt));
                return total_count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<B2dCompany> GetCompanies(string filter, int skip, int size, string sorting)
        {
            try
            {
                List<B2dCompany> companies = new List<B2dCompany>();

                string sqlStmt = @"SELECT * FROM b2b.b2d_company
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
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    companies.Add(new B2dCompany()
                    {
                        XID = dr.ToInt64("xid"),
                        STATUS = dr.ToStringEx("status"),
                        COMP_COOP_MODE = dr.ToStringEx("comp_coop_mode"),
                        PAYMENT_TYPE = dr.ToStringEx("payment_type"),
                        MANAGER_ACCOUNT_XID = dr.ToInt64("manager_account_xid"),
                        PARENT_COMP_XID = dr.ToInt64("parent_comp_xid"),
                        COMP_NAME = dr.ToStringEx("comp_name"),
                        COMP_URL = dr.ToStringEx("comp_url"),
                        COMP_LICENSE = dr.ToStringEx("comp_license"),
                        COMP_LICENSE_2 = dr.ToStringEx("comp_license_2"),
                        COMP_LOCALE = dr.ToStringEx("comp_locale"),
                        COMP_CURRENCY = dr.ToStringEx("comp_currency"),
                        COMP_INVOICE = dr.ToStringEx("comp_invoice"),
                        COMP_COUNTRY = dr.ToStringEx("comp_country"),
                        COMP_TEL_COUNTRY_CODE = dr.ToStringEx("comp_tel_country_code"),
                        COMP_TEL = dr.ToStringEx("comp_tel"),
                        COMP_ADDRESS = dr.ToStringEx("comp_address"),
                        CHARGE_MAN_FIRST = dr.ToStringEx("charge_man_first"),
                        CHARGE_MAN_LAST = dr.ToStringEx("charge_man_last"),
                        CREDITCARD_NO = dr.ToStringEx("creditcard_no"),
                        CREDITCARD_VALID = dr.ToStringEx("creditcard_valid"),
                        CREDITCARD_CVC = dr.ToStringEx("creditcard_cvc"),
                        CONTACT_USER = dr.ToStringEx("contact_user"),
                        CONTACT_USER_EMAIL = dr.ToStringEx("contact_user_email"),
                        FINANCE_USER = dr.ToStringEx("finance_user"),
                        SALES_USER = dr.ToStringEx("sales_user")
                    });
                }

                return companies;
            }
            catch (Exception ex)
            {
                Website.Instance._log.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }
        
        public static B2dCompany GetCompany(Int64 xid)
        {
            try
            {
                string sqlStmt = @"SELECT * FROM b2b.b2d_company WHERE xid=:XID";

                DataSet ds = NpgsqlHelper.ExecuteDataset(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, new NpgsqlParameter("XID", xid));
                DataRow dr = ds.Tables[0].Rows[0];

                var company = new B2dCompany()
                {
                    XID = dr.ToInt64("xid"),
                    STATUS = dr.ToStringEx("status"),
                    COMP_COOP_MODE = dr.ToStringEx("comp_coop_mode"),
                    PAYMENT_TYPE = dr.ToStringEx("payment_type"),
                    MANAGER_ACCOUNT_XID = dr.ToInt64("manager_account_xid"),
                    PARENT_COMP_XID = dr.ToInt64("parent_comp_xid"),
                    COMP_NAME = dr.ToStringEx("comp_name"),
                    COMP_URL = dr.ToStringEx("comp_url"),
                    COMP_LICENSE = dr.ToStringEx("comp_license"),
                    COMP_LICENSE_2 = dr.ToStringEx("comp_license_2"),
                    COMP_LOCALE = dr.ToStringEx("comp_locale"),
                    COMP_CURRENCY = dr.ToStringEx("comp_currency"),
                    COMP_INVOICE = dr.ToStringEx("comp_invoice"),
                    COMP_COUNTRY = dr.ToStringEx("comp_country"),
                    COMP_TEL_COUNTRY_CODE = dr.ToStringEx("comp_tel_country_code"),
                    COMP_TEL = dr.ToStringEx("comp_tel"),
                    COMP_ADDRESS = dr.ToStringEx("comp_address"),
                    CHARGE_MAN_FIRST = dr.ToStringEx("charge_man_first"),
                    CHARGE_MAN_LAST = dr.ToStringEx("charge_man_last"),
                    CREDITCARD_NO = dr.ToStringEx("creditcard_no"),
                    CREDITCARD_VALID = dr.ToStringEx("creditcard_valid"),
                    CREDITCARD_CVC = dr.ToStringEx("creditcard_cvc"),
                    CONTACT_USER = dr.ToStringEx("contact_user"),
                    CONTACT_USER_EMAIL = dr.ToStringEx("contact_user_email"),
                    FINANCE_USER = dr.ToStringEx("finance_user"),
                    SALES_USER = dr.ToStringEx("sales_user")
                };

                return company;
            }
            catch (Exception ex)
            {
                Website.Instance._log.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        ////////////////
          
        public static void UpdateCompany(CompanyUpdModel company, string upd_user)
        {
            try
            {
                string sqlStmt = @"UPDATE b2b.b2d_company SET 
 status=:STATUS, comp_coop_mode=:COMP_COOP_MODE, payment_type=:PAYMENT_TYPE, comp_name=:COMP_NAME, 
 comp_url=:COMP_URL, comp_locale=:COMP_LOCALE, comp_currency=:COMP_CURRENCY, comp_invoice=:COMP_INVOICE, 
 comp_country=:COMP_COUNTRY, comp_tel_country_code=:COMP_TEL_COUNTRY_CODE, comp_tel=:COMP_TEL,
 comp_address=:COMP_ADDRESS, contact_user=:CONTACT_USER, contact_user_email=:CONTACT_USER_EMAIL,
 finance_user=:FINANCE_USER, sales_user=:SALES_USER,
 upd_user=:UPD_USER, upd_datetime=Now()
WHERE xid=:XID";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("XID", company.XID),
                    new NpgsqlParameter("STATUS", company.STATUS),
                    new NpgsqlParameter("COMP_COOP_MODE", company.COOP_MODE),
                    new NpgsqlParameter("PAYMENT_TYPE", company.PAYMENT_TYPE), 
                    new NpgsqlParameter("COMP_NAME", company.NAME),
                    new NpgsqlParameter("COMP_URL", company.URL), 
                    new NpgsqlParameter("COMP_LOCALE", company.LOCALE),
                    new NpgsqlParameter("COMP_CURRENCY", company.CURRENCY),
                    new NpgsqlParameter("COMP_INVOICE", company.INVOICE),
                    new NpgsqlParameter("COMP_COUNTRY", company.COUNTRY),
                    new NpgsqlParameter("COMP_TEL_COUNTRY_CODE", company.TEL_COUNTRY_CODE),
                    new NpgsqlParameter("COMP_TEL", company.TEL),
                    new NpgsqlParameter("COMP_ADDRESS", company.ADDRESS),
                    new NpgsqlParameter("CONTACT_USER", company.CONTACT_USER),
                    new NpgsqlParameter("CONTACT_USER_EMAIL", company.CONTACT_USER_EMAIL),
                    new NpgsqlParameter("FINANCE_USER", company.FINANCE_USER),
                    new NpgsqlParameter("SALES_USER", company.SALES_USER),
                    //new NpgsqlParameter("CHARGE_MAN_FIRST", company.CHARGE_MAN_FIRST),
                    //new NpgsqlParameter("CHARGE_MAN_LAST", company.CHARGE_MAN_LAST),
                    //new NpgsqlParameter("CREDITCARD_NO", company.CREDITCARD_NO),
                    //new NpgsqlParameter("CREDITCARD_VALID", company.CREDITCARD_VALID),
                    //new NpgsqlParameter("CREDITCARD_CVC", company.CREDITCARD_CVC)
                    new NpgsqlParameter("UPD_USER", upd_user)
                };

                NpgsqlHelper.ExecuteNonQuery(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);
            }
            catch (Exception ex)
            {
                Website.Instance._log.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static void UpdateStatus(Int64 xid, string status, string upd_user)
        {
            try
            {
                string sqlStmt = @"UPDATE b2b.b2d_company SET status=:STATUS,
upd_user=:UPD_USER, upd_datetime=Now() WHERE xid=:XID";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("XID", xid),
                    new NpgsqlParameter("STATUS", status),
                    new NpgsqlParameter("UPD_USER", upd_user)
                };
                 
                NpgsqlHelper.ExecuteNonQuery(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
