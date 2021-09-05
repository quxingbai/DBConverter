using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConverter.Base.Model
{
    public class FunctionBasicModel
    {
        public String Name { get; set; }
        public String Content { get; set; }
        public dynamic Source { get; set; }
        public DataBaseBasicModel DataBase { get; set; }
    }
}
