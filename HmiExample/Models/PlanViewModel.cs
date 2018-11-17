﻿using System;
using System.Windows.Media;

namespace HmiExample.Models
{
    public class PlanViewModel : SelectableViewModel
    {
        private Guid _id;
        private Guid _machineId;
        private Guid? _employeeId;
        private Guid _productId;
        private int? _expectedQuantity;  // output, san luong san xuat theo plan
        private int? _actualQuantity;  // output, san luong san xuat theo thuc te
        private DateTime? _startTime;
        private DateTime? _endTime;
        private bool _isProcessed;

        private ProductViewModel _product;
        private MachineViewModel _machine;
        private EmployeeViewModel _employee;

        //private string _productName;
        //private string _machineName;
        //private string _employeeName;
        private bool _isConnected;
        private SolidColorBrush _ledColor;

        public Guid Id
        {
            get { return _id; }
            set
            {
                if (_id == value) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        public Guid MachineId
        {
            get { return _machineId; }
            set
            {
                if (_machineId == value) return;
                _machineId = value;
                OnPropertyChanged();
            }
        }

        public Guid? EmployeeId
        {
            get { return _employeeId; }
            set
            {
                if (_employeeId == value) return;
                _employeeId = value;
                OnPropertyChanged();
            }
        }

        public Guid ProductId
        {
            get { return _productId; }
            set
            {
                if (_productId == value) return;
                _productId = value;
                OnPropertyChanged();
            }
        }

        public int? ExpectedQuantity
        {
            get { return _expectedQuantity; }
            set
            {
                if (_expectedQuantity == value) return;
                _expectedQuantity = value;
                OnPropertyChanged();
            }
        }

        public int? ActualQuantity
        {
            get { return _actualQuantity; }
            set
            {
                if (_actualQuantity == value) return;
                _actualQuantity = value;
                OnPropertyChanged();
            }
        }

        public DateTime? StartTime
        {
            get { return _startTime; }
            set
            {
                if (_startTime == value) return;
                _startTime = value;
                OnPropertyChanged();
            }
        }

        public DateTime? EndTime
        {
            get { return _endTime; }
            set
            {
                if (_endTime == value) return;
                _endTime = value;
                OnPropertyChanged();
            }
        }

        public ProductViewModel Product
        {
            get { return _product; }
            set
            {
                if (_product == value) return;
                _product = value;
                OnPropertyChanged();
            }
        }

        public MachineViewModel Machine
        {
            get { return _machine; }
            set
            {
                if (_machine == value) return;
                _machine = value;
                OnPropertyChanged();
            }
        }

        public EmployeeViewModel Employee
        {
            get { return _employee; }
            set
            {
                if (_employee == value) return;
                _employee = value;
                OnPropertyChanged();
            }
        }

        public bool IsProcessed
        {
            get { return _isProcessed; }
            set
            {
                if (_isProcessed == value) return;
                _isProcessed = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush LedColor
        {
            get { return _ledColor; }
            set
            {
                if (_ledColor == value) return;
                _ledColor = value;
                OnPropertyChanged();
            }
        }

        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                if (_isConnected == value) return;
                _isConnected = value;
                OnPropertyChanged();
            }
        }

        public string ProductName
        {
            get
            {
                return _product != null ? _product.Name : string.Empty;
            }
        }

        public string MachineName
        {
            get
            {
                return _machine != null ? _machine.Name : string.Empty;
            }
        }

        public string EmployeeName
        {
            get
            {
                return _employee != null ? _employee.DisplayName : string.Empty;
            }
        }

        public double TotalHours
        {
            get
            {
                var timeSpan = (EndTime - StartTime);
                return timeSpan.HasValue ? timeSpan.Value.TotalHours : 0;
            }
        }

        #region Constructors
        public PlanViewModel()
        {
        }

        public PlanViewModel(Plan plan)
        {
            _id = plan.Id;
            _machineId = plan.MachineId;
            _employeeId = plan.EmployeeId;
            _productId = plan.ProductId;
            _expectedQuantity = plan.ExpectedQuantity;
            _actualQuantity = plan.ActualQuantity;
            _startTime = plan.StartTime;
            _endTime = plan.EndTime;
            _isProcessed = plan.IsProcessed;
            Product = new ProductViewModel(plan.Product);
            Machine = new MachineViewModel(plan.Machine);
            Employee = new EmployeeViewModel(plan.Employee);
        }
        #endregion
    }
}
