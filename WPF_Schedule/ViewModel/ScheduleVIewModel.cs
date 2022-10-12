using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WPF_Schedule.Database;
using WPF_Schedule.Model;
using System.Text.Json;
using WPF_Schedule.Properties;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Windows;

namespace WPF_Schedule.ViewModel
{
    public class ScheduleVIewModel : BaseViewModel
    {
        private string _city;
        private IEnumerable<string> _citiesList;
        private string _shop;
        private IEnumerable<string> _shopsList;
        private string _employee;
        private IEnumerable<string> _employeesList;
        private string _brigade;
        private string _shift;

        public string City
        {
            get => _city;
            set
            {
                _city = value;
                OnPropertyChanged("City");

                SelectedCity();
            }
        }

        public IEnumerable<string> CitiesList
        {
            get => _citiesList;
            set
            {
                _citiesList = value;
                OnPropertyChanged("CitiesList");
            }
        }

        public string Shop
        {
            get => _shop;
            set
            {
                _shop = value;
                OnPropertyChanged("Shop");

                SelectedShop();
            }
        }

        public IEnumerable<string> ShopsList
        {
            get => _shopsList;
            set
            {
                _shopsList = value;
                OnPropertyChanged("ShopsList");
            }
        }

        public string Employee
        {
            get => _employee;
            set
            {
                _employee = value;
                OnPropertyChanged("Employee");

                SelectedEmployee();
            }
        }

        public IEnumerable<string> EmployeesList
        {
            get => _employeesList;
            set
            {
                _employeesList = value;
                OnPropertyChanged("EmployeesList");
            }
        }

        public string Brigade
        {
            get => _brigade;
            set
            {
                _brigade = value;
                OnPropertyChanged("Brigade");
            }
        }

        public string Shift
        {
            get => _shift;
            set
            {
                _shift = value;
                OnPropertyChanged("Shift");
            }
        }

        private RelayCommand _saveCommand;

        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand((args) =>
                {
                    if (City != "" && Shop != "" && Employee != "")
                    {
                        JsonSerializerOptions options = new JsonSerializerOptions()
                        {
                            AllowTrailingCommas = true,
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                            WriteIndented = true
                        };

                        using (DatabaseContext database = new DatabaseContext())
                        {
                            Schedule schedule = (from item in database.Schedules.ToList()
                                                 where item.City == City
                                                 && item.Shop == Shop
                                                 && item.Employee == Employee
                                                 select item).First();

                            using (FileStream fs = new FileStream("Schedule.json", FileMode.Create))
                            {
                                JsonSerializer.Serialize<Schedule>(fs, schedule, options);
                            }
                            //небольшой отход от mvvm для упрощения
                            MessageBox.Show("Файл успешно записан", "Уведомление",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        //небольшой отход от mvvm для упрощения
                        MessageBox.Show("Не все поля заполнены!", "Уведомление",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }));
            }
        }

        public ScheduleVIewModel()
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                CitiesList = (from item in database.Schedules.ToList()
                              select item.City).Distinct();

                ShopsList = (from item in database.Schedules.ToList()
                             select item.Shop).Distinct();

                EmployeesList = from item in database.Schedules.ToList()
                                select item.Employee;

                City = "";
                Shop = "";
                Employee = "";
                Brigade = "Выберите сотрудника...";
                Shift = "Выберите сотрудника...";
            }
        }

        private void SelectedCity()
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                ShopsList = (from item in database.Schedules.ToList()
                             where item.City == City
                             select item.Shop).Distinct();

                EmployeesList = from item in database.Schedules.ToList()
                                where item.City == City
                                select item.Employee;

                Shop = "";
                Employee = "";
            }
        }

        private void SelectedShop()
        {
            if (City != "" && Shop != "")
            {
                using (DatabaseContext database = new DatabaseContext())
                {
                    EmployeesList = from item in database.Schedules.ToList()
                                    where item.City == City && item.Shop == Shop
                                    select item.Employee;

                    Employee = "";
                }
            }
        }

        private void SelectedEmployee()
        {
            if (Employee != "")
            {
                using (DatabaseContext database = new DatabaseContext())
                {
                    var schedule = (from item in database.Schedules.ToList()
                                    where item.Employee == Employee
                                    select item).First();

                    Brigade = schedule.Brigade;
                    Shift = schedule.Shift;
                }
            }
            else
            {
                Brigade = "Выберите сотрудника...";
                Shift = "Выберите сотрудника...";
            }
        }
    }
}
