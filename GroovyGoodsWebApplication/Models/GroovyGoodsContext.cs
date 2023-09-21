using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GroovyGoodsWebApplication.Models;

public partial class GroovyGoodsContext : DbContext
{
    public GroovyGoodsContext()
    {
    }

    public GroovyGoodsContext(DbContextOptions<GroovyGoodsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrator> Administrators { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<SupplierProduct> SupplierProducts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\ProjectModels;Database=GroovyGoods;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrator>(entity =>
        {
            entity.HasKey(e => e.Aid).HasName("PK__Administ__C6900628BEFFCA9F");

            entity.ToTable("Administrator");

            entity.Property(e => e.Aid).HasColumnName("AId");
            entity.Property(e => e.Hash).HasMaxLength(50);
            entity.Property(e => e.Salt).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Pid).HasName("PK__tmp_ms_x__C5775540BE842118");

            entity.ToTable("Product");

            entity.Property(e => e.Pid).HasColumnName("PId");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.ListPrice).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Sid).HasName("PK__Supplier__CA19595068E4601B");

            entity.ToTable("Supplier");

            entity.Property(e => e.Sid).HasColumnName("SId");
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Company).HasMaxLength(50);
            entity.Property(e => e.ContactName).HasMaxLength(50);
            entity.Property(e => e.Country).HasMaxLength(60);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<SupplierProduct>(entity =>
        {
            entity.HasKey(e => e.Spid).HasName("PK__Supplier__F430612902F6978B");

            entity.ToTable("Supplier_Product");

            entity.Property(e => e.Spid).HasColumnName("SPId");
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Pid).HasColumnName("PId");
            entity.Property(e => e.Sid).HasColumnName("SId");

            entity.HasOne(d => d.PidNavigation).WithMany(p => p.SupplierProducts)
                .HasForeignKey(d => d.Pid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRODUCT_SUPPLIERPRODUCT");

            entity.HasOne(d => d.SidNavigation).WithMany(p => p.SupplierProducts)
                .HasForeignKey(d => d.Sid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SUPPLIER_SUPPLIERPRODUCT");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
