    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using WpfAppLEo;

    namespace WpfAppLeon
    {
        public partial class MainWindow : Window
        {
            public ObservableCollection<Employee> Employees { get; set; }

            public MainWindow()
            {
                InitializeComponent();

                Employees = LoadEmployees();

                // Устанавливаем DataContext
                DataContext = this;
            }
        public class Staff
        {
            public int Id { get; set; }
            public string FullName { get; set; }
            public int VacationDays { get; set; }
            public string CurrentJob { get; set; }
            public int RatePerShift { get; set; }
        }
        // Класс сотрудника
        public class Employee : INotifyPropertyChanged
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public string Job { get; set; }
                public string VacationDays { get; set; }
                public ObservableCollection<Shift> Shifts { get; set; }
                private decimal totalSalary;
                private decimal ratePerShift;  // Ставка для этого сотрудника

                public decimal RatePerShift
                {
                    get => ratePerShift;
                    set
                    {
                        ratePerShift = value;
                        OnPropertyChanged(nameof(RatePerShift));
                    }
                }

                public decimal TotalSalary
                {
                    get => totalSalary;
                    set
                    {
                        totalSalary = value;
                        OnPropertyChanged(nameof(TotalSalary));
                    }
                }

                public event PropertyChangedEventHandler PropertyChanged;

                protected void OnPropertyChanged(string propertyName)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            // Класс смены
            public class Shift : INotifyPropertyChanged
            {
                private int worked;
                public int Worked
                {
                    get => worked;
                    set
                    {
                        if (value != 0 && value != 1)
                        {
                            MessageBox.Show("0 - Не рабочая смена\n1 - Рабочая смена");
                            throw new ArgumentException("Значение должно быть 0 или 1");
                        }
                        worked = value;
                        OnPropertyChanged(nameof(Worked));
                    }
                }

                public event PropertyChangedEventHandler PropertyChanged;

                protected void OnPropertyChanged(string propertyName)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            }

        // Загрузка данных сотрудников
        private ObservableCollection<Employee> LoadEmployees()
        {
            var dbHelper = new DatabaseHelper();
            var staffList = dbHelper.LoadStaff();

            return new ObservableCollection<Employee>(
                staffList.Select(s => new Employee
                {
                    Id = s.Id,
                    Name = s.FullName,
                    VacationDays = s.VacationDays.ToString(),
                    Job = s.CurrentJob,
                    RatePerShift = s.RatePerShift,
                    Shifts = GenerateShifts()
                }));
        }

        // Генерация смен для месяца
        private ObservableCollection<Shift> GenerateShifts()
            {
                var shifts = new ObservableCollection<Shift>();
                for (int i = 0; i < 7; i++) // Пример для 7 смен якобы неделя
                {
                    shifts.Add(new Shift { Worked = 0 });
                }
                return shifts;
            }

            // Обновление данных (из таблицы)
            private void Update(object sender, RoutedEventArgs e)
            {
                MessageBox.Show("Данные успешно обновлены!");
            }

            // Рассчитать зарплату
            private void OnCalculateSalary(object sender, RoutedEventArgs e)
            {
                foreach (var employee in Employees)
                {
                    // Теперь для каждого сотрудника используется его индивидуальная ставка
                    employee.TotalSalary = employee.Shifts.Sum(shift => shift.Worked * employee.RatePerShift);
                }

                MessageBox.Show("Зарплата рассчитана!");
            }

            private void OpenEmployeeForm(object sender, RoutedEventArgs e)
            {
                var employeeForm = new EmployeeForm(Employees);
                employeeForm.ShowDialog();
            }

        }
    }
