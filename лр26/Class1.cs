using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace лр26
{
    public class Database
    {
        // Поле для хранения строки подключения к базе данных
        private string _connectionString;

        // Конструктор, принимающий строку подключения
        public Database(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Свойство для получения подключения к базе данных
        public IDbConnection Connection => new SqlConnection(_connectionString);

        // Метод для получения списка консультаций
        public IEnumerable<Consultation> GetConsultations()
        {
            using (var db = Connection)
            {
                // Выполнение запроса SELECT и возврат списка консультаций
                return db.Query<Consultation>("SELECT * FROM Consultations");
            }
        }

        // Метод для добавления новой консультации
        public void AddConsultation(Consultation consultation)
        {
            using (var db = Connection)
            {
                // SQL-запрос для вставки новой консультации
                var sql = "INSERT INTO Consultations (FullName, Subject, Date, TimeSlot, IsBooked) VALUES (@FullName, @Subject, @Date, @TimeSlot, @IsBooked)";
                db.Execute(sql, consultation); // Выполнение запроса
            }
        }

        // Метод для обновления статуса бронирования консультации
        public void UpdateConsultationBooking(int id, bool isBooked)
        {
            using (var db = Connection)
            {
                // SQL-запрос для обновления статуса бронирования консультации
                var sql = "UPDATE Consultations SET IsBooked = @IsBooked WHERE Id = @Id";
                db.Execute(sql, new { Id = id, IsBooked = isBooked }); // Выполнение запроса
            }
        }

        // Метод для удаления консультации по ID
        public void DeleteConsultation(int id)
        {
            using (var db = Connection)
            {
                // SQL-запрос для удаления консультации
                var sql = "DELETE FROM Consultations WHERE Id = @Id";
                db.Execute(sql, new { Id = id }); // Выполнение запроса
            }
        }

        // Метод для поиска консультаций по предмету
        public IEnumerable<Consultation> SearchConsultations(string subject)
        {
            using (var db = Connection)
            {
                // SQL-запрос для поиска консультаций по предмету
                var sql = "SELECT * FROM Consultations WHERE Subject = @Subject";
                return db.Query<Consultation>(sql, new { Subject = subject }); // Выполнение запроса и возврат списка консультаций
            }
        }
    }

    // Класс для представления консультации
    public class Consultation
    {
        public int Id { get; set; } // ID консультации
        public string FullName { get; set; } // Полное имя
        public string Subject { get; set; } // Предмет
        public DateTime Date { get; set; } // Дата консультации
        public string TimeSlot { get; set; } // Время консультации
        public bool IsBooked { get; set; } // Статус бронирования
    }
}
