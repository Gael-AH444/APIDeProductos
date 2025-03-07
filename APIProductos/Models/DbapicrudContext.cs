using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace APIProductos.Models;

public partial class DbapicrudContext : DbContext
{
    public DbapicrudContext()
    {
    }

    public DbapicrudContext(DbContextOptions<DbapicrudContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__Categori__A3C02A103198DCE6");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__Producto__09889210BFE470E1");

            entity.ToTable("Producto");

            entity.Property(e => e.CodigoBarra)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Marca)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.oCategoria).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("FK_IDCATEGORIA");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
