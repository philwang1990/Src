using System;
using System.Collections.Generic;
namespace KKday.API.WMS.Models.DataModel.Common

{

    public class CurrencyModel
    {

        public string result { get; set; }
        public string result_msg { get; set; }
        public List<Json> currencyList { get; set; }


    }

    public class Json
    {
        public string currency { get; set; }
        public string name { get; set; }
        public string param1 { get; set; } // CURRENCY API 本身就有的 都是null

    }

    //public class CurrencyModel
    //{

    //    public Json content { get; set; }


    //}

    //public class Json
    //{
    //    public string result { get; set; }
    //    public string msg { get; set; }
    //    public List<Json2> codeList { get; set; }


    //}


    //public class Json2
    //{
    //    public Json3 code { get; set; }
    //}

    //public class Json3
    //{
    //    public string dataCd { get; set; }
    //    public string dataName { get; set; }
    //    public string param1 { get; set; }


    //}


}