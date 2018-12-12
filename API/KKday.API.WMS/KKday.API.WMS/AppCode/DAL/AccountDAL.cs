using System;
using System.Data;
using KKday.API.WMS.Models.DataModel.Account;
using Npgsql;
using KKday.API.WMS.AppCode;
namespace KKday.API.WMS.AppCode.DAL
{
    public class AccountDAL
    {
        public static int RegisterIs4User(AccountModel model)
        {
            int count = 0;

            try
            {

                NpgsqlConnection conn = new NpgsqlConnection(Website.Instance.Configuration["ConnectionStrings:NpgsqlConnectionIs4"]);
                conn.Open();
                NpgsqlTransaction trans = conn.BeginTransaction();
                String sql = null;
                NpgsqlParameter[] np = null;


                sql = @"INSERT INTO is4.users(
                    user_no, source_type, user_pass, status, xid)
                    VALUES (:user_no, :source_type, :user_pass, :status, Nextval('is4.user_xid_seq'));";

                if (model.status == null)
                    model.status = "00";

                np = new NpgsqlParameter[]{
                     new NpgsqlParameter("user_no",model.user_no),
                     new NpgsqlParameter("source_type",model.source_type),
                     new NpgsqlParameter("user_pass",Sha256Helper.Gethash(model.user_pass)),
                     new NpgsqlParameter("status",model.status) // 預設00 01才是可以使用的status

            };

                count = NpgsqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, np);

                trans.Commit();

                conn.Close();


            }
            catch (Exception ex)
            {

                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                throw ex;
            }

            return count;


        }

        public static int UpdateIs4User(AccountModel model)
        {
            int count = 0;
            try
            {

                NpgsqlConnection conn = new NpgsqlConnection(Website.Instance.Configuration["ConnectionStrings:NpgsqlConnectionIs4"]);
            conn.Open();
            NpgsqlTransaction trans = conn.BeginTransaction();
            String sql = null;
            NpgsqlParameter[] np = null;
            

            sql = @"UPDATE is4.users 
                    SET source_type = :source_type,user_pass = coalesce(:user_pass,user_pass),status = :status
                    WHERE 1=1
                    AND user_no = :user_no;";



            np = new NpgsqlParameter[]{
                new NpgsqlParameter("source_type",model.source_type),
                new NpgsqlParameter("user_pass",model.user_pass=="" ?null : Sha256Helper.Gethash(model.user_pass)),
                new NpgsqlParameter("status",model.status), // 預設00 01才是可以使用的status
                new NpgsqlParameter("user_no",model.user_no),

            };

            count = NpgsqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, np);

            trans.Commit();

            conn.Close();

            }
            catch (Exception ex)
            {

                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                throw ex;
            }

            return count;

        }
    }
}
