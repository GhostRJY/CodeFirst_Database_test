using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CodeFirst.Models; //мои модели

namespace CodeFirst.Contexts
{
    public class MarketPlaceDBContext: DbContext
    {
        public DbSet<User>? Users { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Review>? Reviews { get; set; }
        public DbSet<UserToReview>? UserToReviews { get; set; }
        public DbSet<ProductToOrder>? ProductToOrders { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost; Database=MarketPlace; Trusted_Connection=True; TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasKey(c => c.Id);


            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId); //связь user и order

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId); //связь product и category

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId); //связь category и product


            //связь product и order
            modelBuilder.Entity<ProductToOrder>()
                .HasKey(po => new { po.Product_id, po.Order_id }); //составной ключ

            
            modelBuilder.Entity<ProductToOrder>()
                .HasOne(po=>po.Order)
                .WithMany(o => o.ProductToOrders)
                .HasForeignKey(po => po.Order_id);

            modelBuilder.Entity<ProductToOrder>()
                .HasOne(po => po.Product)
                .WithMany(p => p.ProductToOrders)
                .HasForeignKey(po => po.Product_id);

            //связь user и review
            modelBuilder.Entity<UserToReview>().HasKey(ur => new { ur.User_id, ur.Review_id });

            modelBuilder.Entity<UserToReview>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserToReviews)
                .HasForeignKey(ur => ur.User_id);

            modelBuilder.Entity<UserToReview>()
                .HasOne(ur => ur.Review)
                .WithMany(r => r.UserToReviews)
                .HasForeignKey(ur => ur.Review_id);

            modelBuilder.Entity<CategoriesOfProduct>(entity =>
            {
                entity.HasNoKey(); // убираем первичный ключ, так как это не таблица, а результат процедуры
                //entity.ToFunction("GetCategoriesOfProduct"); // указываем, что это результат процедуры
            });


        }

        

        public void ResetDatabase()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
            Console.WriteLine("База данных очищена и пересоздана.");
        }
    }
    
    
}
