using System;
using System.IO;
using System.Reflection.Emit;
using System.Runtime.Serialization;

public static class ClassMapping
{
    /*
    public static T Clone<T>(this T obj)
    {
        var inst = obj.GetType().GetMethod("MemberwiseClone", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

        return (T)inst?.Invoke(obj, null);
    }*/

    public static T Clone<T>(this T obj)
    {
        DataContractSerializer dcSer = new DataContractSerializer(obj.GetType());
        MemoryStream memoryStream = new MemoryStream();

        dcSer.WriteObject(memoryStream, obj);
        memoryStream.Position = 0;

        T newObject = (T)dcSer.ReadObject(memoryStream);
        return newObject;
    }


    public static void CopyPropertiesFrom(this object self, object parent)
    {
        var fromProperties = parent.GetType().GetProperties();
        var toProperties = self.GetType().GetProperties();
         
        foreach (var fromProperty in fromProperties)
        {
            foreach (var toProperty in toProperties)
            {

                if (fromProperty.Name == toProperty.Name && fromProperty.PropertyType == toProperty.PropertyType)
                {
                    if (toProperty.CanWrite)
                    {
                        toProperty.SetValue(self, fromProperty.GetValue(parent));
                        break;
                    }
                } 
            }
        }

    }
}
