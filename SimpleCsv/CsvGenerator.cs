using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

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
            PropertyInfo[] pinfo = GetObjectProperties(obj);

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryable"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="fileSavePath"></param>
        public void CollectionToCsvFile(IQueryable<object> queryable, string fileSavePath)
        {
            this.CollectionToCsvFile(queryable, fileSavePath, false);
        }

        /// <summary>
        /// Writes a UTF-16 encoded file
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="fileSavePath"></param>
        public void CollectionToCsvFile(IQueryable<object> queryable, string fileSavePath, bool includeHeaders)
        {
            FileStream fs = File.Create(fileSavePath);
            StreamWriter sw = new StreamWriter(fs);

            if (includeHeaders)
            {
                string header = GetCsvHeader(queryable);
                sw.WriteLine(header);
            }

            foreach (object item in queryable)
            {
                string rowString = this.ObjectToCsv(item);
                sw.WriteLine(rowString);
                
            }
            sw.Close();
        }

        #region Private Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private PropertyInfo[] GetObjectProperties(object obj)
        {
            Type t = obj.GetType();
            PropertyInfo[] pinfo = t.GetProperties();
            return pinfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryable"></param>
        /// <returns></returns>
        private string GetCsvHeader(IQueryable<object> queryable)
        {
            PropertyInfo[] pinfo = GetObjectProperties(queryable.First());
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < pinfo.Length; i++)
            {
                sb.Append("\"");//enclose every value in quotes
                sb.Append(pinfo[i].Name);
                sb.Append("\"");
                if (i < pinfo.Length - 1)
                {
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }
        #endregion
    }
}
