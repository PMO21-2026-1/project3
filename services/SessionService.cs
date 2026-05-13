namespace ClubManamentSystem.Services
{
    public class SessionService
    {
        public int AddSession(int clubId, string room, DateTime date, TimeSpan startTime)
        {
            int nextId = DataStore.Sessions.Count == 0 ? 1 : DataStore.Sessions.Max(s => s.Id) + 1;

            var session = new Session
            {
                Id = nextId,
                ClubId = clubId,
                Room = room,
                Date = date,
                StartTime = startTime
            };

            DataStore.Sessions.Add(session);
            return nextId;
        }

        public List<Session> GetSessionsByClub(int clubId)
            => DataStore.Sessions.Where(s => s.ClubId == clubId).OrderBy(s => s.Date).ToList();

        public bool UpdateRoom(int sessionId, string newRoom)
        {
            var session = DataStore.Sessions.FirstOrDefault(s => s.Id == sessionId);
            if (session == null) return false;
            session.Room = newRoom;
            return true;
        }
    }
}