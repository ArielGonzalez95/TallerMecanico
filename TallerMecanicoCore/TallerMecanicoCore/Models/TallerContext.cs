using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TallerMecanicoCore.DTO;
using TallerMecanicoCore.Models;

namespace TallerMecanicoCore.Models;

public partial class TallerContext : DbContext
{
    public TallerContext()
    {
    }

    public TallerContext(DbContextOptions<TallerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Automovil> Automovils { get; set; }

    public virtual DbSet<Desperfecto> Desperfectos { get; set; }

    public virtual DbSet<Moto> Motos { get; set; }

    public virtual DbSet<Presupuesto> Presupuestos { get; set; }

    public virtual DbSet<Repuesto> Repuestos { get; set; }

    public virtual DbSet<Vehiculo> Vehiculos { get; set; }

    public DbSet<DatosPresupuesto> DatosPresupuesto { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-PF6KMVK; DataBase=Taller;TrustServerCertificate=True;Integrated Security=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Automovil>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Automovi__3213E83F3F0D7F61");

            entity.ToTable("Automovil");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.IdVehiculo).HasColumnName("idVehiculo");
            entity.Property(e => e.Tipo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.Automovils)
                .HasForeignKey(d => d.IdVehiculo)
                .HasConstraintName("FK__Automovil__idVeh__66603565");
        });

        modelBuilder.Entity<Desperfecto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Desperfe__3214EC07117A8EE6");

            entity.ToTable("Desperfecto");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdPresupuesto).HasColumnName("idPresupuesto");
            entity.Property(e => e.Manodeobra)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("manodeobra");

            entity.HasOne(d => d.IdPresupuestoNavigation).WithMany(p => p.Desperfectos)
                .HasForeignKey(d => d.IdPresupuesto)
                .HasConstraintName("FK__Desperfec__idPre__6383C8BA");

            entity.HasMany(d => d.IdRepuestos).WithMany(p => p.IdDesperfectos)
                .UsingEntity<Dictionary<string, object>>(
                    "DesperfectoRepuesto",
                    r => r.HasOne<Repuesto>().WithMany()
                        .HasForeignKey("IdRepuesto")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Desperfec__idRep__74AE54BC"),
                    l => l.HasOne<Desperfecto>().WithMany()
                        .HasForeignKey("IdDesperfecto")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Desperfec__idDes__73BA3083"),
                    j =>
                    {
                        j.HasKey("IdDesperfecto", "IdRepuesto").HasName("PK__Desperfe__DA4E4F14ED4A58B3");
                        j.ToTable("DesperfectoRepuesto");
                        j.IndexerProperty<int>("IdDesperfecto").HasColumnName("idDesperfecto");
                        j.IndexerProperty<int>("IdRepuesto").HasColumnName("idRepuesto");
                    });
        });

        modelBuilder.Entity<Moto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Moto__3213E83F6D427D8D");

            entity.ToTable("Moto");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.IdVehiculo).HasColumnName("idVehiculo");

            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.Motos)
                .HasForeignKey(d => d.IdVehiculo)
                .HasConstraintName("FK__Moto__idVehiculo__693CA210");
        });

        modelBuilder.Entity<Presupuesto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Presupue__3214EC077BB20D69");

            entity.ToTable("Presupuesto");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.Presupuestos)
                .HasForeignKey(d => d.IdVehiculo)
                .HasConstraintName("FK__Presupues__IdVeh__60A75C0F");
        });

        modelBuilder.Entity<Repuesto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Repuesto__3214EC0778CA5BEA");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(18, 6)");
        });

        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Vehiculo__3213E83FBCE89F2D");

            entity.ToTable("Vehiculo");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Marca)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("marca");
            entity.Property(e => e.Modelo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("modelo");
            entity.Property(e => e.Patente)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("patente");
        });

        OnModelCreatingPartial(modelBuilder);

        modelBuilder.Entity<DatosPresupuesto>().HasNoKey().ToView(null);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
