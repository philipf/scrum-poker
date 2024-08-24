namespace ScrumPokerLogic.Domain
{
    public interface ISessionRepository
    {
        Session? GetById(string sessionId);
        IList<Session> GetAll();

        void Add(Session session);
        void Update(Session session);
        void Delete(Session session);
    }
}
