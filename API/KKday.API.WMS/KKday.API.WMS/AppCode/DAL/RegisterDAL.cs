using System;
using System.Data;
using KKday.API.WMS.Models.DataModel.Account;
using Npgsql;

namespace KKday.API.WMS.AppCode.DAL
{
    public class RegisterDAL
    {
        // 分銷商註冊
        public static void InsCompany(RegisterRQModel reg, ref RegisterRSModel rs)
        {


            NpgsqlConnection conn = new NpgsqlConnection(Website.Instance.Configuration["ConnectionStrings:NPGSQL_Connection"]);
        
            NpgsqlTransaction trans = null;

            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                string sqlStmt = @"INSERT INTO b2b.b2d_company(
 status, comp_coop_mode, payment_type, manager_account_xid, comp_name, comp_url, comp_license,
 comp_license2, comp_country, comp_locale, comp_currency, comp_invoice, comp_tel_country_code, 
 comp_tel, contact_user_email, comp_address, charge_man_first, charge_man_last, contact_user,
 charge_man_gender, comp_timezone,vou_logo_url,vou_name,vou_tel,vou_email,vou_address,vou_description,
 crt_user, crt_datetime)
VALUES (:status, :comp_coop_mode, :payment_type, :manager_account_xid, :comp_name, :comp_url, :comp_license,
 :comp_license_2, :comp_country, :comp_locale, :comp_currency, :comp_invoice, :comp_tel_country_code, :comp_tel,
 :contact_user_email, :comp_address, :charge_man_first, :charge_man_last, :contact_user, 
 :charge_man_gender, :comp_timezone,:vou_logo_url,:vou_name,:vou_tel,:vou_email,:vou_address,:vou_description,
 :crt_user, now() );
SELECT currval('b2b.b2d_company_xid_seq') AS new_comp_xid;
";


                NpgsqlParameter[] sqlParams = new NpgsqlParameter[]
                {


                    new NpgsqlParameter("status", "00"),            //審核狀態(00已申請/01審核中/02待補件/03已核准/04未核准)
                    new NpgsqlParameter("comp_coop_mode", "02"),    //合作方式(00全開/01串接API/02Web平台)
                    new NpgsqlParameter("payment_type", "01"),      //付款方式(01逐筆結/02額度付款)
                    new NpgsqlParameter("manager_account_xid", 1),
                    new NpgsqlParameter("comp_name", reg.COMPANY_NAME),
                    new NpgsqlParameter("comp_url", reg.URL),
                    new NpgsqlParameter("comp_license", reg.LICENCSE_1),
                    new NpgsqlParameter("comp_license_2", reg.LICENCSE_2),
                    new NpgsqlParameter("comp_locale", reg.LOCALE),
                    new NpgsqlParameter("comp_currency", reg.CURRENCY),
                    new NpgsqlParameter("comp_invoice", reg.INVOICE),
                    new NpgsqlParameter("comp_country", reg.COUNTRY_CODE),
                    new NpgsqlParameter("comp_tel_country_code", reg.TEL_CODE),
                    new NpgsqlParameter("comp_tel", reg.TEL),
                    new NpgsqlParameter("contact_user_email", reg.EMAIL),
                    new NpgsqlParameter("comp_address", reg.ADDRESS),
                    new NpgsqlParameter("charge_man_first",reg.NAME_FIRST),
                    new NpgsqlParameter("charge_man_last",reg.NAME_LAST),
                    new NpgsqlParameter("contact_user", string.Empty),
                    new NpgsqlParameter("crt_user", "system"),
                    new NpgsqlParameter("charge_man_gender", reg.GENDER_TITLE),
                    new NpgsqlParameter("comp_timezone", Convert.ToInt32(reg.TIMEZONE)),
                    new NpgsqlParameter("vou_logo_url", ""),
                    new NpgsqlParameter("vou_name", reg.COMPANY_NAME),
                    new NpgsqlParameter("vou_tel", string.Format("{0}{1}",reg.TEL_CODE,reg.TEL)),
                    new NpgsqlParameter("vou_email", reg.EMAIL),
                    new NpgsqlParameter("vou_address", reg.ADDRESS),
                    new NpgsqlParameter("vou_description", "")

                };

                var new_comp_xid = NpgsqlHelper.ExecuteScalar(trans, CommandType.Text, sqlStmt, sqlParams);
                var new_acc_xid = InsAccount(trans, reg, new_comp_xid);
                UpdManagerXid(trans, new_comp_xid, new_acc_xid);

                rs.result = "00";
                rs.result_msg = "OK";
                rs.STATUS = "00";
                rs.COMPANY_XID = Int64.Parse(new_comp_xid.ToString());

                trans.Commit();
                conn.Close();

            }
            catch (Exception ex)
            {
                rs.result = "10001";
                rs.result_msg = ex.Message;
                rs.STATUS = null;
                rs.COMPANY_XID = 0;


                Website.Instance.logger.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                if (trans != null) trans.Rollback();
                conn.Close();
                throw ex;
            }
        }

        // 新增分銷商主帳號
        public static string InsAccount(NpgsqlTransaction trans, RegisterRQModel reg, object comp_xid)
        {
            try
            {
                string sqlStmt = @"INSERT INTO b2b.b2d_account(company_xid, account_type, enable, is_api, api_token, password,
name_last, name_first, gender_title, email, tel, crt_user, crt_datetime, job_title, user_uuid)
VALUES (:company_xid, :account_type, :enable, :is_api, :api_token, :password,:name_last, :name_first, :gender_title, 
:email, :tel, :crt_user, now(), :job_title, :user_uuid);
SELECT currval('b2b.b2d_account_xid_seq') AS new_comp_xid ;";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("company_xid",comp_xid),
                    new NpgsqlParameter("account_type","01"),   //帳號權限(00一般/01管理者)
                    new NpgsqlParameter("enable",true),         //是否有效(true/false) 
                    new NpgsqlParameter("password",reg.PASSWORD),
                    new NpgsqlParameter("gender_title",reg.GENDER_TITLE),
                    new NpgsqlParameter("name_last",reg.NAME_LAST),
                    new NpgsqlParameter("name_first",reg.NAME_FIRST),
                    new NpgsqlParameter("job_title",reg.JOB_TITLE),
                    new NpgsqlParameter("email",reg.EMAIL),
                    new NpgsqlParameter("tel",reg.TEL),
                    new NpgsqlParameter("crt_user","system"),
                    new NpgsqlParameter("user_uuid",reg.USER_UUID),

                    //20181212欄位調整
                    new NpgsqlParameter("is_api", false),
                    new NpgsqlParameter("api_token", "")
                };

                var new_xid = NpgsqlHelper.ExecuteScalar(trans, CommandType.Text, sqlStmt, sqlParams);

                return new_xid.ToString();
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        // 更新主帳號xid
        public static void UpdManagerXid(NpgsqlTransaction trans, object comp_xid, object acc_xid)
        {
            try
            {
                string sqlStmt = @"UPDATE b2b.b2d_company
SET manager_account_xid=" + acc_xid + @"
WHERE xid=" + comp_xid + ";";

                NpgsqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlStmt);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
