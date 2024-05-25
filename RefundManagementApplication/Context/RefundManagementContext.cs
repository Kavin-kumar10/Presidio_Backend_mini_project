using Microsoft.EntityFrameworkCore;
using RefundManagementApplication.Models;

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
                new Member() {  Id= 101, email="kavinkumar.prof@gmail.com",Name = "Kavin", Role="Admin" },
                new Member() {  Id = 102,email="pravinkumar.prof@gmail.com",Name = "Pravin", Role="Admin" },
                new Member() { Id = 103, email = "raju@gmail.com",Name = "Raju",Role="Collector"}
            );

            modelBuilder.Entity<Member>()
                .HasMany(m => m.orders)
                .WithOne(o => o.OrderedBy)
                .HasForeignKey(m => m.OrderId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

        }
    }
}
