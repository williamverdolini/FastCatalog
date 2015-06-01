using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using SQLCommon.Models;

namespace SQLCommon.Logic
{
    public static class SQLExtensions
    {
        public static List<string> ToStringList(this List<SQLSynonim> synonims)
        {
            List<string> strings = new List<string>();
            foreach (var s in synonims)
            {
                strings.Add(s.Code);
            }
            return strings;
        }

        public static T FromXmlTo<T>(this String xml)
        {
            T returnedXmlClass = default(T);

            try
            {
                using (TextReader reader = new StringReader(xml))
                {
                    try
                    {
                        returnedXmlClass = (T)new XmlSerializer(typeof(T)).Deserialize(reader);
                    }
                    catch (InvalidOperationException)
                    {
                        // String passed is not XML, simply return defaultXmlClass
                        throw;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnedXmlClass;
        }
    }
}
