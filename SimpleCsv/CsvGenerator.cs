using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SimpleCsv
{
    public class CsvGenerator
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public CsvGenerator()
        {
        }


        public string ObjectToCsv(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj", "Value can not be null or Nothing!");
            }

            StringBuilder sb = new StringBuilder();
            Type t = obj.GetType();
            PropertyInfo[] pinfo = t.GetProperties();

            for (int i = 0; i < pinfo.Length; i++)
            {
                sb.Append("\"");//enclose every value in quotes
                sb.Append(pinfo[i].GetValue(obj, null));
                sb.Append("\"");
                if (i < pinfo.Length - 1)
                {
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }

        public string CollectionToCsv(IQueryable<object> queryable)
        {
            if(queryable == null)
                return null;
            StringBuilder sb = new StringBuilder();
            foreach(object item in queryable)
            {
                sb.AppendLine(this.ObjectToCsv(item));
            }

            return sb.ToString();
        }
    }
}
