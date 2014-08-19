using MackPortfolio.DAL.ActivityModels;
using MackPortfolio.DAL.MediaModels;
using MackPortfolio.DAL.WebModels;
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
        public MackPortfolioContext()
            : base("PortfolioContext"){}

        public DbSet<WebContent> Webpages { get; set; }
        public DbSet<WebContentRevision> WebRevisions { get; set; }
        public DbSet<UserMessage> Messages { get; set; }
        public DbSet<Activity> Events { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<Thumbnail> Thumbnails { get; set; }
        public DbSet<Standard> Standards { get; set; }
        public DbSet<Original> Originals { get; set; }

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
