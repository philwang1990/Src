using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.IO;

class XMLTool
{
    //反序列化
    public static object XMLDeSerialize(string XmlStr, string TypeName)
    {
        try
        {
            XmlSerializer Des = new XmlSerializer(Type.GetType(TypeName));
            System.IO.StringReader Sr = new System.IO.StringReader(XmlStr);
            return Des.Deserialize(Sr);
        }
        catch (Exception)
        {
            //throw ex;
            return null;
        }
    }

    //序列化
    public static string XMLSerialize(object obj)
    {
        try
        {
            XmlSerializer s = new XmlSerializer(obj.GetType());
            System.IO.StringWriter Sw = new System.IO.StringWriter();
            s.Serialize(Sw, obj);
            return Sw.ToString();
        }
        catch (Exception)
        {
            //throw ex;
            return null;
        }
    }
}
