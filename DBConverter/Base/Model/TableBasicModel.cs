using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConverter.Base.Model
{
    public class TableBasicModel
    {
        public DataBaseBasicModel DataBase { get; set; }
        public String Name { get; set; }
        public List<ColumnBasicModel> Columns { get; set; }
        public List<dynamic> Rows { get; set; }
        public dynamic Source { get; set; }
    }
}
