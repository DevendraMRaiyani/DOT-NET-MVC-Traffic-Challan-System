namespace PoliceAdmin.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class Universal : DbContext
    {
        // Your context has been configured to use a 'Universal' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'PoliceAdmin.Models.Universal' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Universal' 
        // connection string in the application configuration file.
        public Universal()
            : base("name=Universal")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public virtual DbSet<DL> DLs { get; set; }
        public virtual DbSet<RC> Rcs { get; set; }
        public virtual DbSet<PUC> PUCs { get; set; }
       
        public virtual DbSet<RULES> RULESs { get; set; }
        public virtual DbSet<PublicUser> pus { get; set; }
        public virtual DbSet<Challan> Challans { get; set; }


    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}