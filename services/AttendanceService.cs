namespace ClubManamentSystem.Services
{
    public class AttendanceService
    {
        public void MarkAttendance(int sessionId, int studentId, string status)
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

        public List<Attendance> GetAttendanceBySession(int sessionId)
            => DataStore.Attendances.Where(a => a.SessionId == sessionId).ToList();
    }
}