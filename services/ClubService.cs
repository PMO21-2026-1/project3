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
    }
}