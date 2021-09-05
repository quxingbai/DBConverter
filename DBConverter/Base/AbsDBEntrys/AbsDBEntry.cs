using DBConverter.Base.Help;
using DBConverter.Base.Interface;
using DBConverter.Base.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConverter.Base.AbsDBEntrys
{
    public abstract class AbsDBEntry : IDBEntry
    {
        public DbConnection connection;

        public event Action<object, DataBaseBasicModel> Used;

        public AbsDBEntry(String Server, int Port, String UName, String UPassword, String DBName)
        {
            this.Name = DBName;
            this.Server = Server;
            this.Port = Port;
            this.UserName = UName;
            this.UserPassword = UPassword;
        }
        //public Dictionary<string, TableDataType> TableDataTypeRelation { get; set; } = new Dictionary<string, TableDataType>();
        public string Name { get; set; }
        public string Server { get; set; }
        public string UserName { get; set; }
        public int Port { get; set; }
        public string UserPassword { get; set; }

        //public ConnectionType Connection { get => connection; set { connection = value; } }
        public void Dispose()
        {
            (connection as DbConnection).Dispose();
        }

        public abstract List<TableBasicModel> QueryAllTables(string DBName=null);
        public abstract List<DataBaseBasicModel> QueryAllDataBase();
        public abstract List<ProcedureBasicModel> QueryAllProcedures(String DBName=null);
        public abstract List<FunctionBasicModel> QueryAllFunctions(String DBName=null);
        public abstract DataBaseBasicModel ToModel();

        public abstract List<dynamic> QueryAllTablesInfo();

        public void Use(string DBName)
        {
            this.Name = DBName;
            connection.Dispose();
            this.connection = MySqlHelp.Open(Server,Port,UserName,UserPassword,DBName);
            connection.Open();
            Used?.Invoke(this, ToModel()); 
        }

        public abstract TableBasicModel QueryTableInfo(string TBName, DataBaseBasicModel DB);
        public abstract FunctionBasicModel QueryFunctionInfo(string DBName, string FuncName);
        public abstract ProcedureBasicModel QueryProcedureInfo(string DBName, string ProcName);
    }
}
