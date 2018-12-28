using ProductionEquipmentControlSoftware.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace ProductionEquipmentControlSoftware.Data
{
    // DropCreateDatabaseIfModelChanges
    // DropCreateDatabaseAlways
    public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var defaultCreatedOn = new DateTime(2018, 11, 18, 12, 0, 0);

            #region Seed Machines
            IList<Machine> defaultMachines = new List<Machine>();

            defaultMachines.Add(new Machine() { Id = Guid.NewGuid(), Name = "Machine 1", Code = "M001", TagIndex = 1, CumulativeCount = 4590, CreatedOn = defaultCreatedOn });
            defaultMachines.Add(new Machine() { Id = Guid.NewGuid(), Name = "Machine 2", Code = "M002", TagIndex = 2, CumulativeCount = 1203, CreatedOn = defaultCreatedOn });
            defaultMachines.Add(new Machine() { Id = Guid.NewGuid(), Name = "Machine 3", Code = "M003", TagIndex = 3, CumulativeCount = 31, CreatedOn = defaultCreatedOn });
            defaultMachines.Add(new Machine() { Id = Guid.NewGuid(), Name = "Machine 4", Code = "M004", TagIndex = 4, CumulativeCount = 523, CreatedOn = defaultCreatedOn });

            context.Machines.AddRange(defaultMachines);
            #endregion

            #region Seed Product
            IList<Product> defaultProducts = new List<Product>();

            defaultProducts.Add(new Product() { Id = Guid.NewGuid(), Name = "Product 1", Code = "P001", CreatedOn = defaultCreatedOn });
            defaultProducts.Add(new Product() { Id = Guid.NewGuid(), Name = "Product 2", Code = "P002", CreatedOn = defaultCreatedOn });
            defaultProducts.Add(new Product() { Id = Guid.NewGuid(), Name = "Product 3", Code = "P003", CreatedOn = defaultCreatedOn });
            defaultProducts.Add(new Product() { Id = Guid.NewGuid(), Name = "Product 4", Code = "P004", CreatedOn = defaultCreatedOn });
            defaultProducts.Add(new Product() { Id = Guid.NewGuid(), Name = "Product 5", Code = "P005", CreatedOn = defaultCreatedOn });

            context.Products.AddRange(defaultProducts);
            #endregion

            #region Seed Employee
            IList<Employee> defaultEmployees = new List<Employee>();

            defaultEmployees.Add(new Employee()
            {
                Id = Guid.NewGuid(),
                Code = "E001",
                Email = "employee1@gmail.com",
                DisplayName = "Employee 1",
                FirstName = "A",
                MiddleName = "Van",
                LastName = "Tran",
                PhoneNumber = "0901234567",
                CreatedOn = defaultCreatedOn
            });
            defaultEmployees.Add(new Employee()
            {
                Id = Guid.NewGuid(),
                Code = "E002",
                Email = "employee2@gmail.com",
                DisplayName = "Employee 2",
                FirstName = "B",
                MiddleName = "Van",
                LastName = "Nguyen",
                PhoneNumber = "0971234567",
                CreatedOn = defaultCreatedOn
            });
            defaultEmployees.Add(new Employee()
            {
                Id = Guid.NewGuid(),
                Code = "E003",
                Email = "employee3@gmail.com",
                DisplayName = "Employee 3",
                FirstName = "C",
                MiddleName = "Van",
                LastName = "Le",
                PhoneNumber = "0921234567",
                CreatedOn = defaultCreatedOn
            });

            context.Employees.AddRange(defaultEmployees);
            #endregion

            #region Seed Plan
            IList<Plan> defaultPlans = new List<Plan>();

            // employee 1 - machine 1 - product 1
            defaultPlans.Add(new Plan()
            {
                Id = Guid.NewGuid(),
                MachineId = defaultMachines[0].Id,
                ProductId = defaultProducts[0].Id,
                EmployeeId = defaultEmployees[0].Id,
                ExpectedQuantity = 17,
                ActualQuantity = 17,
                NotGoodQuantity = 2,
                CreatedOn = new DateTime(2018, 11, 20, 12, 0, 0),
                //StartTime = new DateTime(2018, 11, 20, 8, 0, 0),
                //EndTime = new DateTime(2018, 11, 20, 16, 0, 0),
                TotalMilliseconds = 8 / 1000 / 60 / 60,
                IsProcessed = true
            });
            defaultPlans.Add(new Plan()
            {
                Id = Guid.NewGuid(),
                MachineId = defaultMachines[0].Id,
                ProductId = defaultProducts[0].Id,
                EmployeeId = defaultEmployees[0].Id,
                ExpectedQuantity = 25,
                ActualQuantity = 26,
                NotGoodQuantity = 5,
                CreatedOn = new DateTime(2018, 11, 21, 12, 0, 0),
                //StartTime = new DateTime(2018, 11, 21, 9, 0, 0),
                //EndTime = new DateTime(2018, 11, 21, 16, 0, 0),
                TotalMilliseconds = 7 / 1000 / 60 / 60,
                IsProcessed = true
            });

            // employee 1 - machine 1 - product 2
            defaultPlans.Add(new Plan()
            {
                Id = Guid.NewGuid(),
                MachineId = defaultMachines[0].Id,
                ProductId = defaultProducts[1].Id,
                EmployeeId = defaultEmployees[0].Id,
                ExpectedQuantity = 13,
                ActualQuantity = 15,
                NotGoodQuantity = 4,
                CreatedOn = new DateTime(2018, 11, 16, 12, 0, 0),
                //StartTime = new DateTime(2018, 11, 16, 8, 0, 0),
                //EndTime = new DateTime(2018, 11, 16, 19, 0, 0),
                TotalMilliseconds = 11 / 1000 / 60 / 60,
                IsProcessed = true
            });
            defaultPlans.Add(new Plan()
            {
                Id = Guid.NewGuid(),
                MachineId = defaultMachines[0].Id,
                ProductId = defaultProducts[1].Id,
                EmployeeId = defaultEmployees[0].Id,
                ExpectedQuantity = 19,
                ActualQuantity = 17,
                NotGoodQuantity = 0,
                CreatedOn = new DateTime(2018, 11, 22, 12, 0, 0),
                //StartTime = new DateTime(2018, 11, 22, 7, 30, 0),
                //EndTime = new DateTime(2018, 11, 22, 19, 0, 0),
                TotalMilliseconds = 12 / 1000 / 60 / 60,
                IsProcessed = true
            });

            // employee 1 - machine 2 - product 1
            defaultPlans.Add(new Plan()
            {
                Id = Guid.NewGuid(),
                MachineId = defaultMachines[1].Id,
                ProductId = defaultProducts[0].Id,
                EmployeeId = defaultEmployees[0].Id,
                ExpectedQuantity = 34,
                ActualQuantity = 31,
                NotGoodQuantity = 11,
                CreatedOn = new DateTime(2018, 11, 19, 12, 0, 0),
                //StartTime = new DateTime(2018, 11, 19, 6, 0, 0),
                //EndTime = new DateTime(2018, 11, 19, 15, 0, 0),
                TotalMilliseconds = 9 / 1000 / 60 / 60,
                IsProcessed = true
            });
            defaultPlans.Add(new Plan()
            {
                Id = Guid.NewGuid(),
                MachineId = defaultMachines[1].Id,
                ProductId = defaultProducts[0].Id,
                EmployeeId = defaultEmployees[0].Id,
                ExpectedQuantity = 34,
                ActualQuantity = 31,
                NotGoodQuantity = 0,
                CreatedOn = new DateTime(2018, 11, 22, 12, 0, 0),
                //StartTime = new DateTime(2018, 11, 22, 9, 0, 0),
                //EndTime = new DateTime(2018, 11, 22, 16, 30, 0),
                TotalMilliseconds = 7 / 1000 / 60 / 60,
                IsProcessed = true
            });

            // employee 1 - machine 2 - product 2
            defaultPlans.Add(new Plan()
            {
                Id = Guid.NewGuid(),
                MachineId = defaultMachines[1].Id,
                ProductId = defaultProducts[1].Id,
                EmployeeId = defaultEmployees[0].Id,
                ExpectedQuantity = 22,
                ActualQuantity = 22,
                NotGoodQuantity = 2,
                CreatedOn = new DateTime(2018, 11, 18, 12, 0, 0),
                //StartTime = new DateTime(2018, 11, 18, 8, 0, 0),
                //EndTime = new DateTime(2018, 11, 18, 17, 0, 0),
                TotalMilliseconds = 9 / 1000 / 60 / 60,
                IsProcessed = true
            });

            // employee 2 - machine 1 - product 2
            defaultPlans.Add(new Plan()
            {
                Id = Guid.NewGuid(),
                MachineId = defaultMachines[0].Id,
                ProductId = defaultProducts[1].Id,
                EmployeeId = defaultEmployees[1].Id,
                ExpectedQuantity = 32,
                ActualQuantity = 29,
                NotGoodQuantity = 0,
                CreatedOn = new DateTime(2018, 11, 18, 12, 0, 0),
                //StartTime = new DateTime(2018, 11, 18, 8, 0, 0),
                //EndTime = new DateTime(2018, 11, 18, 17, 0, 0),
                TotalMilliseconds = 9 / 1000 / 60 / 60,
                IsProcessed = true
            });

            // employee 2 - machine 1 - product 1
            defaultPlans.Add(new Plan()
            {
                Id = Guid.NewGuid(),
                MachineId = defaultMachines[0].Id,
                ProductId = defaultProducts[0].Id,
                EmployeeId = defaultEmployees[1].Id,
                ExpectedQuantity = 23,
                ActualQuantity = 28,
                NotGoodQuantity = 2,
                CreatedOn = new DateTime(2018, 11, 23, 12, 0, 0),
                //StartTime = new DateTime(2018, 11, 23, 10, 0, 0),
                //EndTime = new DateTime(2018, 11, 23, 18, 0, 0),
                TotalMilliseconds = 8 / 1000 / 60 / 60,
                IsProcessed = true
            });

            // employee 2 - machine 2 - product 1
            defaultPlans.Add(new Plan()
            {
                Id = Guid.NewGuid(),
                MachineId = defaultMachines[1].Id,
                ProductId = defaultProducts[0].Id,
                EmployeeId = defaultEmployees[1].Id,
                ExpectedQuantity = 41,
                ActualQuantity = 41,
                NotGoodQuantity = 12,
                CreatedOn = new DateTime(2018, 11, 16, 12, 0, 0),
                //StartTime = new DateTime(2018, 11, 16, 8, 0, 0),
                //EndTime = new DateTime(2018, 11, 16, 19, 0, 0),
                TotalMilliseconds = 11 / 1000 / 60 / 60,
                IsProcessed = true
            });
            defaultPlans.Add(new Plan()
            {
                Id = Guid.NewGuid(),
                MachineId = defaultMachines[1].Id,
                ProductId = defaultProducts[0].Id,
                EmployeeId = defaultEmployees[1].Id,
                ExpectedQuantity = 64,
                ActualQuantity = 45,
                NotGoodQuantity = 0,
                CreatedOn = new DateTime(2018, 11, 23, 12, 0, 0),
                //StartTime = new DateTime(2018, 11, 23, 8, 0, 0),
                //EndTime = new DateTime(2018, 11, 23, 17, 0, 0),
                TotalMilliseconds = 9 / 1000 / 60 / 60,
                IsProcessed = true
            });

            context.Plans.AddRange(defaultPlans);
            #endregion

            base.Seed(context);
        }
    }
}
