using System.Data.Entity;

namespace Web.Models.Entity
{


    public partial class CRM_DbContext : DbContext
    {
        public CRM_DbContext()
            : base("name=DefaultConnection")
        {
        }
        public CRM_DbContext(string connectionName)
           : base("name="+connectionName)
        {
        }

    
        public virtual DbSet<CRMLog> CRMLogs { get; set; }
        public virtual DbSet<CustomerRelation> CustomerRelations { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomersCategoriesBridge> CustomersCategoriesBridges { get; set; }
        public virtual DbSet<CustomersOptionValue> CustomersOptionValues { get; set; }
        public virtual DbSet<CustomersOrganizationsBridge> CustomersOrganizationsBridges { get; set; }
        public virtual DbSet<FactorCost> FactorCosts { get; set; }
        public virtual DbSet<FactorCostSet> FactorCostSets { get; set; }
        public virtual DbSet<FactorItemCost> FactorItemCosts { get; set; }
        public virtual DbSet<FactorItem> FactorItems { get; set; }
        public virtual DbSet<Factor> Factors { get; set; }
        public virtual DbSet<ProductPriceLog> ProductPriceLogs { get; set; }
        public virtual DbSet<ProductPrice> ProductPrices { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<SiteValue> SiteValues { get; set; }

        public virtual DbSet<WorkFlowJob> WorkFlowJobs { get; set; }
        public virtual DbSet<WorkFlow> WorkFlows { get; set; }
        public virtual DbSet<WorkFlowToken> WorkFlowTokens { get; set; }
        public virtual ProductOptionValue ProductOptionValues { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserClaim> UserClaims { get; set; }
        public virtual DbSet<UserLogin> UserLogins { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Policy> Policies { get; set; }
       

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Customer>()
                .HasMany(e => e.CustomerRelations)
                .WithOptional(e => e.Customer)
                .HasForeignKey(e => e.customer_id);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.CustomersCategoriesBridges)
                .WithRequired(e => e.Customer)
                .HasForeignKey(e => e.customer_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.CustomersOptionValues)
                .WithOptional(e => e.Customer)
                .HasForeignKey(e => e.customer_id);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.CustomersOrganizationsBridges)
                .WithRequired(e => e.Customer)
                .HasForeignKey(e => e.customer_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CustomerRelation>()
                .HasMany(e => e.CustomersOptionValues)
                .WithOptional(e => e.CustomerRelation)
                .HasForeignKey(e => e.customerRelation_id);

            modelBuilder.Entity<SiteValue>()
                .HasMany(e => e.CustomersCategoriesBridges)
                .WithRequired(e => e.SiteValue)
                .HasForeignKey(e => e.category_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SiteValue>()
                .HasMany(e => e.CustomersOptionValues)
                .WithRequired(e => e.SiteValue)
                .HasForeignKey(e => e.option_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SiteValue>()
                .HasMany(e => e.CustomersOptionValues1)
                .WithOptional(e => e.SiteValue1)
                .HasForeignKey(e => e.value_id);

            modelBuilder.Entity<SiteValue>()
                .HasMany(e => e.CustomersOrganizationsBridges)
                .WithRequired(e => e.SiteValue)
                .HasForeignKey(e => e.organization_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SiteValue>()
                .HasMany(e => e.Customers)
                .WithRequired(e => e.SiteValue)
                .HasForeignKey(e => e.province_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FactorCostSet>()
                .HasMany(e => e.FactorCosts)
                .WithRequired(e => e.FactorCostSet)
                .HasForeignKey(e => e.factorCostSet_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FactorCostSet>()
                .HasMany(e => e.FactorItemCosts)
                .WithOptional(e => e.FactorCostSet)
                .HasForeignKey(e => e.FactorCostSet_id);

            modelBuilder.Entity<FactorItem>()
                .HasMany(e => e.FactorItemCosts)
                .WithOptional(e => e.FactorItem)
                .HasForeignKey(e => e.factorItem_id);

            modelBuilder.Entity<Factor>()
                .HasMany(e => e.FactorCosts)
                .WithRequired(e => e.Factor)
                .HasForeignKey(e => e.factor_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Factor>()
                .HasMany(e => e.FactorItems)
                .WithRequired(e => e.Factor)
                .HasForeignKey(e => e.factor_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductPrice>()
                .HasMany(e => e.FactorItems)
                .WithRequired(e => e.ProductPrice)
                .HasForeignKey(e => e.price_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.FactorItems)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.product_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ProductPriceLogs)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.product_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ProductPrices)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.product_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SiteValue>()
                .HasMany(e => e.ProductPriceLogs)
                .WithRequired(e => e.SiteValue)
                .HasForeignKey(e => e.vahed_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SiteValue>()
                .HasMany(e => e.ProductPrices)
                .WithOptional(e => e.SiteValue)
                .HasForeignKey(e => e.vahed_id);

            modelBuilder.Entity<SiteValue>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.SiteValue)
                .HasForeignKey(e => e.category_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ProductOptionValues)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.product_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SiteValue>()
                .HasMany(e => e.ProductOptionValues1)
                .WithRequired(e => e.SiteValue)
                .HasForeignKey(e => e.product_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SiteValue>()
                .HasMany(e => e.ProductOptionValues1)
                .WithRequired(e => e.SiteValue1)
                .HasForeignKey(e => e.option_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SiteValue>()
                .HasMany(e => e.SiteValues1)
                .WithOptional(e => e.SiteValue1)
                .HasForeignKey(e => e.parentId);

            //workFlow
            modelBuilder.Entity<WorkFlow>()
                .HasMany(e => e.WorkFlowJobs)
                .WithRequired(e => e.WorkFlow)
                .HasForeignKey(e => e.workFlow_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<WorkFlowToken>()
                .HasMany(e => e.WorkFlowJobs)
                .WithOptional(e => e.WorkFlowToken)
                .HasForeignKey(e => e.token_id);

            modelBuilder.Entity<SiteValue>()
                .HasMany(e => e.WorkFlows)
                .WithRequired(e => e.SiteValue)
                .HasForeignKey(e => e.sectionType_id)
                .WillCascadeOnDelete(false);

            //customer factor
            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Factors)
                .WithOptional(e => e.Customer)
                .HasForeignKey(e => e.customer_id);


            modelBuilder.Entity<Role>()
              .HasMany(e => e.Users)
              .WithMany(e => e.Roles)
              .Map(m => m.ToTable("UserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));
            modelBuilder.Entity<Permission>()
                .HasMany(c => c.Policies).
                WithMany(c => c.Permissions).Map(c => c.ToTable("PolicyPermition").MapLeftKey("PermissionId").MapLeftKey("PolicyId"));

            modelBuilder.Entity<User>().HasMany(c => c.Policies).WithRequired(c => c.User);

            //OrganizationalUnit_id
            modelBuilder.Entity<SiteValue>()
                .HasMany(e => e.Users)
                .WithOptional(e => e.SiteValue)
                .HasForeignKey(e => e.OrganizationalUnit_id);


            //factor-siteValue
            modelBuilder.Entity<SiteValue>()
                .HasMany(e => e.Factors)
                .WithOptional(e => e.SiteValue)
                .HasForeignKey(e => e.status);

            modelBuilder.Entity<SiteValue>()
                .HasMany(e => e.Factors1)
                .WithOptional(e => e.SiteValue1)
                .HasForeignKey(e => e.presentation);

            modelBuilder.Entity<SiteValue>()
                .HasMany(e => e.Factors2)
                .WithOptional(e => e.SiteValue2)
                .HasForeignKey(e => e.paymentType);

            modelBuilder.Entity<SiteValue>()
                .HasMany(e => e.FactorOptionValues)
                .WithOptional(e => e.SiteValue)
                .HasForeignKey(e => e.factor_id);

            modelBuilder.Entity<SiteValue>()
                .HasMany(e => e.FactorOptionValues1)
                .WithOptional(e => e.SiteValue1)
                .HasForeignKey(e => e.value_id);

            modelBuilder.Entity<Factor>()
                .HasMany(e => e.FactorOptionValues)
                .WithOptional(e => e.Factor)
                .HasForeignKey(e => e.factor_id);
        }
    }
}
