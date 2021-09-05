using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConverter.Base.Model
{
    public class ProcedureBasicModel
    {
        public String Name { get; set; }
        public String Content { get; set; }
        public DataBaseBasicModel DataBase { get; set; }
        public dynamic Source { get; set; }
    }
}
