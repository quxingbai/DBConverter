using DBConverter.Base.AbsDBEntrys;
using DBConverter.Base.Interface;
using DBConverter.Base.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConverter.Base.Help
{
    /// <summary>
    /// MySql帮助
    /// </summary>
    public static class MySqlHelp
    {

        public static int ExecuteNoQuery(MySqlConnection connection, string Sql, DbParameter[] parameters = null)
        {
            var cmd = connection.CreateCommand();
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }
            var q = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return q;
        }

        public static List<dynamic> ExecuteQuery(MySqlConnection connection, string Sql, DbParameter[] parameters = null)
        {
            List<dynamic> list = new List<dynamic>();
            var cmd = connection.CreateCommand();
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }
            cmd.CommandText = Sql;
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ExpandoObject e = new ExpandoObject();
                for (int f = 0; f < reader.FieldCount; f++)
                {
                    var name = reader.GetName(f);
                    var value = reader[f];
                    if (value is DateTime)
                    {
                        value = value.ToString();
                    }
                    (e as IDictionary<string, object>).Add(name, value);
                }
                list.Add(e);
            }
            cmd.Dispose();
            reader.Dispose();
            return list;
        }

        public static List<dynamic> QueryAllDataBase(MySqlConnection connection, String Where = "")
        {
            return MySqlHelp.ExecuteQuery(connection, "SELECT * FROM INFORMATION_SCHEMA.SCHEMATA " + Where);
        }

        public static List<dynamic> QueryAllTables(MySqlConnection connection, String Where = "")
        {
            return ExecuteQuery(connection, "SELECT * FROM INFORMATION_SCHEMA.TABLES " + Where);
        }

        public static List<dynamic>QueryAllProcedures(MySqlConnection connection ,String Where = "")
        {
            return ExecuteQuery(connection, "SHOW PROCEDURE STATUS " + Where);
        }

        public static List<dynamic> QueryAllFunctions(MySqlConnection connection, String Where = "")
        {
            return ExecuteQuery(connection, "SHOW FUNCTION STATUS " + Where);
        }
        public static ProcedureBasicModel QueryProcedureCreateSql(MySqlConnection connection, String DBName, String ProcedureName)
        {
            var d = ExecuteQuery(connection, "    SHOW CREATE PROCEDURE " + DBName + "." + ProcedureName).First();

            return new ProcedureBasicModel()
            {
                Content = (d as IDictionary<string,Object>)["Create Procedure"].ToString(),
                Source = d,
                Name = ProcedureName,
                DataBase = new DataBaseBasicModel()
                {
                    Name = DBName
                }
            };
        }
        public static FunctionBasicModel QueryFunctionCreateSql(MySqlConnection connection, String DBName, String FuncName)
        {
            var d = ExecuteQuery(connection, "    SHOW CREATE FUNCTION " + DBName + "." + FuncName).First();
            return new FunctionBasicModel()
            {
                Content = (d as IDictionary<string,Object>)["Create Function"].ToString(),
                Source =d,
                Name= FuncName,
                DataBase=new DataBaseBasicModel()
                {
                    Name=DBName
                }
            };
        }
        /// <summary>
        /// 查询所有字段
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="Where"> TABLE_SCHEMA='数据库' AND TABLE_NAME='表' </param>
        /// <returns></returns>
        public static List<dynamic> QueryAllColumns(MySqlConnection connection, String Where = "")
        {
            return ExecuteQuery(connection, "SELECT * FROM INFORMATION_SCHEMA.COLUMNS " + Where);
        }


        public static MySqlConnection Open(string Server, int Port, string UName, string UPassword, string DBName,String Rest= "Allow Zero Datetime=True")
        {
            return new MySqlConnection($"SERVER={Server};PORT={Port};USER={UName};PASSWORD={UPassword};DATABASE={DBName};"+Rest);
        }
        public static String OpenStr(string Server, int Port, string UName, string UPassword, string DBName, String Rest = "Allow Zero Datetime=True")
        {
            return $"SERVER={Server};PORT={Port};USER={UName};PASSWORD={UPassword};DATABASE={DBName};" + Rest;
        }


        /// <summary>
        /// 将一行数据转换为基本的字段实体
        /// </summary>
        /// <param name="SqlRow">查出的一行数据</param>
        /// <param name="tableParent">父表</param>
        /// <returns></returns>
        public static ColumnBasicModel SqlRowToColumnBasicModel(dynamic SqlRow, TableBasicModel tableParent = null)
        {
            return new ColumnBasicModel()
            {
                //ColumnType = CantDBNull(SqlRow.COLUMN_TYPE, ""),
                //DataType = CantDBNull(SqlRow.DATA_TYPE, ""),
                DataType = new ColumnDataTypeBasicModel()
                {
                    FullName= CantDBNull(SqlRow.COLUMN_TYPE, ""),
                    Name= CantDBNull(SqlRow.DATA_TYPE, "")
                },
                Default = CantDBNull(SqlRow.COLUMN_DEFAULT, ""),
                Extra = CantDBNull(SqlRow.EXTRA, ""),
                IsKey = SqlRow.COLUMN_KEY == "PRI",
                IsNullBle = CantDBNull(SqlRow.IS_NULLABLE, "") == "YES",
                Name = CantDBNull(SqlRow.COLUMN_NAME, ""),
                Source = CantDBNull(SqlRow, ""),
                Table = CantDBNull(tableParent, ""),
                Tip = CantDBNull(SqlRow.COLUMN_COMMENT, "")
            };
        }

        public static dynamic CantDBNull(dynamic Source, dynamic Value)
        {
            return Source is DBNull ? Value : Source;
        }
        public static TableBasicModel QueryTableInfo(MySqlConnection connection,String TName,DataBaseBasicModel DB)
        {
            var stb = QueryAllTables(connection, $" WHERE TABLE_NAME='{TName}' AND TABLE_SCHEMA='{DB.Name}'").First();
            //表
            TableBasicModel tbm = new TableBasicModel()
            {
                Name = stb.TABLE_NAME,
                Source = stb,
                DataBase = DB,
                Columns = new List<ColumnBasicModel>(),
            };

            //列
            var tbm_cols = QueryAllColumns(connection, $" WHERE TABLE_SCHEMA='{DB.Name}' AND TABLE_NAME='{TName}'");
            foreach (var col in tbm_cols)
            {
                tbm.Columns.Add(MySqlHelp.SqlRowToColumnBasicModel(col, tbm));
            }
            //查出所有数据行
            tbm.Rows = MySqlHelp.ExecuteQuery(connection, $"SELECT * FROM {DB.Name}.{tbm.Name}");

            return tbm;
        }
    }
}
