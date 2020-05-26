using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadData.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ModelClassAttribute : Attribute
    {

        //Add Logic if needed.
        public ModelClassAttribute()
        {
        }
    }




    [ModelClass]
    public class TestClass
    {
        [Key]
        public int id { get; set; }
    }
}
