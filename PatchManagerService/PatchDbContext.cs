using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BLToolkit.Data.Linq;
using MySql.Data.MySqlClient;
namespace PatchManagerService
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    class PatchDbContext : DbContext
    {
        private static string connectionName = "NameFromXml=patchInfoDb";

        public PatchDbContext() : this(connectionName)
        {
        }
       
        public PatchDbContext(string connectionName) : base(connectionName)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            Database.Initialize(true);
        }

        public DbSet<PatchInfo> PatchInfo { get; set; }

        public static void AddPatchInfo2Db(PatchInfo patchinfo)
        {
            using (var db = new PatchDbContext(connectionName))
            {
                db.Set<PatchInfo>().AddOrUpdate(patchinfo);
                db.SaveChanges();
            }
        }

        public static PatchInfo GetPathInfo(int patchNumber)
        {
            using (var db = new PatchDbContext(connectionName))
            {
                var foudedItem = db.PatchInfo.Find("P-" + patchNumber);
                if (foudedItem != null) Console.WriteLine(foudedItem.Description);
                return foudedItem;
            }
        }

    }
}
