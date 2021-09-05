using DBConverter.Base;
using DBConverter.Base.AbsDBEntrys;
using DBConverter.Base.Help;
using DBConverter.Base.Interface;
using DBConverter.Base.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConverter.DBEntrys
{
    public class MySqlEntry : AbsDBEntry
    {

        public MySqlEntry(string Server, int Port, string UName, string UPassword, string DBName) : base(Server, Port, UName, UPassword, DBName)
        {
            this.Connection = MySqlHelp.Open(Server, Port, UName, UPassword, DBName);
            this.Connection.Open();
        }

        public  MySqlConnection Connection { get => this.connection as MySqlConnection; set => this.connection = value; }


        public override List<DataBaseBasicModel> QueryAllDataBase()
        {
            var dbs= MySqlHelp.QueryAllDataBase(this.Connection);
            var list = new List<DataBaseBasicModel>();

            foreach(var db in dbs)
            {
                list.Add(new DataBaseBasicModel()
                {
                    Name= db.SCHEMA_NAME,
                });
            }
            return list;
        }


        public override List<FunctionBasicModel> QueryAllFunctions(string DBName = null)
        {
            List<FunctionBasicModel> l = new List<FunctionBasicModel>();
            foreach(var f in MySqlHelp.QueryAllFunctions(Connection,$" WHERE DB='{DBName ?? Name}'"))
            {
                l.Add(new FunctionBasicModel()
                {
                    Name=f.Name,
                    Source=f
                });
            }
            return l;
        }

        public override List<ProcedureBasicModel> QueryAllProcedures(string DBName = null)
        {
            List<ProcedureBasicModel> l = new List<ProcedureBasicModel>();
            foreach (var p in MySqlHelp.QueryAllProcedures(this.Connection, $" WHERE DB='{DBName ?? Name}'")) 
            {
                l.Add(new ProcedureBasicModel()
                {
                    Name=p.Name,
                    Source=p,
                });
            }
            return l;
        }

        //public override List<TableBasicModel> QueryAllTables()
        //{
        //    var tbs= MySqlHelp.QueryAllTables(this.Connection, "WHERE TABLE_SCHEMA='" + this.Name + "'");
        //    var list = new List<TableBasicModel>();

        //    foreach (var db in tbs)
        //    {
        //        list.Add(new TableBasicModel()
        //        {
        //            Name = db.TABLE_NAME,
        //            Source=tbs
        //        });
        //    }
        //    return list;
        //}

        public override List<TableBasicModel> QueryAllTables(string DBName=null)
        {
            var tbs = MySqlHelp.QueryAllTables(this.Connection, " WHERE TABLE_SCHEMA='" + (DBName ?? this.Name) + "'");
            var list = new List<TableBasicModel>();

            foreach (var db in tbs)
            {
                list.Add(new TableBasicModel()
                {
                    Name = db.TABLE_NAME,
                    Source = tbs
                });
            }
            return list;
        }

        public override List<dynamic> QueryAllTablesInfo()
        {
            return MySqlHelp.QueryAllTables(this.Connection, " WHERE TABLE_SCHEMA='" + this.Name + "'");
        }

        public override FunctionBasicModel QueryFunctionInfo(string DBName, string FuncName)
        {
            return MySqlHelp.QueryFunctionCreateSql(this.Connection, DBName, FuncName);
        }

        public override ProcedureBasicModel QueryProcedureInfo(string DBName, string ProcName)
        {
            return MySqlHelp.QueryProcedureCreateSql(this.Connection, DBName, ProcName);
        }

        public override TableBasicModel QueryTableInfo(string TBName, DataBaseBasicModel DB)
        {
            return MySqlHelp.QueryTableInfo(this.Connection, TBName, DB);
        }


        /// <summary>
        /// 将表转换为基本的数据库模型
        /// </summary>
        /// <returns></returns>
        public override DataBaseBasicModel ToModel()
        {
            DataBaseBasicModel db = new DataBaseBasicModel()
            {
                Name=this.Name,
                Tables=new List<TableBasicModel>()
            };
            List<TableBasicModel> tbs = db.Tables;

            var source_tables = QueryAllTablesInfo();

            foreach (var stb in source_tables)
            {
                tbs.Add(QueryTableInfo(stb.TABLE_NAME, db));
            }
            return db;
        }
    }
}
