using Npgsql;
using System;
using System.Collections.Generic;

namespace WpfAppLeon
{
    public class DatabaseHelper
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=EmployeeLeon;Username=postgres;Password=sa;";

        public List<MainWindow.Staff> LoadStaff()
        {
            var staffList = new List<MainWindow.Staff>();

            try
            {
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT id, fullname, vacationdays, currentjob, ratepershift FROM staff";

                    using (var command = new NpgsqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            staffList.Add(new MainWindow.Staff
                            {
                                Id = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                VacationDays = reader.GetInt32(2),
                                CurrentJob = reader.GetString(3),
                                RatePerShift = reader.GetInt32(4)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при загрузке сотрудников: {ex.Message}");
            }

            return staffList;
        }

        /// <summary>
        /// Добавляет нового сотрудника в базу данных.
        /// </summary>
        public void AddEmployee(string fullName, int vacationDays, string currentJob, int ratePerShift)
        {
            try
            {
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = @"
                        INSERT INTO staff (fullname, vacationdays, currentjob, ratepershift)
                        VALUES (@fullname, @vacationdays, @currentjob, @ratepershift)";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("fullname", fullName);
                        command.Parameters.AddWithValue("vacationdays", vacationDays);
                        command.Parameters.AddWithValue("currentjob", currentJob);
                        command.Parameters.AddWithValue("ratepershift", ratePerShift);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при добавлении сотрудника: {ex.Message}");
            }
        }
    }
}