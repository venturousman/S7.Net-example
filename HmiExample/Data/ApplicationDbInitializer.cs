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

            // seed Employee
            IList<Employee> defaultEmployees = new List<Employee>();

            defaultEmployees.Add(new Employee() { Id = Guid.NewGuid(), Code = "E001", Email = "employee1@gmail.com", DisplayName = "Employee 1", FirstName = "A", MidleName = "Van", LastName = "Tran" });
            defaultEmployees.Add(new Employee() { Id = Guid.NewGuid(), Code = "E002", Email = "employee2@gmail.com", DisplayName = "Employee 2", FirstName = "B", MidleName = "Van", LastName = "Nguyen" });

            context.Employees.AddRange(defaultEmployees);

            // seed Plan
            IList<Plan> defaultPlans = new List<Plan>();

            defaultPlans.Add(new Plan() { Id = Guid.NewGuid(), MachineId = defaultMachines[0].Id, ProductId = defaultProducts[1].Id, EmployeeId = defaultEmployees[1].Id, ExpectedQuantity = 32 });
            defaultPlans.Add(new Plan() { Id = Guid.NewGuid(), MachineId = defaultMachines[0].Id, ProductId = defaultProducts[1].Id, EmployeeId = defaultEmployees[0].Id, ExpectedQuantity = 13 });
            defaultPlans.Add(new Plan() { Id = Guid.NewGuid(), MachineId = defaultMachines[0].Id, ProductId = defaultProducts[0].Id, EmployeeId = defaultEmployees[0].Id, ExpectedQuantity = 17 });
            defaultPlans.Add(new Plan() { Id = Guid.NewGuid(), MachineId = defaultMachines[1].Id, ProductId = defaultProducts[0].Id, EmployeeId = defaultEmployees[1].Id, ExpectedQuantity = 41 });
            defaultPlans.Add(new Plan() { Id = Guid.NewGuid(), MachineId = defaultMachines[1].Id, ProductId = defaultProducts[1].Id, EmployeeId = defaultEmployees[0].Id, ExpectedQuantity = 22 });

            context.Plans.AddRange(defaultPlans);

            base.Seed(context);
        }
    }
}
