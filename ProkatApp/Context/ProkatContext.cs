using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProkatApp.Models;

namespace ProkatApp.Context;

public partial class ProkatContext : DbContext
{
    public ProkatContext()
    {
    }

    public ProkatContext(DbContextOptions<ProkatContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<EntranceStatus> EntranceStatuses { get; set; }

    public virtual DbSet<LoginHistory> LoginHistories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<UserDatum> UserData { get; set; }

    public virtual DbSet<UserService> UserServices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=26.192.39.184;Port=5434;Database=prokat;Username=postgres;Password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("Client_pkey");

            entity.ToTable("Client");

            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.AddresIndex).HasColumnName("addresIndex");
            entity.Property(e => e.AddresTittle).HasColumnName("addresTittle");
            entity.Property(e => e.DateOfBirth).HasColumnName("dateOfBirth");
            entity.Property(e => e.PasportNumber).HasColumnName("pasportNumber");
            entity.Property(e => e.PasportSeriya).HasColumnName("pasportSeriya");
            entity.Property(e => e.UserDataId).HasColumnName("userData_id");

            entity.HasOne(d => d.UserData).WithMany(p => p.Clients)
                .HasForeignKey(d => d.UserDataId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Client_userData_id_fkey");
        });

        modelBuilder.Entity<EntranceStatus>(entity =>
        {
            entity.HasKey(e => e.EntranceStatusId).HasName("EntranceStatus_pkey");

            entity.ToTable("EntranceStatus");

            entity.Property(e => e.EntranceStatusId).HasColumnName("entranceStatus_id");
            entity.Property(e => e.StatusTittle).HasColumnName("statusTittle");
        });

        modelBuilder.Entity<LoginHistory>(entity =>
        {
            entity.HasKey(e => e.LoginHistoryId).HasName("LoginHistory_pkey");

            entity.ToTable("LoginHistory");

            entity.Property(e => e.LoginHistoryId).HasColumnName("loginHistory_id");
            entity.Property(e => e.EntranceStatusId).HasColumnName("entranceStatus_id");
            entity.Property(e => e.LoginTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("loginTime");
            entity.Property(e => e.UserDataId).HasColumnName("userData_id");

            entity.HasOne(d => d.EntranceStatus).WithMany(p => p.LoginHistories)
                .HasForeignKey(d => d.EntranceStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("LoginHistory_entranceStatus_id_fkey");

            entity.HasOne(d => d.UserData).WithMany(p => p.LoginHistories)
                .HasForeignKey(d => d.UserDataId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("LoginHistory_userData_id_fkey");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("Orders_pkey");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.DateClose).HasColumnName("dateClose");
            entity.Property(e => e.DateCreate).HasColumnName("dateCreate");
            entity.Property(e => e.OrderCode)
                .HasMaxLength(255)
                .HasColumnName("orderCode");
            entity.Property(e => e.OrderStatusId).HasColumnName("orderStatus_id");
            entity.Property(e => e.RentTime).HasColumnName("rentTime");
            entity.Property(e => e.TimeCreate).HasColumnName("timeCreate");

            entity.HasOne(d => d.Client).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Orders_client_id_fkey");

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Orders_orderStatus_id_fkey");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.OrderStatusId).HasName("OrderStatus_pkey");

            entity.ToTable("OrderStatus");

            entity.Property(e => e.OrderStatusId).HasColumnName("orderStatus_id");
            entity.Property(e => e.StatusTittle).HasColumnName("statusTittle");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("Role_pkey");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName).HasColumnName("roleName");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("Services_pkey");

            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.CostPerHour).HasColumnName("costPerHour");
            entity.Property(e => e.ServiceCode).HasColumnName("serviceCode");
            entity.Property(e => e.ServiceTittle).HasColumnName("serviceTittle");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("Staff_pkey");

            entity.Property(e => e.StaffId).HasColumnName("staff_id");
            entity.Property(e => e.ImagePath).HasColumnName("imagePath");
            entity.Property(e => e.UserDataId).HasColumnName("userData_id");

            entity.HasOne(d => d.UserData).WithMany(p => p.Staff)
                .HasForeignKey(d => d.UserDataId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Staff_userData_id_fkey");
        });

        modelBuilder.Entity<UserDatum>(entity =>
        {
            entity.HasKey(e => e.UserDataId).HasName("UserData_pkey");

            entity.Property(e => e.UserDataId).HasColumnName("userData_id");
            entity.Property(e => e.Fio).HasColumnName("FIO");
            entity.Property(e => e.Login)
                .HasMaxLength(255)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.UserData)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserData_role_id_fkey");
        });

        modelBuilder.Entity<UserService>(entity =>
        {
            entity.HasKey(e => e.UsId).HasName("UserServices_pkey");

            entity.Property(e => e.UsId)
                .HasDefaultValueSql("nextval('\"UserServices_uo_id_seq\"'::regclass)")
                .HasColumnName("us_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");

            entity.HasOne(d => d.Order).WithMany(p => p.UserServices)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserServices_order_id_fkey");

            entity.HasOne(d => d.Service).WithMany(p => p.UserServices)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserServices_service_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
