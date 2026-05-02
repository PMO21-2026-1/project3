using ClubManamentSystem.Models;
using System.Collections.Generic;
using ClubManamentSystem.DataStorage;
using System.Linq;
using System;
public class ClubManager
{
	
	public bool EnrollStudent(int studentId, int clubId)
	{
		var studentExists = DataStore.Students.Any(s => s.Id == studentId); 
		if (!studentExists) return false;

		var clubExists = DataStore.Clubs.Any(s => s.Id == clubId);
		if (!clubExists) return false;

		if (DataStore.Enrollments.Any(e => e.StudentId == studentId && e.ClubId == clubId)) return false;
        
        int nextId = 0;
        if (DataStore.Enrollments.Count == 0)
        {
            nextId = 1;
        }
        else { nextId = DataStore.Enrollments.Max(s => s.Id) + 1; }
        DataStore.Enrollments.Add(new Enrollment
        {
            Id = nextId,
            StudentId = studentId,
            ClubId = clubId
        });
        return true;
	}
    public int CreateSession(int clubId, string room, DateTime date, TimeSpan startTime, TimeSpan endTime)
    {
        var clubExists = DataStore.Clubs.Any(c => c.Id == clubId);
        if (!clubExists) return -1;

        int nextId = 0;
		if (DataStore.Sessions.Count == 0)
		{
			nextId = 1;
		}
		else { nextId = DataStore.Sessions.Max(s => s.Id) + 1; }

    var session = new Session
	{
		Id = nextId,
		ClubId = clubId,
		Room = room,
		Date = date,
		StartTime = startTime,
		EndTime = endTime
	}; 

    DataStore.Sessions.Add(session); 
    return nextId; 
	}

	public void MarkAttendance(int sessionId, List<int> presentStudentIds)
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

        DataStore.Attendances.Add(new Attendance
		{
			Id = DataStore.Attendances.Count + 1,
			SessionId = sessionId,
			StudentId = studentId,
			Status = status
		});
    }
	}
    public List<Student> GetStudentsInClub(int clubId)
    {
        return DataStore.Enrollments
            .Where(e => e.ClubId == clubId)
            .Join(DataStore.Students,
                  e => e.StudentId,
                  s => s.Id,
                  (e, s) => s)
            .ToList();
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