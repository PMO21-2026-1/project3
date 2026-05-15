using System.Collections.Generic;
using System.Linq;
using ClubManamentSystem.Models;
using ClubManamentSystem.DataStorage;

namespace ClubManamentSystem.Services
{
    public class StudentService
    {
        public bool EnrollStudent(int studentId, int clubId)
        {
            if (!DataStore.Students.Any(s => s.Id == studentId) || !DataStore.Clubs.Any(c => c.Id == clubId))
                return false;

            if (DataStore.Enrollments.Any(e => e.StudentId == studentId && e.ClubId == clubId))
                return false;

            int nextId = DataStore.Enrollments.Count == 0 ? 1 : DataStore.Enrollments.Max(e => e.Id) + 1;
            DataStore.Enrollments.Add(new Enrollment
            {
                Id = nextId,
                StudentId = studentId,
                ClubId = clubId
            });
            return true;
        }

        public List<Session> GetStudentSchedule(int studentId)
        {
            var clubIds = DataStore.Enrollments
                .Where(e => e.StudentId == studentId)
                .Select(e => e.ClubId)
                .ToList();

            return DataStore.Sessions
                .Where(s => clubIds.Contains(s.ClubId))
                .OrderBy(s => s.Date)
                .ToList();
        }

    }
}