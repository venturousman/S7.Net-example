using HmiExample.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace HmiExample.Data
{
    // DropCreateDatabaseIfModelChanges
    // DropCreateDatabaseAlways
    public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            // seed Machines
            IList<Machine> defaultMachines = new List<Machine>();

            defaultMachines.Add(new Machine() { Id = Guid.NewGuid(), Name = "Machine 1", Code = "M001", TagIndex = 0, Counts = 4590, CreatedOn = DateTime.UtcNow });
            defaultMachines.Add(new Machine() { Id = Guid.NewGuid(), Name = "Machine 2", Code = "M002", TagIndex = 1, Counts = 4320, CreatedOn = DateTime.UtcNow });

            context.Machines.AddRange(defaultMachines);

            // seed Product
            IList<Product> defaultProducts = new List<Product>();

            defaultProducts.Add(new Product() { Id = Guid.NewGuid(), Name = "Product 1", Code = "P001", CreatedOn = DateTime.UtcNow });
            defaultProducts.Add(new Product() { Id = Guid.NewGuid(), Name = "Product 2", Code = "P002", CreatedOn = DateTime.UtcNow });

            context.Products.AddRange(defaultProducts);

            base.Seed(context);
        }
    }
}
