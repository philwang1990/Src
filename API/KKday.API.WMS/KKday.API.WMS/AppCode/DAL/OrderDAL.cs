using System;
using System.Data;
using Npgsql;

namespace KKday.API.WMS.AppCode.DAL
{
    public class OrderDAL
    {
        //order_no 找出 orderMid(KODxxxx>>18KKxxxxx)
        public static bool CheckOrder(Int64 xid ,string order_no,ref string orderMid)
        {
            bool check = false;

            try
            {

                String sql = @"select kkday_order_mid 
from b2b.orders a
join b2b.order_source b on a.order_no = b.order_no 
where b.company_xid = :COMPANY_XID and a.order_no =:ORDER_NO ";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                        new NpgsqlParameter("COMPANY_XID", xid),
                    new NpgsqlParameter("ORDER_NO", order_no)
                    };

                DataSet ds = NpgsqlHelper.ExecuteDataset(Website.Instance.B2D_DB, CommandType.Text, sql, sqlParams);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    orderMid = ds.Tables[0].Rows[0]["kkday_order_mid"].ToString();
                    check = true;
                }
            }
            catch (Exception ex)
            {
                 Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
            }
            return check;
        }
        //orderMid(KODxxxx>>18KKxxxxx) 找出 order_no 
        public static bool CheckOrder(Int64 xid, string orderMid, string  orderOid ,ref string order_no)
        {
            bool check = false;

            try
            {

                String sql = @"select a.order_no 
from b2b.orders a
join b2b.order_source b on a.order_no = b.order_no 
where b.company_xid = :COMPANY_XID and a.kkday_order_oid =:ORDER_OID and a.kkday_order_mid=:ORDER_MID";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("COMPANY_XID", xid),
                    new NpgsqlParameter("ORDER_OID", orderOid),
                    new NpgsqlParameter("ORDER_MID", orderMid)
                    };

                DataSet ds = NpgsqlHelper.ExecuteDataset(Website.Instance.B2D_DB, CommandType.Text, sql, sqlParams);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    order_no = ds.Tables[0].Rows[0]["order_no"].ToString();
                    check = true;
                }
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
            }
            return check;
        }
    }
}
