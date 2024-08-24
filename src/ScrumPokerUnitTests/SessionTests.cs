using ScrumPokerLogic.Domain;
using ScrumPokerLogic.Infra.Persistance.InMemory;

namespace ScrumPokerUnitTests
{
    [TestClass]
    public class SessionTests
    {
        [TestMethod]
        public void CreateNewSessionWithoutVotingScale()
        {
            // acquire
            var facilitator = new Participant("Philip Fourie", true);

            // act
            var session = Session.NewSession(new SessionRepository(), facilitator, "SessionId", "Test 1");

            Assert.IsNotNull(session);
            Assert.IsTrue(session.Participants.Count == 1);
            Assert.AreEqual(session.Participants[0], facilitator);
            Assert.AreEqual(session.SessionName, "Test 1");
            Assert.AreEqual(session.VotingScale, Session.DefaultVotingScale);
        }

        [TestMethod]
        [DataRow(["S", "M", "L"])]
        [DataRow([])]
        [DataRow(["1", "2", "3"])]
        public void SessionWithCustomVotingScale(string[] value)
        {
            // acquire
            var facilitator = new Participant("Philip Fourie", true);

            // act
            var session = Session.NewSession(new SessionRepository(), facilitator, "AAA", "Test 1", value);

            Assert.IsNotNull(session);
            Assert.AreEqual(session.VotingScale, value);
        }

        [TestMethod]
        [DataRow(["S", "M", "L", "M"])]
        public void SessionWithDuplicateVotingScale(string[] value)
        {
            // acquire
            var facilitator = new Participant("Philip Fourie", true);

            // act
            var ex = Assert.ThrowsException<ArgumentException>(() => Session.NewSession(new SessionRepository(), facilitator, "AAA", "Test", value));

            // assert
            Assert.IsTrue(ex.Message.Contains("Duplicate value found in voting scale"));
        }

        [TestMethod]
        [DataRow(["a", "a"])]
        [DataRow(["a", "b", "a"])]
        public void DuplicateSessionCheck(string[] sessionIds)
        {
            // acquire
            var facilitator = new Participant("Philip Fourie", true);

            // act
            var sessionRepo = new SessionRepository();

            try
            {
                foreach (var sessionId in sessionIds)
                {
                    var session = Session.NewSession(sessionRepo, facilitator, sessionId);
                    sessionRepo.Add(session);
                }
            }
            catch (DuplicateSessionIdException ex)
            {
                Assert.IsTrue(ex.Message.Contains("already exists"));
                return;
            }

            Assert.Fail("Duplicate session Id exception not triggered");
        }
    }
}