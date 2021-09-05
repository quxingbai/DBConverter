using DBConverter.Base;
using DBConverter.Base.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConverter
{
    public class DBConvert
    {
        private static Dictionary<string, TableDataType> MySqlColumnData = new Dictionary<string, TableDataType>();
        private static Dictionary<string, TableDataType> SqlServerColumnData = new Dictionary<string, TableDataType>();

        static DBConvert()
        {
            #region MySql

            MySqlColumnData.Add("BIT", TableDataType.Boolean);
            MySqlColumnData.Add("CHAR", TableDataType.Char);
            MySqlColumnData.Add("DATETIME", TableDataType.DateTime);
            MySqlColumnData.Add("DECIMAL", TableDataType.Decimal);
            MySqlColumnData.Add("INT", TableDataType.Int);
            MySqlColumnData.Add("BIGINT", TableDataType.Long);
            MySqlColumnData.Add("VARCHAR", TableDataType.String);
            MySqlColumnData.Add("NVARCHAR", TableDataType.String);

            #endregion

            #region SqlServer

            SqlServerColumnData.Add("BIT", TableDataType.Boolean);
            SqlServerColumnData.Add("CHAR", TableDataType.Char);
            SqlServerColumnData.Add("DATETIME", TableDataType.DateTime);
            SqlServerColumnData.Add("MONEY", TableDataType.Decimal);
            SqlServerColumnData.Add("INT", TableDataType.Int);
            SqlServerColumnData.Add("BIGINT", TableDataType.Long);
            SqlServerColumnData.Add("VARCHAR", TableDataType.String);
            SqlServerColumnData.Add("NVARCHAR", TableDataType.String);

            #endregion
        }
        public static String ToSQL_MySqlToSqlServer(DataBaseBasicModel DBModel)
        {
            String GO = " GO";
            String CreateDB = $"CREATE DATABASE {DBModel.Name};\n{GO}\n USE {DBModel.Name};\n{GO}\n";
            StringBuilder CreateTable = new StringBuilder();
            StringBuilder InsertData = new StringBuilder();

            foreach (var tb in DBModel.Tables)
            {
                StringBuilder ct = new StringBuilder();
                ct.AppendLine($"CREATE TABLE {tb.Name}");
                ct.AppendLine("(");
                for (int f = 0; f < tb.Columns.Count; f++)
                {
                    var col = tb.Columns[f];
                    //ct.Append($"{col.Name} {ColumnDataTypeConvert(MySqlColumnData, SqlServerColumnData, col)}");
                    ct.Append($"[{col.Name}] {col.DataType.MySqlToSqlServer().FullName}");
                    if (col.IsKey)
                    {
                        ct.Append(" PRIMARY KEY IDENTITY(1,1)");

                    }
                    if (col.IsNullBle)
                    {
                        ct.Append(" NOT NULL");
                    }
                    if (f != tb.Columns.Count - 1)
                    {
                        ct.Append(',');
                    }
                    ct.AppendLine();
                }
                ct.AppendLine(");");
                ct.AppendLine();
                ct.Append(GO);
                ct.AppendLine();
                ct.AppendLine();

                //添加一个表的SQL
                CreateTable.Append(ct.ToString());

                //添加表的数据
                //Insert行
                foreach (var row in tb.Rows)
                {
                    var keys = tb.Columns.Where(i => i.IsKey);
                    string key = keys.Count() > 0 ? keys.First().Name : "";
                    InsertData.AppendLine(ToMySqlInserSql(tb.Name, row, new string[] { key }));
                }
            }
            return CreateDB + CreateTable.ToString() + InsertData;
        }

        /// <summary>
        /// 将Mode变为Insert字符串，使用的dynamic
        /// </summary>
        public static string ToMySqlInserSql(String TableName, dynamic Source, String[] BlackMember)
        {
            string param = "INSERT INTO " + TableName + "(";
            string value = "VALUES(";
            int p = 0;

            foreach (var f in Source)
            {
                string n = Convert.ToString(f.Key);
                if (BlackMember.Contains(n))
                {
                    continue;
                }
                p += 1;
                if (p > 1)
                {
                    value += ",";
                    param += ",";
                }

                bool isValue = int.TryParse(Convert.ToString(f.Value), out int ValueVal);

                param += "[" + f.Key + "]";
                value += isValue ? ValueVal : "'" + f.Value + "'";

            }
            param += ")";
            value += ");\n GO";

            return param + " " + value;
        }

        /// <summary>
        /// 将某数据库个数据类型变为另一个数据库数据类型
        /// </summary>
        /// <returns></returns>
        //public static String ColumnDataTypeConvert(Dictionary<string, TableDataType> Source, Dictionary<string, TableDataType> To, ColumnBasicModel column)
        //{
        //    var sd = Source.Where(s => s.Key.ToString().ToUpper() == column.DataType.ToUpper()).First();
        //    var datatype = To.Where(t => t.Value == sd.Value).First().Key;
        //    var size = column.DataType.ToUpper().Replace(datatype, "");

        //    return datatype + size;
        //}
    }
}
