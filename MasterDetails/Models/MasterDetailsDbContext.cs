using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MasterDetails.Models;

public class MasterDetailsDbContext: DbContext
{
    public MasterDetailsDbContext(DbContextOptions<MasterDetailsDbContext> options) : base(options)
        {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<PurchaseProduct> PurchaseProducts { get; set; }
    public DbSet<Unit> Units { get; set; }
}
