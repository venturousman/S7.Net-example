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
            var defaultCreatedOn = new DateTime(2018, 10, 18, 12, 0, 0);

            // seed Machines
            IList<Machine> defaultMachines = new List<Machine>();

            defaultMachines.Add(new Machine() { Id = Guid.NewGuid(), Name = "Machine 1", Code = "M001", TagIndex = 0, Counts = 4590, CreatedOn = defaultCreatedOn });
            defaultMachines.Add(new Machine() { Id = Guid.NewGuid(), Name = "Machine 2", Code = "M002", TagIndex = 1, Counts = 4320, CreatedOn = defaultCreatedOn });

            context.Machines.AddRange(defaultMachines);

            // seed Product
            IList<Product> defaultProducts = new List<Product>();

            defaultProducts.Add(new Product() { Id = Guid.NewGuid(), Name = "Product 1", Code = "P001", CreatedOn = defaultCreatedOn });
            defaultProducts.Add(new Product() { Id = Guid.NewGuid(), Name = "Product 2", Code = "P002", CreatedOn = defaultCreatedOn });
            defaultProducts.Add(new Product() { Id = Guid.NewGuid(), Name = "Product 3", Code = "P003", CreatedOn = defaultCreatedOn });
            defaultProducts.Add(new Product() { Id = Guid.NewGuid(), Name = "Product 4", Code = "P004", CreatedOn = defaultCreatedOn });
            defaultProducts.Add(new Product() { Id = Guid.NewGuid(), Name = "Product 5", Code = "P005", CreatedOn = defaultCreatedOn });

            context.Products.AddRange(defaultProducts);

            // seed Employee
            IList<Employee> defaultEmployees = new List<Employee>();

            defaultEmployees.Add(new Employee()
            {
                Id = Guid.NewGuid(),
                Code = "E001",
                Email = "employee1@gmail.com",
                DisplayName = "Employee 1",
                FirstName = "A",
                MidleName = "Van",
                LastName = "Tran",
                CreatedOn = defaultCreatedOn
            });
            defaultEmployees.Add(new Employee()
            {
                Id = Guid.NewGuid(),
                Code = "E002",
                Email = "employee2@gmail.com",
                DisplayName = "Employee 2",
                FirstName = "B",
                MidleName = "Van",
                LastName = "Nguyen",
                CreatedOn = defaultCreatedOn
            });
            defaultEmployees.Add(new Employee()
            {
                Id = Guid.NewGuid(),
                Code = "E003",
                Email = "employee3@gmail.com",
                DisplayName = "Employee 3",
                FirstName = "C",
                MidleName = "Van",
                LastName = "Le",
                CreatedOn = defaultCreatedOn
            });

            context.Employees.AddRange(defaultEmployees);

            // seed Plan
            IList<Plan> defaultPlans = new List<Plan>();

            defaultPlans.Add(new Plan()
            {
                Id = Guid.NewGuid(),
                MachineId = defaultMachines[0].Id,
                ProductId = defaultProducts[1].Id,
                EmployeeId = defaultEmployees[1].Id,
                ExpectedQuantity = 32,
                ActualQuantity = 29,
                CreatedOn = new DateTime(2018, 10, 18, 12, 0, 0),
                StartTime = new DateTime(2018, 10, 18, 8, 0, 0),
                EndTime = new DateTime(2018, 10, 18, 17, 0, 0)
            });
            defaultPlans.Add(new Plan()
            {
                Id = Guid.NewGuid(),
                MachineId = defaultMachines[0].Id,
                ProductId = defaultProducts[1].Id,
                EmployeeId = defaultEmployees[0].Id,
                ExpectedQuantity = 13,
                ActualQuantity = 15,
                CreatedOn = new DateTime(2018, 10, 16, 12, 0, 0),
                StartTime = new DateTime(2018, 10, 16, 8, 0, 0),
                EndTime = new DateTime(2018, 10, 16, 19, 0, 0)
            });
            defaultPlans.Add(new Plan()
            {
                Id = Guid.NewGuid(),
                MachineId = defaultMachines[0].Id,
                ProductId = defaultProducts[0].Id,
                EmployeeId = defaultEmployees[0].Id,
                ExpectedQuantity = 17,
                ActualQuantity = 17,
                CreatedOn = new DateTime(2018, 10, 20, 12, 0, 0),
                StartTime = new DateTime(2018, 10, 20, 8, 0, 0),
                EndTime = new DateTime(2018, 10, 20, 16, 0, 0)
            });
            defaultPlans.Add(new Plan()
            {
                Id = Guid.NewGuid(),
                MachineId = defaultMachines[1].Id,
                ProductId = defaultProducts[0].Id,
                EmployeeId = defaultEmployees[1].Id,
                ExpectedQuantity = 41,
                ActualQuantity = 41,
                CreatedOn = new DateTime(2018, 10, 16, 12, 0, 0),
                StartTime = new DateTime(2018, 10, 16, 8, 0, 0),
                EndTime = new DateTime(2018, 10, 16, 19, 0, 0)
            });
            defaultPlans.Add(new Plan()
            {
                Id = Guid.NewGuid(),
                MachineId = defaultMachines[1].Id,
                ProductId = defaultProducts[1].Id,
                EmployeeId = defaultEmployees[0].Id,
                ExpectedQuantity = 22,
                ActualQuantity = 22,
                CreatedOn = new DateTime(2018, 10, 18, 12, 0, 0),
                StartTime = new DateTime(2018, 10, 18, 8, 0, 0),
                EndTime = new DateTime(2018, 10, 18, 17, 0, 0)
            });

            context.Plans.AddRange(defaultPlans);

            base.Seed(context);
        }
    }
}
