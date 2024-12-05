using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace WpfAppLeon
{
    public partial class EmployeeForm : Window
    {
        private ObservableCollection<MainWindow.Employee> Employees;

        public EmployeeForm(ObservableCollection<MainWindow.Employee> employees)
        {
            InitializeComponent();
            Employees = employees;
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                string.IsNullOrWhiteSpace(JobTextBox.Text) ||
                string.IsNullOrWhiteSpace(RatePerShiftTextBox.Text) ||
                string.IsNullOrWhiteSpace(VacationTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            if (!int.TryParse(RatePerShiftTextBox.Text, out int ratePerShift) ||
                !int.TryParse(VacationTextBox.Text, out int vacationDays))
            {
                MessageBox.Show("Пожалуйста, введите корректные числовые значения для отпуска и ставки.");
                return;
            }

            try
            {
                var dbHelper = new DatabaseHelper();
                dbHelper.AddEmployee(NameTextBox.Text, vacationDays, JobTextBox.Text, ratePerShift);

                // Добавляем в ObservableCollection
                var newEmployeeId = Employees.Max(emp => emp.Id) + 1;
                Employees.Add(new MainWindow.Employee
                {
                    Id = newEmployeeId,
                    Name = NameTextBox.Text,
                    VacationDays = VacationTextBox.Text,
                    Job = JobTextBox.Text,
                    RatePerShift = ratePerShift,
                    Shifts = GenerateShifts()
                });

                MessageBox.Show($"Сотрудник {NameTextBox.Text} успешно добавлен!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении сотрудника: {ex.Message}");
            }
        }

        private ObservableCollection<MainWindow.Shift> GenerateShifts()
        {
            var shifts = new ObservableCollection<MainWindow.Shift>();
            for (int i = 0; i < 7; i++) // Пример для 7 смен (неделя)
            {
                shifts.Add(new MainWindow.Shift { Worked = 0 });
            }
            return shifts;
        }

        private void DeleteEmployeeButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}