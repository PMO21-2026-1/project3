using System;
using System.Collections.Generic;

namespace ClubManamentSystem.Models
{

    public enum AttendanceStatus
    {
        Present,   
        Absent,    
        Excused,    
        Late        
    }
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
    }

    public class Teacher
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Specialization { get; set; }
        public string Phone { get; set; }
    }

    public class Club
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Schedule { get; set; }
        public string Address { get; set; }
        public int TeacherId { get; set; }
    }

    public class Session
    {
        public int Id { get; set; }
        public int ClubId { get; set; }
        public string Room { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

    }

    public class Enrollment
    {

        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ClubId { get; set; }
    }

    public class Attendance
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public int StudentId { get; set; }

        public AttendanceStatus Status { get; set; }
    }
}