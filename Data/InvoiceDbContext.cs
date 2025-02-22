
using InvoiceApiRest.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApiRest.Data
{
    public class InvoiceDbContext : DbContext
    {
        public InvoiceDbContext(DbContextOptions<InvoiceDbContext> options) : base(options) { }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InvoiceDetail>().ToTable("invoiceDetail");
            modelBuilder.Entity<Invoice>().ToTable("invoice");

            //modelBuilder.Entity<Invoice>()
            //    .Property(i => i.Id)
            //    .ValueGeneratedOnAdd(); // Auto Incremental

            //modelBuilder.Entity<DetailInvoice>()
            //    .Property(d => d.Id)
            //    .ValueGeneratedOnAdd(); // Auto Incremental

            modelBuilder.Entity<Invoice>()
                .Property(i => i.Status)
                .HasDefaultValue("active"); // ✅ Configura el valor por defecto

            modelBuilder.Entity<Invoice>()
                .HasMany(i => i.Details)
                .WithOne(d => d.Invoice)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);  // ✅ Configura la relación y eliminación en cascada
        }
    }
}
