namespace MackPortfolio.DAL.AppOppModels
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AppsOppDb : DbContext
    {
        public AppsOppDb()
            : base("name=AppsOppContext")
        {
        }

        public virtual DbSet<AppLookup> AppLookups { get; set; }
        public virtual DbSet<AppPlatLookup> AppPlatLookups { get; set; }
        public virtual DbSet<AppStatus> AppStatus { get; set; }
        public virtual DbSet<CustomerIndustry> CustomerIndustries { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerSegment> CustomerSegments { get; set; }
        public virtual DbSet<CustomerSubSegment> CustomerSubSegments { get; set; }
        public virtual DbSet<DeviceType> DeviceTypes { get; set; }
        public virtual DbSet<FilterSet> FilterSets { get; set; }
        public virtual DbSet<GeoHierarchy> GeoHierarchies { get; set; }
        public virtual DbSet<IndustryScenario> IndustryScenarios { get; set; }
        public virtual DbSet<MSAssociate> MSAssociates { get; set; }
        public virtual DbSet<NativeOrWeb> NativeOrWebs { get; set; }
        public virtual DbSet<Opportunity> Opportunities { get; set; }
        public virtual DbSet<OpportunityDevice> OpportunityDevices { get; set; }
        public virtual DbSet<OpportunityTeam> OpportunityTeams { get; set; }
        public virtual DbSet<OSType> OSTypes { get; set; }
        public virtual DbSet<OSVersion> OSVersions { get; set; }
        public virtual DbSet<Platform> Platforms { get; set; }
        public virtual DbSet<PublishersLookup> PublishersLookups { get; set; }
        public virtual DbSet<SalesDate> SalesDates { get; set; }
        public virtual DbSet<SalesStage> SalesStages { get; set; }
        public virtual DbSet<SalesStageFilter> SalesStageFilters { get; set; }
        public virtual DbSet<ApplicationOpportunity> ApplicationOpportunities { get; set; }
        public virtual DbSet<UserApplication> UserApplications { get; set; }
        public virtual DbSet<UserPublisher> UserPublishers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppPlatLookup>()
                .Property(e => e.AppDataSource)
                .IsUnicode(false);

            modelBuilder.Entity<AppStatus>()
                .HasMany(e => e.AppPlatLookups)
                .WithOptional(e => e.AppStatus)
                .HasForeignKey(e => e.AppStatusID);

            modelBuilder.Entity<AppStatus>()
                .HasMany(e => e.UserApplications)
                .WithOptional(e => e.AppStatus)
                .HasForeignKey(e => e.AppStatusID);

            modelBuilder.Entity<CustomerIndustry>()
                .Property(e => e.VerticalCategoryName)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerIndustry>()
                .Property(e => e.VerticalName)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerIndustry>()
                .Property(e => e.IndustryName)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Opportunities)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CustomerSegment>()
                .HasMany(e => e.CustomerSubSegments)
                .WithRequired(e => e.CustomerSegment)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CustomerSubSegment>()
                .HasMany(e => e.Customers)
                .WithRequired(e => e.CustomerSubSegment)
                .HasForeignKey(e => e.CustomerSubSegmentID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DeviceType>()
                .HasMany(e => e.OpportunityDevices)
                .WithRequired(e => e.DeviceType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GeoHierarchy>()
                .Property(e => e.AreaName)
                .IsUnicode(false);

            modelBuilder.Entity<GeoHierarchy>()
                .Property(e => e.SubsidiaryName)
                .IsUnicode(false);

            modelBuilder.Entity<GeoHierarchy>()
                .Property(e => e.SubsidiaryDistrictName)
                .IsUnicode(false);

            modelBuilder.Entity<IndustryScenario>()
                .HasMany(e => e.ApplicationOpportunities)
                .WithOptional(e => e.IndustryScenario)
                .HasForeignKey(e => e.IndustryScenarioID);

            modelBuilder.Entity<MSAssociate>()
                .HasMany(e => e.OpportunityTeams)
                .WithRequired(e => e.MSAssociate)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NativeOrWeb>()
                .HasMany(e => e.UserApplications)
                .WithRequired(e => e.NativeOrWeb1)
                .HasForeignKey(e => e.NativeOrWeb)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Opportunity>()
                .HasMany(e => e.ApplicationOpportunities)
                .WithRequired(e => e.Opportunity)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Opportunity>()
                .HasMany(e => e.OpportunityDevices)
                .WithRequired(e => e.Opportunity)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Opportunity>()
                .HasMany(e => e.OpportunityTeams)
                .WithRequired(e => e.Opportunity)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PublishersLookup>()
                .HasMany(e => e.UserPublishers)
                .WithOptional(e => e.PublishersLookup)
                .HasForeignKey(e => e.PublisherLookupID);

            modelBuilder.Entity<SalesDate>()
                .Property(e => e.CalendarDateName)
                .IsUnicode(false);

            modelBuilder.Entity<SalesDate>()
                .Property(e => e.FiscalMonthName)
                .IsUnicode(false);

            modelBuilder.Entity<SalesDate>()
                .Property(e => e.FiscalQuarterName)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SalesDate>()
                .Property(e => e.FiscalYearName)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SalesDate>()
                .HasMany(e => e.Opportunities)
                .WithOptional(e => e.SalesDate)
                .HasForeignKey(e => e.OpportunityDueDate);

            modelBuilder.Entity<SalesDate>()
                .HasMany(e => e.Opportunities1)
                .WithOptional(e => e.SalesDate1)
                .HasForeignKey(e => e.ActualOpportunityCloseDate);

            modelBuilder.Entity<SalesDate>()
                .HasMany(e => e.ApplicationOpportunities)
                .WithRequired(e => e.SalesDate)
                .HasForeignKey(e => e.DateAdded)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SalesDate>()
                .HasMany(e => e.ApplicationOpportunities1)
                .WithRequired(e => e.SalesDate1)
                .HasForeignKey(e => e.DateUpdated)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SalesDate>()
                .HasMany(e => e.UserApplications)
                .WithOptional(e => e.SalesDate)
                .HasForeignKey(e => e.ModifiedDate);

            modelBuilder.Entity<SalesDate>()
                .HasMany(e => e.UserApplications1)
                .WithOptional(e => e.SalesDate1)
                .HasForeignKey(e => e.CreatedDate);

            modelBuilder.Entity<SalesStage>()
                .HasOptional(e => e.SalesStageFilter)
                .WithRequired(e => e.SalesStage);

            modelBuilder.Entity<UserApplication>()
                .HasMany(e => e.ApplicationOpportunities)
                .WithRequired(e => e.UserApplication)
                .WillCascadeOnDelete(false);
        }
    }
}
