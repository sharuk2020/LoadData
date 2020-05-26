namespace LoadData
{
    using LoadData.Infrastructure;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public class CodeFirstDynamicModelEF : DbContext
    {
        // Your context has been configured to use a 'CodeFirstDynamicModelEF' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'LoadData.CodeFirstDynamicModelEF' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'CodeFirstDynamicModelEF' 
        // connection string in the application configuration file.
        //public CodeFirstDynamicModelEF()
        //    : base("name=CodeFirstDynamicModelEF")
        //{
         
        //}
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // call Processor
            //var configuration = new DbConfiguration
            //{
            //    TargetDatabase = new DbConnectionInfo(
            //  "Server=MyServer;Database=MyDatabase;Trusted_Connection=True;",
            //  "System.Data.SqlClient")
            //};

    //        var configuration = new DbMigrationsConfiguration();
    //       configuration.ContextType = typeof(CodeFirstDynamicModelEF);
    //        configuration.TargetDatabase = new System.Data.Entity.Infrastructure.DbConnectionInfo("Server=LAPTOP-RLGIHTV4\\SSHAIK7;Database=DynamicModelDb;Trusted_Connection=True;",
    //"System.Data.SqlClient");

    //        var migrator = new DbMigrator(configuration);
    //        migrator.Update();

            var entityMethod = typeof(DbModelBuilder).GetMethod("Entity");

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where( ass => ass.IsDynamic))
            {
                var entityTypes = assembly
                  .GetTypes()
                  .Where(t =>
                    t.GetCustomAttributes(typeof(ModelClassAttribute), inherit: true)
                    .Any());

                foreach (var type in entityTypes)
                {
                    entityMethod.MakeGenericMethod(type)
                      .Invoke(modelBuilder, new object[] { });
                }
            }
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    public class MyEntity
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }
}