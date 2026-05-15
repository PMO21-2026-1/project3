using ClubManamentSystem.Models;
using ClubManamentSystem.DataStorage;
using System.Collections.Generic;
using System.Linq;

namespace ClubManamentSystem.Services
{
    public class TeacherService
    {
        public int AddTeacher(string fullName, string email, string specialization, string phone)
        {
            int nextId = DataStore.Teachers.Count == 0 ? 1 : DataStore.Teachers.Max(t => t.Id) + 1;

            var teacher = new Teacher
            {
                Id = nextId,
                FullName = fullName,
                Email = email,
                Specialization = specialization,
                Phone = phone
            };

            DataStore.Teachers.Add(teacher);
            return nextId;
        }   

        public Teacher GetById(int id) => DataStore.Teachers.FirstOrDefault(t => t.Id == id);

        public List<Teacher> GetAll() => DataStore.Teachers.ToList();

        public bool Update(int id, string fullName, string email, string spec, string phone)
        {
            var teacher = GetById(id);
            if (teacher == null) return false;

            teacher.FullName = fullName;
            teacher.Email = email;
            teacher.Specialization = spec;
            teacher.Phone = phone;
            return true;
        }

        public bool Delete(int id)
        {
            var teacher = GetById(id);
            if (teacher == null) return false;

            var clubs = DataStore.Clubs.Where(c => c.TeacherId == id).ToList();
            foreach (var club in clubs) club.TeacherId = 0;

            DataStore.Teachers.Remove(teacher);
            return true;
        }
    }
}