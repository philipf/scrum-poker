using ScrumPokerLogic.Domain;

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
            var session = new Session(facilitator, "Test 1");

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
            var session = new Session(facilitator, "Test 1", value);

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
            var ex = Assert.ThrowsException<ArgumentException>(() => new Session(facilitator, "Test", value));

            // assert
            Assert.IsTrue(ex.Message.Contains("Duplicate value found in voting scale"));
        }
    }
}