using Microsoft.EntityFrameworkCore;
using RefundManagementApplication.Models;
using RefundManagementApplication.Models.Enums;
using System.Security.Cryptography;
using System.Text;

namespace RefundManagementApplication.Context
{
    public class RefundManagementContext:DbContext
    {
        public RefundManagementContext(DbContextOptions options) : base(options){ }

        public DbSet<Refund> Refunds { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>().HasData(
                new Member() {  Id= 101, email="kavinkumar.prof@gmail.com",Name = "Kavin", Role=MemberRole.User },
                new Member() {  Id = 102,email="pravinkumar.prof@gmail.com",Name = "Pravin", Role=MemberRole.Admin },
                new Member() {  Id = 103, email = "raju@gmail.com",Name = "Raju",Role=MemberRole.Collector}
            );

            modelBuilder.Entity<User>().HasData(
                new User(){ MemberId = 101, Password = Encoding.UTF8.GetBytes("yourPassword"), PasswordHashKey = Encoding.UTF8.GetBytes("yourPassword"),Status = "Disabled"},
                new User() { MemberId = 102, Password = Encoding.UTF8.GetBytes("yourPassword"), PasswordHashKey = Encoding.UTF8.GetBytes("yourPassword"), Status = "Active" },
                new User() { MemberId = 103, Password = Encoding.UTF8.GetBytes("yourPassword"), PasswordHashKey = Encoding.UTF8.GetBytes("yourPassword"), Status = "Disabled" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product() { ProductId = 101, Title = "Soccor Football Nivia", Description = "Sportsman Products", Act_price = 1200, Curr_price = 1000, Count = 10, Returnable = 7, ReturnableForPrime = 14 },
                new Product() { ProductId = 102, Title = "Noice TWS", Description = "ANC Tws airdopes", Act_price = 1000, Curr_price = 899, Count = 50, Returnable = 0, ReturnableForPrime = 7 }
            );

            modelBuilder.Entity<Order>().HasData(
                new Order() { OrderId = 1, MemberID = 101, CreatedDate = DateTime.Now, OrderStatus = OrderStatuses.Refund_Accepted, ProductId = 101, TotalPrice = 1000 }
            );

            modelBuilder.Entity<Refund>().HasData(
                new Refund(){ RefundId = 1,OrderId = 1, RefundAmount = 1000, Reason = "Damaged", RefundStatus = RefundStatuses.PENDING, CreatedBy = 101,CreatedDate = DateTime.Now}
            );

            //modelBuilder.Entity<Member>()
            //    .HasMany(m => m.orders)
            //    .WithOne(o => o.OrderedBy)
            //    .HasForeignKey(m => m.OrderId)
            //    .OnDelete(DeleteBehavior.Restrict)
            //    .IsRequired();

            modelBuilder.Entity<Member>()
              .HasMany(m => m.orders) 
              .WithOne(o => o.OrderedBy) 
              .HasForeignKey(o => o.MemberID) 
              .OnDelete(DeleteBehavior.Restrict) 
              .IsRequired();

            modelBuilder.Entity<Order>()
            .Navigation(order => order.Refund)
            .AutoInclude();

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Refund)
                .WithOne(r => r.Order)
                .HasForeignKey<Refund>(r => r.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .Navigation<Refund>(order=>order.Refund)
                .AutoInclude();

            //modelBuilder.Entity<Refund>()
            //    .HasOne(r => r.Order)
            //    .WithOne(o => o.Refund)
            //    .HasForeignKey<Refund>(r => r.OrderId)
            //    .OnDelete(DeleteBehavior.Restrict); 

            //modelBuilder.Entity<Order>()
            //    .HasOne(o => o.Refund)
            //    .WithOne(r => r.Order)
            //    .HasForeignKey<Order>(o => o.RefundId);

        }
    }
}