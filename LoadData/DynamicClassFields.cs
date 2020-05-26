using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadData
{
    public interface IDynamicClassFields
    {

        string FieldName { get; set; }

        Type FieldType { get; set; }

    }

    public class DynamicClassFields : IDynamicClassFields
    {
        public string FieldName { get; set; }

        public Type FieldType { get; set; }
    }

}
