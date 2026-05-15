using System.Collections.Generic;
using System.Linq;
using ClubManamentSystem.Models;
using ClubManamentSystem.DataStorage;

namespace ClubManamentSystem.Services
{
    public class AttendanceService
    {
        public void MarkAttendance(int sessionId, int studentId, AttendanceStatus status)
        {
            var existing = DataStore.Attendances
                .FirstOrDefault(a => a.SessionId == sessionId && a.StudentId == studentId);

            if (existing != null)
            {
                existing.Status = status;
            }
            else
            {
                int nextId = DataStore.Attendances.Count == 0 ? 1 : DataStore.Attendances.Max(a => a.Id) + 1;
                DataStore.Attendances.Add(new Attendance
                {
                    Id = nextId,
                    SessionId = sessionId,
                    StudentId = studentId,
                    Status = status
                });
            }
        }

        public void MarkSessionAttendance(int sessionId, List<int> presentStudentIds)
        {
            var session = DataStore.Sessions.FirstOrDefault(s => s.Id == sessionId);
            if (session == null) return;

            var enrolledStudents = DataStore.Enrollments
                .Where(e => e.ClubId == session.ClubId)
                .Select(e => e.StudentId)
                .ToList();

            foreach (var studentId in enrolledStudents)
            {
                if (DataStore.Attendances.Any(a => a.SessionId == sessionId && a.StudentId == studentId))
                    continue;

                var status = presentStudentIds.Contains(studentId)
                    ? AttendanceStatus.Present
                    : AttendanceStatus.Absent;

                int nextId = DataStore.Attendances.Count == 0 ? 1 : DataStore.Attendances.Max(a => a.Id) + 1;
                DataStore.Attendances.Add(new Attendance
                {
                    Id = nextId,
                    SessionId = sessionId,
                    StudentId = studentId,
                    Status = status
                });
            }
        }

        public List<Attendance> GetAttendanceBySession(int sessionId)
            => DataStore.Attendances.Where(a => a.SessionId == sessionId).ToList();
    }
}