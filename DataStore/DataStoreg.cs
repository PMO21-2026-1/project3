using ClubManamentSystem.Models;
using System.Collections.Generic;

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
            Teachers.Add(new Teacher { Id = 1, FullName = "Андрій Тренер", Specialization = "Пауерліфтинг" });
            Clubs.Add(new Club { Id = 1, Name = "Залізний Світ", Address = "вул. Дудаєва, 2", TeacherId = 1 });
        }
    }
}