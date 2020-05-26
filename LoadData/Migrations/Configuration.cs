namespace LoadData.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LoadData.CodeFirstDynamicModelEF>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "LoadData.CodeFirstDynamicModelEF";
        }

        protected override void Seed(LoadData.CodeFirstDynamicModelEF context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
