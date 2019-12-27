using Microsoft.EntityFrameworkCore;

namespace Carts.Data.POCO
{
    public partial class AirTableContext : DbContext
    {
        public AirTableContext()
        {
        }

        public AirTableContext(DbContextOptions<AirTableContext> options) : base(options)
        {
        }

        public virtual DbSet<InvoiceTb> InvoiceTb { get; set; }
        public virtual DbSet<OrderItemTb> OrderItemTb { get; set; }
        public virtual DbSet<OrderTb> OrderTb { get; set; }
        public virtual DbSet<ProductTb> ProductTb { get; set; }
        public virtual DbSet<RoleTb> RoleTb { get; set; }
        public virtual DbSet<UsersTb> UsersTb { get; set; }
        public virtual DbSet<MailLogTb> MailLogTb { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            //modelBuilder.Entity<InvoiceTb>(entity =>
            //{
            //    entity.HasKey(e => e.InvoiceId);

            //    entity.ToTable("InvoiceTB");

            //    entity.Property(e => e.DateCreated).HasColumnType("datetime");

            //    entity.Property(e => e.TotalOrderCost).HasColumnType("decimal(18, 4)");

            //    entity.HasOne(d => d.CreatedBy)
            //        .WithMany(p => p.InvoiceTb)
            //        .HasForeignKey(d => d.CreatedBy)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_InvoiceTB_UsersTB");

            //});

            //modelBuilder.Entity<OrderItemTb>(entity =>
            //{
            //    entity.HasKey(e => e.OrderItemId);

            //    entity.ToTable("OrderItemTB");

            //    entity.Property(e => e.DateCreated).HasColumnType("datetime");

            //    entity.Property(e => e.PriceSold).HasColumnType("decimal(18, 4)");

            //});

            //modelBuilder.Entity<OrderTb>(entity =>
            //{
            //    entity.HasKey(e => e.OrderId);

            //    entity.ToTable("OrderTB");

            //    entity.Property(e => e.DateCreated)
            //        .IsRequired()
            //        .HasMaxLength(10);

            //    entity.Property(e => e.DeliveryAddress)
            //        .HasMaxLength(250)
            //        .IsUnicode(false);

            //    entity.Property(e => e.DeliveryCost).HasColumnType("decimal(18, 4)");

            //});

            
            //modelBuilder.Entity<ProductTb>(entity =>
            //{
            //    entity.ToTable("ProductTB");

            //    entity.Property(e => e.DateCreated).HasColumnType("datetime");

            //    entity.Property(e => e.Description)
            //        .HasMaxLength(100)
            //        .IsUnicode(false);

            //    entity.Property(e => e.FeatureImage).IsUnicode(false);

            //    entity.Property(e => e.Title)
            //        .HasMaxLength(100)
            //        .IsUnicode(false);
            //});

            //modelBuilder.Entity<RoleTb>(entity =>
            //{
            //    entity.HasKey(e => e.RoleId);

            //    entity.ToTable("RoleTB");

            //    entity.Property(e => e.RoleName)
            //        .HasMaxLength(50)
            //        .IsUnicode(false);
            //});

            //modelBuilder.Entity<UsersTb>(entity =>
            //{
            //    entity.HasKey(e => e.UserId);

            //    entity.ToTable("UsersTB");

            //    entity.Property(e => e.Address1)
            //        .HasMaxLength(500)
            //        .IsUnicode(false);

            //    entity.Property(e => e.Address2)
            //        .HasMaxLength(500)
            //        .IsUnicode(false);

            //    entity.Property(e => e.DateCreated).HasColumnType("datetime");

            //    entity.Property(e => e.Profileimage).IsUnicode(false);

            //    entity.Property(e => e.Username)
            //        .HasMaxLength(50)
            //        .IsUnicode(false);
                
            //});

        }
    }
}
