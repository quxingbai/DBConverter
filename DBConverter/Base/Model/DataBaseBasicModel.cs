using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConverter.Base.Model
{
    public class DataBaseBasicModel
    {
        public String Name { get; set; }
        public List<TableBasicModel> Tables = new List<TableBasicModel>();
        public List<ProcedureBasicModel> Procedures = new List<ProcedureBasicModel>();
        public List<FunctionBasicModel> Functions = new List<FunctionBasicModel>();
    }
}
