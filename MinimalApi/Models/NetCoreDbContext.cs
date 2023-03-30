using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MinimalApi.Models;

public partial class NetCoreDbContext : DbContext
{
    public NetCoreDbContext()
    {
    }

    public NetCoreDbContext(DbContextOptions<NetCoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblPerson> TblPeople { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=198.49.75.133\\SQLSCB010,1433;Database=NetCoreDB;Uid=sa; Pwd=Asus8426*; TrustServerCertificate=True; encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblPerson>(entity =>
        {
            entity.ToTable("tbl_person");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.Birth)
                .HasColumnType("smalldatetime")
                .HasColumnName("birth");
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
