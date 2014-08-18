using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MackPortfolio.DAL
{
    public class MackPortfolioContext : DbContext
    {

        private void ModSaveChanges()
        {
            var entryList = this.ChangeTracker.Entries()
                .Where(e => e.Entity is LogIt &&
                    (e.State == EntityState.Modified || e.State == EntityState.Added))
                .Select(e => e.Entity as LogIt);

            foreach (var entry in entryList)
            {
                if (this.Entry(entry).State == EntityState.Added)
                {
                    entry.Created = DateTime.Now;
                    entry.IsActive = true;
                }
                entry.Modified = DateTime.Now;
            }
        }

        public override int SaveChanges()
        {
            ModSaveChanges();
            return base.SaveChanges();
        }
        public override async Task<int> SaveChangesAsync()
        {
            ModSaveChanges();
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
        }
    }
}
