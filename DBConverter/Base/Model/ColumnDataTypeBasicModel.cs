using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConverter.Base.Model
{
    public class ColumnDataTypeBasicModel
    {
        public String FullName { get; set; }
        public String Name { get; set; }
        /// <summary>
        /// 这里指的是 例如 Varchar(100) 中的 (100)
        /// </summary>
        public String Size { get => FullName.Replace(Name, ""); }
        public ColumnDataTypeBasicModel MySqlToSqlServer()
        {
            ColumnDataTypeBasicModel d = new ColumnDataTypeBasicModel();
            var select = MySQL_SQLServer_Relation.Where(i => i.Key == Name.ToUpper());
            if (select.Count() < 1)
            {
                throw new Exception($"未写入此关系，无法转换 MYSQL-("+FullName+")");
            }
            else
            {
                d.Name = select.First().Key;
                d.FullName = select.First().Key + this.Size;
            }
            return d;
        }
        public ColumnDataTypeBasicModel SqlServerToMySql()
        {
            ColumnDataTypeBasicModel d = new ColumnDataTypeBasicModel();
            var select = MySQL_SQLServer_Relation.Where(i => i.Value == Name.ToUpper());
            if (select.Count() < 1)
            {
                throw new Exception($"未写入此关系，无法转换 MYSQL-(" + FullName + ")");
            }
            else
            {
                d.Name = select.First().Value;
                d.FullName = select.First().Value + this.Size;
            }
            return d;

        }

        /**
         //JS
         clear();
            var cSharp="";
            for(var f=0;f<items.length;f++)
            {
                var splitVal=items[f].innerText.split('	');
                var mysqlVal=splitVal[1];
                var sqlServerVal=splitVal[2];
                cSharp+='new ("'+mysqlVal.toUpperCase()+'","'+sqlServerVal.toUpperCase()+'"),\n';
            }
            console.log(cSharp);
         **/
        /// <summary>
        /// 描述类型转换的关系
        /// 左边Mysql 右边SqlServer 
        /// 来自 https://www.cnblogs.com/hopesun/p/9095198.html
        /// </summary>
        public KeyValuePair<String, String>[] MySQL_SQLServer_Relation = new KeyValuePair<string, string>[]
        {
            new ("BIGINT","BIGINT"),
            new ("BINARY","BINARY"),
            new ("BIT","TINYINT"),
            new ("CHAR","CHAR"),
            new ("DATE","DATE"),
            new ("DATETIME","DATETIME"),
            new ("DATETIME2","DATETIME"),
            new ("DATETIMEOFFSET","DATETIME"),
            new ("DECIMAL","DECIMAL"),
            new ("FLOAT","FLOAT"),
            new ("INT","INT"),
            new ("MONEY","FLOAT"),
            new ("NCHAR","CHAR"),
            new ("NTEXT","TEXT"),
            new ("NUMERIC","DECIMAL"),
            new ("NVARCHAR","VARCHAR"),
            new ("REAL","FLOAT"),
            new ("SMALLDATETIME","DATETIME"),
            new ("SMALLINT","SMALLINT"),
            new ("SMALLMONEY","FLOAT"),
            new ("TEXT","TEXT"),
            new ("TIME","TIME"),
            new ("TIMESTAMP","TIMESTAMP"),
            new ("TINYINT","TINYINT"),
            new ("UNIQUEIDENTIFIER","VARCHAR(40)"),
            new ("VARBINARY","VARBINARY"),
            new ("VARCHAR","VARCHAR"),
            new ("XML","TEXT"),
        };
    }
}
