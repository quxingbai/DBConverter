using DBConverter.Base.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConverter.Base.Interface
{
    public interface IDBEntry: IDisposable
    {
        public String Server { get; set; }
        public String UserName { get; set; }
        public int Port { get; set; }
        public string UserPassword { get; set; }

        public event Action<Object,DataBaseBasicModel> Used;
        /// <summary>
        /// 获取所有表
        /// </summary>
        public List<dynamic> QueryAllTablesInfo();
        public List<TableBasicModel> QueryAllTables(String DBName);
        public TableBasicModel QueryTableInfo(String TBName, DataBaseBasicModel DB);
        public FunctionBasicModel QueryFunctionInfo(String DBName, String FuncName);
        public ProcedureBasicModel QueryProcedureInfo(String DBName, String ProcName);

        /// <summary>
        /// 使用一个数据库
        /// </summary>
        /// <param name="DBName"></param>
        public void Use(String DBName);
        /// <summary>
        /// 最常见的数据库字段对应的C#字段
        /// 数据库转换时会用到
        /// </summary>
        //Dictionary<string, TableDataType> TableDataTypeRelation { get; set; }
        String Name { get; set; }
    }
}
