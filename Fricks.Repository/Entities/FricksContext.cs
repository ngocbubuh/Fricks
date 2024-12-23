﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Fricks.Repository.Entities;

public partial class FricksContext : DbContext
{
    public FricksContext()
    {
    }

    public FricksContext(DbContextOptions<FricksContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<FavoriteProduct> FavoriteProducts { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductPrice> ProductPrices { get; set; }

    public virtual DbSet<ProductUnit> ProductUnits { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    public virtual DbSet<Otp> Otps { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Withdraw> Withdraws { get; set; }
    public virtual DbSet<Banner> Banners { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Brand__3214EC0769856975");

            entity.ToTable("Brand");

            entity.Property(e => e.Name).HasMaxLength(150);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC0778AEA4C8");

            entity.ToTable("Category");

            entity.Property(e => e.Name).HasMaxLength(250);

            entity.Property(e => e.Code).HasMaxLength(5);
        });

        modelBuilder.Entity<FavoriteProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Favorite__3214EC074EA02728");

            entity.ToTable("FavoriteProduct");

            entity.HasOne(d => d.Product).WithMany(p => p.FavoriteProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__FavoriteP__Produ__6B24EA82");

            entity.HasOne(d => d.User).WithMany(p => p.FavoriteProducts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__FavoriteP__UserI__6C190EBB");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__3214EC07A6C249C8");

            entity.ToTable("Feedback");

            entity.Property(e => e.Content).HasMaxLength(500);

            entity.HasOne(d => d.Product).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Feedback__Produc__6477ECF3");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC07E1941292");

            entity.ToTable("Order");

            entity.Property(e => e.Code).HasMaxLength(60);
            entity.Property(e => e.CustomerAddress).HasMaxLength(250);
            entity.Property(e => e.CustomerEmail).HasMaxLength(250);
            entity.Property(e => e.CustomerName).HasMaxLength(250);
            entity.Property(e => e.CustomerPhone).HasMaxLength(10);
            entity.Property(e => e.PaymentMethod).HasMaxLength(10);
            entity.Property(e => e.PaymentStatus).HasMaxLength(10);
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.Property(e => e.BankCode).HasMaxLength(50);
            entity.Property(e => e.BankTranNo).HasMaxLength(100);
            entity.Property(e => e.TransactionNo).HasMaxLength(100);

            entity.Property(e => e.DeliveryDate).HasColumnType("datetime2");

            entity.HasOne(d => d.Store).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK__Order__StoreId__6D0D32F4");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Order__UserId__6383C8BA");

            entity.HasOne(d => d.Voucher).WithMany(p => p.Orders)
                .HasForeignKey(d => d.VoucherId)
                .HasConstraintName("FK__Order__VoucherId__6EF57B66");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3214EC07BFF156B5");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.ProductUnit).HasMaxLength(20);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__Order__628FA481");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__OrderDeta__Produ__619B8048");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Post__3214EC0790D5D4E0");

            entity.ToTable("Post");

            entity.Property(e => e.Title).HasMaxLength(250);

            entity.HasOne(d => d.Product).WithMany(p => p.Posts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Post__ProductId__656C112C");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3214EC071F30D584");

            entity.ToTable("Product");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Sku)
                .HasMaxLength(10)
                .HasColumnName("SKU");
            entity.Property(e => e.UnsignName).HasMaxLength(250);

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK__Product__BrandId__66603565");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Product__Categor__60A75C0F");

            entity.HasOne(d => d.Store).WithMany(p => p.Products)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK__Product__StoreId__693CA210");
        });

        modelBuilder.Entity<ProductPrice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductP__3214EC07152E9AA2");

            entity.ToTable("ProductPrice");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductPrices)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__ProductPr__Produ__6754599E");

            entity.HasOne(d => d.Unit).WithMany(p => p.ProductPrices)
                .HasForeignKey(d => d.UnitId)
                .HasConstraintName("FK__ProductPr__UnitI__68487DD7");
        });

        modelBuilder.Entity<ProductUnit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductU__3214EC070A61266E");

            entity.ToTable("ProductUnit");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.Property(e => e.Code).HasMaxLength(4);
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Store__3214EC07144D2B0C");

            entity.ToTable("Store");

            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.TaxCode).HasMaxLength(13);
            entity.Property(e => e.PhoneNumber).HasMaxLength(10);
            entity.Property(e => e.BankCode).HasMaxLength(20);
            entity.Property(e => e.AccountNumber).HasMaxLength(20);
            entity.Property(e => e.AccountName).HasMaxLength(100);

            entity.HasOne(d => d.Manager).WithMany(p => p.Stores)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__Store__ManagerId__6A30C649");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07CA998980");

            entity.ToTable("User");

            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.FullName).HasMaxLength(250);
            entity.Property(e => e.UnsignFullName).HasMaxLength(250);
            entity.Property(e => e.GoogleId).HasMaxLength(500);
            entity.Property(e => e.PasswordHash).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasMaxLength(10);
            entity.Property(e => e.Dob).HasColumnName("DOB");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Voucher__3214EC079C55FA7B");

            entity.ToTable("Voucher");

            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Store).WithMany(p => p.Vouchers)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK__Voucher__StoreId__6E01572D");
        });

        modelBuilder.Entity<Otp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Otp");

            entity.ToTable("Otp");

            entity.Property(e => e.Email).HasMaxLength(250);

            entity.Property(e => e.OtpCode).HasMaxLength(6);

            entity.Property(e => e.ExpiryTime).HasColumnType("datetime2");

            entity.Property(e => e.CreateDate)
               .HasDefaultValueSql("(getdate())")
               .HasColumnType("datetime2");

            entity.Property(e => e.UpdateDate).HasColumnType("datetime2");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Wallet");

            entity.ToTable("Wallet");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime2");

            entity.Property(e => e.UpdateDate).HasColumnType("datetime2");

            entity.HasOne(d => d.Store).WithOne(p => p.Wallet)
                .HasForeignKey<Wallet>(d => d.StoreId)
                .HasConstraintName("FK__Wallet__Store__114A936A");

            entity.Property(e => e.Balance).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transaction");

            entity.ToTable("Transaction");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime2");

            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TransactionDate).HasColumnType("datetime2");
            entity.Property(e => e.TransactionType).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime2");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.WalletId)
                .HasConstraintName("FK__Transaction__Wallet");
        });

        modelBuilder.Entity<Withdraw>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Withdraw");

            entity.ToTable("Withdraw");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime2");

            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.ConfirmDate).HasColumnType("datetime2");
            entity.Property(e => e.TransferDate).HasColumnType("datetime2");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime2");
            entity.Property(e => e.Requester).HasMaxLength(250);
            entity.Property(e => e.ConfirmBy).HasMaxLength(250);
            entity.Property(e => e.Note).HasMaxLength(500);

            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Withdraws)
                .HasForeignKey(d => d.WalletId)
                .HasConstraintName("FK__Withdraw__Wallet");
        });

        modelBuilder.Entity<Banner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Banner");

            entity.ToTable("Banner");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime2");

            entity.Property(e => e.Name).HasMaxLength(150);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
