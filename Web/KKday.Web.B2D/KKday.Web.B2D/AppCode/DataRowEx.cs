using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

public static class DataRowEx
{
    public static DateTime ToDateTime(this DataRow dr, string column_name)
    {
        if (dr.IsNull(column_name)) return DateTime.MinValue;
        return Convert.ToDateTime(dr[column_name]);
    }

    public static DateTime ToDateTime(this DataRow dr, string column_name, string dt_format)
    {
        if (dr.IsNull(column_name)) return DateTime.MinValue;
        IFormatProvider ifp = new System.Globalization.CultureInfo("zh-TW", true);
        DateTime dt = DateTime.ParseExact(dr[column_name].ToString(), dt_format, ifp);
        return dt;
    }

    public static DateTime? ToDateTimeEx(this DataRow dr, string column_name)
    {
        DateTime? dt = null;

        if (dr.IsNull(column_name)) return dt;
        return Convert.ToDateTime(dr[column_name]);
    }

    public static DateTime? ToDateTimeEx(this DataRow dr, string column_name, string dt_format)
    {
        DateTime? dt = null;

        if (dr.IsNull(column_name)) return dt;
        
        IFormatProvider ifp = new System.Globalization.CultureInfo("zh-TW", true);
        dt = DateTime.ParseExact(dr[column_name].ToString(), dt_format, ifp);
        return dt;
    }


    public static string ToStringEx(this DataRow dr, string column_name)
    {
        if (dr.IsNull(column_name)) return string.Empty;
        return dr[column_name].ToString();
    }

    public static bool ToBoolean(this DataRow dr, string column_name)
    {
        if (dr.IsNull(column_name)) return false;
        return Convert.ToBoolean(dr[column_name]);
    }

    public static double ToDouble(this DataRow dr, string column_name)
    {
        if (dr.IsNull(column_name)) return 0.0f;
        return Convert.ToDouble(dr[column_name]);
    }

    public static float ToSingle(this DataRow dr, string column_name)
    {
        if (dr.IsNull(column_name)) return 0.0f;
        return Convert.ToSingle(dr[column_name]);
    }

    public static Int64 ToInt64(this DataRow dr, string column_name)
    {
        if (dr.IsNull(column_name)) return 0;
        return Convert.ToInt64(dr[column_name]);
    }

    public static Int32 ToInt32(this DataRow dr, string column_name)
    {
        if (dr.IsNull(column_name)) return 0;
        return Convert.ToInt32(dr[column_name]);
    }
}