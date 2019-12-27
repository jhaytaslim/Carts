using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Carts.DbModel
{
    public partial class AirTableContextX : DbContext
    {
        public AirTableContextX()
        {
        }

        public AirTableContextX(DbContextOptions<AirTableContext> options)
            : base(options)
        {
        }

        public virtual DbSet<InvoiceTb> InvoiceTb { get; set; }
        public virtual DbSet<OrderItemTb> OrderItemTb { get; set; }
        public virtual DbSet<OrderTb> OrderTb { get; set; }
        //public virtual DbSet<PaymentMethodTb> PaymentMethodTb { get; set; }
        public virtual DbSet<PaymentTb> PaymentTb { get; set; }
        public virtual DbSet<ProductTb> ProductTb { get; set; }
        public virtual DbSet<RoleTb> RoleTb { get; set; }
        public virtual DbSet<UsersTb> UsersTb { get; set; }
        //public virtual DbSet<WishlistTb> WishlistTb { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<InvoiceTb>(entity =>
            {
                entity.HasKey(e => e.InvoiceId);

                entity.ToTable("InvoiceTB");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.TotalOrderCost).HasColumnType("decimal(18, 4)");

                entity.HasOne(d => d.CreatedBy)
                    .WithMany(p => p.InvoiceTb)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InvoiceTB_UsersTB");

                //entity.HasOne(d => d.Order)
                //    .WithMany(p => p.InvoiceTb)
                //    .HasForeignKey(d => d.OrderId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_InvoiceTB_OrderTB");

                //entity.HasOne(d => d.Payment)
                //    .WithMany(p => p.InvoiceTb)
                //    .HasForeignKey(d => d.PaymentId)
                //    .HasConstraintName("FK_InvoiceTB_PaymentTB");

                //entity.HasOne(d => d.Product)
                //    .WithMany(p => p.InvoiceTb)
                //    .HasForeignKey(d => d.ProductId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_InvoiceTB_ProductTB");
            });

            modelBuilder.Entity<OrderItemTb>(entity =>
            {
                entity.HasKey(e => e.OrderItemId);

                entity.ToTable("OrderItemTB");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.PriceSold).HasColumnType("decimal(18, 4)");

                //entity.HasOne(d => d.CreatedByNavigation)
                //    .WithMany(p => p.OrderItemTb)
                //    .HasForeignKey(d => d.CreatedBy)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_OrderItemTB_UsersTB");

                //entity.HasOne(d => d.Order)
                //    .WithMany(p => p.OrderItemTb)
                //    .HasForeignKey(d => d.OrderId)
                //    .HasConstraintName("FK_OrderItemTB_OrderTB");

                //entity.HasOne(d => d.Product)
                //    .WithMany(p => p.OrderItemTb)
                //    .HasForeignKey(d => d.ProductId)
                //    .HasConstraintName("FK_OrderItemTB_ProductTB");
            });

            modelBuilder.Entity<OrderTb>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.ToTable("OrderTB");

                entity.Property(e => e.DateCreated)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.DeliveryAddress)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DeliveryCost).HasColumnType("decimal(18, 4)");

                //entity.HasOne(d => d.CreatedbyNavigation)
                //    .WithMany(p => p.OrderTbCreatedbyNavigation)
                //    .HasForeignKey(d => d.Createdby)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_OrderTB_UsersTB");

                //entity.HasOne(d => d.Customer)
                //    .WithMany(p => p.OrderTbCustomer)
                //    .HasForeignKey(d => d.CustomerId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_OrderTB_UsersTB1");

                //entity.HasOne(d => d.PaymentMethod)
                //    .WithMany(p => p.OrderTb)
                //    .HasForeignKey(d => d.PaymentMethodId)
                //    .HasConstraintName("FK_OrderTB_PaymentMethodTB");
            });

            modelBuilder.Entity<PaymentMethodTb>(entity =>
            {
                entity.ToTable("PaymentMethodTB");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PaymentTb>(entity =>
            {
                entity.HasKey(e => e.PaymentId);

                entity.ToTable("PaymentTB");

                entity.Property(e => e.PaymentId).ValueGeneratedNever();

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.PaymentTb)
                    .HasForeignKey(d => d.PaymentMethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PaymentTB_PaymentMethodTB");
            });

            modelBuilder.Entity<ProductTb>(entity =>
            {
                entity.ToTable("ProductTB");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FeatureImage).IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RoleTb>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.ToTable("RoleTB");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UsersTb>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("UsersTB");

                entity.Property(e => e.Address1)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Address2)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.Profileimage).IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                //entity.HasOne(d => d.Role)
                //    .WithMany(p => p.UsersTb)
                //    .HasForeignKey(d => d.RoleId)
                //    .HasConstraintName("FK_UsersTB_RoleTB");
            });

            modelBuilder.Entity<WishlistTb>(entity =>
            {
                entity.ToTable("WishlistTB");

                entity.Property(e => e.Id).ValueGeneratedNever();

                //entity.HasOne(d => d.CreatedbyNavigation)
                //    .WithMany(p => p.WishlistTb)
                //    .HasForeignKey(d => d.Createdby)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_WishlistTB_UsersTB");

                //entity.HasOne(d => d.Product)
                //    .WithMany(p => p.WishlistTb)
                //    .HasForeignKey(d => d.ProductId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_WishlistTB_ProductTB");
            });
        }
    }
}
