using LoadData.Migrations;
using LoadData.TypeBuilderNamespace;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadData
{
  public   class Program
    {
     public   static void Main(string[] args)
        {
            // read the metadata from the table 
            // foreach row in the metadata table
            // Create a class  using dynamic class.


            List<IDynamicClassFields> dynamicClassFields = new List<IDynamicClassFields>()
            {
                new DynamicClassFields() { FieldName="Id",FieldType = typeof(Int32)},
                 new DynamicClassFields() { FieldName="Name",FieldType = typeof(string)}
            };
          

            var DemoClass =   MyTypeBuilder.CreateNewObject(dynamicClassFields, "Demo");

            object value = "sharuk";

            MyTypeBuilder.SetValue(DemoClass, "Name", value);

            CodeFirstDynamicModelEF codeFirstDynamicModelEF = new CodeFirstDynamicModelEF();
            var configuration = new Configuration();
            var migrator = new DbMigrator(configuration);
            migrator.Update();
            codeFirstDynamicModelEF.MyEntities.Add(new MyEntity());
            codeFirstDynamicModelEF.Set(DemoClass.GetType()).Add(DemoClass);


            codeFirstDynamicModelEF.SaveChanges();

        }

        
    }
}
