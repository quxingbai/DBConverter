using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConverter.Base.Model
{
    public class ColumnBasicModel
    {
        public TableBasicModel Table { get; set; }
        public String Name { get; set; }
        /// <summary>
        /// 提示
        /// </summary>
        public String Tip { get; set; }
        /// <summary>
        /// 可能会带有取值范围
        /// </summary>
        //public String ColumnType { get; set; }
        //public String DataType { get; set; }
        public ColumnDataTypeBasicModel DataType { get; set; }

        /// <summary>
        /// 可为空，方便判断值属于什么
        /// </summary>
        //public TableDataType DataTypeEnum { get; set; }
        public String Default { get; set; }

        /// <summary>
        /// 额外的
        /// </summary>
        public String Extra { get; set; }
        public bool IsKey { get; set; }
        /// <summary>
        /// 可为空
        /// </summary>
        public bool IsNullBle { get; set; }
        /// <summary>
        /// 源
        /// </summary>
        public dynamic Source { get; set; }
    }
}
