using System;
using System.Collections.Generic;
using System.Linq;
using ClubManamentSystem.Models;
using ClubManamentSystem.DataStorage;

namespace ClubManamentSystem.Services
{
    public class ClubService
    {
        public int AddClub(string name, string description, string schedule, string address, int teacherId)
        {
            int nextId = DataStore.Clubs.Count == 0 ? 1 : DataStore.Clubs.Max(c => c.Id) + 1;

            var club = new Club
            {
                Id = nextId,
                Name = name,
                Description = description,
                Schedule = schedule,
                Address = address,
                TeacherId = teacherId
            };

            DataStore.Clubs.Add(club);
            return nextId;
        }

        public List<Club> SearchByAddress(string addressPart)
        {
            return DataStore.Clubs
                .Where(c => c.Address.Contains(addressPart, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public Club GetById(int id) => DataStore.Clubs.FirstOrDefault(c => c.Id == id);

        public List<Club> GetAll() => DataStore.Clubs.ToList();

     
        public List<Student> GetStudentsInClub(int clubId)
        {
            return DataStore.Enrollments
                .Where(e => e.ClubId == clubId)
                .Join(DataStore.Students, e => e.StudentId, s => s.Id, (e, s) => s)
                .ToList();
        }
    }
}