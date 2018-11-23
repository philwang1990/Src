using System;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;

namespace KKday.API.WMS.AppCode.DAL {
    public class DiscountDAL {

        /// <summary>
        /// Gets the black list.撈所有的黑名單
        /// </summary>
        /// <returns>The black list.</returns>

        public static JObject GetBlackList()
        {

            var obj = new JObject();

            try
            {

                String sql = @"SELECT b.prod_no                     
                               FROM b2b.b2d_list_price_blacks b ";

                DataSet ds = NpgsqlHelper.ExecuteDataset(Website.Instance.B2D_DB, CommandType.Text, sql.ToString());


                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {


                    ds.AcceptChanges();
                    //把dataset轉成結森物件
                    string json = JsonConvert.SerializeObject(ds, Formatting.Indented);

                    obj = JObject.Parse(json);
                }


            }
            catch (Exception ex)
            {


                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
            }

            return obj;

        }

        public static JObject GetDiscRuleList(Int64 comp_xid,string comp_currency)
        {
            var obj = new JObject();
            try
            {

                String sql = @"SELECT MST.*,DISC.*,coalesce(AMT.Amount,0) AS AMT,AMT.currency AS currency
FROM (
   SELECT A.xid AS mst_xid,
    (SELECT  regexp_replace(xpath('.//text()', ('<items>' || XMLAGG(XMLELEMENT(name C, CATE)) || '</items>')::xml)::text,  '[{}]', '', 'g') AS CATE
       FROM (
           SELECT mst_xid, disc_list||'^'||whitelist  AS CATE
           FROM b2b.b2d_discount_dtl dtl WHERE dtl.disc_type='type2'
            )G WHERE  G.mst_xid=A.xid
    )AS main_cat_wb,
    (SELECT  regexp_replace(xpath('.//text()', ('<items>' || XMLAGG(XMLELEMENT(name C, PRODNO)) || '</items>')::xml)::text,  '[{}]', '', 'g') AS PRODNO
       FROM (
           SELECT mst_xid, disc_list||'^'||whitelist  AS PRODNO
           FROM b2b.b2d_discount_dtl dtl WHERE dtl.disc_type='type1'
            )G WHERE  G.mst_xid=A.xid
    )AS prod_no_wb  ,
    (SELECT  regexp_replace(xpath('.//text()', ('<items>' || XMLAGG(XMLELEMENT(name C, dtl_xid)) || '</items>')::xml)::text, '[{}]', '', 'g') AS PRODNO
       FROM (
           SELECT mst_xid, dtl.xid AS dtl_xid
           FROM b2b.b2d_discount_dtl dtl
            )G WHERE  G.mst_xid=A.xid
    )AS disc_dtl_xid
   FROM b2b.b2d_discount_mst A
   GROUP BY A.xid
)DISC
INNER JOIN b2b.b2d_discount_mst MST ON DISC.mst_xid = MST.xid
LEFT JOIN b2b.b2d_comp_disc_map MAPP ON  MST.xid = MAPP.disc_mst_xid
LEFT JOIN b2b.b2d_company COMP ON MAPP.company_xid = COMP.xid
LEFT JOIN b2b.b2d_discount_curr_amt AMT ON MAPP.disc_mst_xid = AMT.mst_xid AND AMT.currency = :COMPANY_CURRENCY
WHERE  1=1
AND  TO_DATE(TO_CHAR(current_date,'YYYY-MM-dd'),'YYYY-MM-dd') BETWEEN MST.S_DATE AND MST.E_DATE  AND MST.status='01'
AND MAPP.company_xid = :COMPANY_XID
ORDER BY mst.xid";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                        new NpgsqlParameter("COMPANY_XID", comp_xid),
                    new NpgsqlParameter("COMPANY_CURRENCY", comp_currency)
                    };

                DataSet ds = NpgsqlHelper.ExecuteDataset(Website.Instance.B2D_DB, CommandType.Text, sql, sqlParams);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {

                    ds.AcceptChanges();
                    //把dataset轉成結森物件
                    string json = JsonConvert.SerializeObject(ds, Formatting.Indented);

                    obj = JObject.Parse(json);
                }

            }
            catch (Exception ex)
            {

                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
            }

            return obj;
        }

        public static JObject GetFixedPriceList(Int64 comp_xid,string prod_no)
        {
            var obj = new JObject();

            try
            {
                String sql = @"SELECT a.prod_no,b.pkg_no,c.price_cond,c.price
FROM b2b.b2d_fixedprice_prod a 
LEFT JOIN b2b.b2d_fixedprice_prod_pkg b ON b.prod_xid = a.xid
LEFT JOIN b2b.b2d_fixedprice_pkg_price c ON b.xid = c.pkg_xid
WHERE a.company_xid = :COMPANY_XID AND a.prod_no=:PROD_NO ";


                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("COMPANY_XID", comp_xid),
                    new NpgsqlParameter("PROD_NO", prod_no)
                };

                DataSet ds = NpgsqlHelper.ExecuteDataset(Website.Instance.B2D_DB, CommandType.Text, sql, sqlParams);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    ds.AcceptChanges();
                    //把dataset轉成結森物件
                    string json = JsonConvert.SerializeObject(ds, Formatting.Indented);

                    obj = JObject.Parse(json);
                }


            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
            }

            return obj;

        }
    }

}
