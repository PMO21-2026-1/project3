using System;
using System.Collections.Generic;
using ClubManamentSystem.Models;

namespace ClubManamentSystem.DataStorage
{
    public class DataStore
    {
        public static List<Student> Students = new List<Student>();
        public static List<Teacher> Teachers = new List<Teacher>();
        public static List<Club> Clubs = new List<Club>();
        public static List<Enrollment> Enrollments = new List<Enrollment>();
        public static List<Session> Sessions = new List<Session>();
        public static List<Attendance> Attendances = new List<Attendance>();

        public static void Initialize()
        {
            // --- СТУДЕНТИ ---
            Students.Add(new Student { Id = 1, FullName = "Максим Кодер", Email = "max@test.com", Phone = "0991112233" });
            Students.Add(new Student { Id = 2, FullName = "Олег База", Email = "oleg@test.com", Phone = "0987776655" });
            Students.Add(new Student { Id = 3, FullName = "Анна Студентка", Email = "anna@test.com", Phone = "0639998877" });

            // --- ВИКЛАДАЧІ ---
            Teachers.Add(new Teacher { Id = 1, FullName = "Андрій Тренер", Specialization = "Пауерліфтинг" });
            Teachers.Add(new Teacher { Id = 2, FullName = "Ігор Дотнетчик", Specialization = "Програмування C#" });
            Teachers.Add(new Teacher { Id = 3, FullName = "Василь Скюель", Specialization = "Бази даних PostgreSQL" });

            // --- ГУРТКИ ---
            Clubs.Add(new Club { Id = 1, Name = "Залізний Світ", Address = "вул. Дудаєва, 2", TeacherId = 1, Description = "Качалка", Schedule = "Пн-Ср-Пт" });
            Clubs.Add(new Club { Id = 2, Name = "Розробка Telegram-ботів", Address = "вул. Степана Бандери, 12", TeacherId = 2, Description = "Пишемо ботів на C# та .NET", Schedule = "Вів-Чет 18:00" });
            Clubs.Add(new Club { Id = 3, Name = "Архітектура БД", Address = "вул. Франка, 15", TeacherId = 3, Description = "Проєктування в PostgreSQL", Schedule = "Субота 12:00" });

            // --- ЗАПИСИ (Хто куди ходить) ---
            Enrollments.Add(new Enrollment { Id = 1, StudentId = 1, ClubId = 2 }); // Максим ходить на ботів
            Enrollments.Add(new Enrollment { Id = 2, StudentId = 1, ClubId = 3 }); // Максим ще й на бази даних
            Enrollments.Add(new Enrollment { Id = 3, StudentId = 2, ClubId = 3 }); // Олег тільки на бази даних
            Enrollments.Add(new Enrollment { Id = 4, StudentId = 3, ClubId = 1 }); // Анна ходить в качалку

            // --- ЗАНЯТТЯ (Сесії) ---
            Sessions.Add(new Session { Id = 1, ClubId = 2, Room = "Аудиторія 404", Date = DateTime.Today, StartTime = new TimeSpan(18, 0, 0), EndTime = new TimeSpan(19, 30, 0) });
            Sessions.Add(new Session { Id = 2, ClubId = 3, Room = "Комп'ютерний клас", Date = DateTime.Today.AddDays(1), StartTime = new TimeSpan(12, 0, 0), EndTime = new TimeSpan(14, 0, 0) });
            Sessions.Add(new Session { Id = 3, ClubId = 1, Room = "Зал 1", Date = DateTime.Today, StartTime = new TimeSpan(16, 0, 0), EndTime = new TimeSpan(18, 0, 0) });
        }
    }
}