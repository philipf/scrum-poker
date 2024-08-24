using ScrumPokerLogic.Domain;

namespace ScrumPokerLogic.Infra.Persistance.InMemory;

public class SessionRepository : ISessionRepository
{
    private static readonly List<Session> sessions = new();
    private static readonly object lockObj = new();

    public void Add(Session session)
    {
        lock (lockObj)
        {
            sessions.Add(session);
        }
    }

    public void Delete(Session session)
    {
        lock (lockObj)
        {
            sessions.Remove(session);
        }
    }

    public IList<Session> GetAll()
    {
        lock (lockObj)
        {
            return sessions;
        }
    }

    public Session? GetById(string sessionId)
    {
        lock (lockObj)
        {
            return sessions.SingleOrDefault(s => s.SessionId == sessionId);
        }
    }

    public void Update(Session session)
    {
        // no op
    }
}
