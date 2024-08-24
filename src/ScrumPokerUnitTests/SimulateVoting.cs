using ScrumPokerLogic.Domain;
using ScrumPokerLogic.Infra.Persistance.InMemory;

namespace ScrumPokerUnitTests
{
    [TestClass]
    public class SimulateVoting
    {

        [TestMethod]
        public void SingleRound()
        {
            var f = new Participant("PF", true);
            var s = Session.NewSession(new SessionRepository(), f);

            var p1 = new Participant("TF");
            var p2 = new Participant("CF");

            s.AddParticpant(p1);
            s.AddParticpant(p2);

            Assert.AreEqual(3, s.Participants.Count);
            Assert.AreEqual(0, s.Rounds.Count);

            var r = s.CreateRound();
            s.AddRound(r);
                
            Assert.AreEqual(1, s.Rounds.Count);

            s.CastVote(p1, "1");
            s.CastVote(p2, "2");

            Assert.AreEqual("1", r.Votes.Single(v => v.Participant == p1).Value);
            Assert.AreEqual("2", r.Votes.Single(v => v.Participant == p2).Value);
            Assert.AreEqual(null, r.Votes.Single(v => v.Participant == f).Value);
        }

        [TestMethod]
        public void TwoRound()
        {
            var f = new Participant("PF", true);
            var s = Session.NewSession(new SessionRepository(), f);

            var p1 = new Participant("TF");
            var p2 = new Participant("CF");

            s.AddParticpant(p1);
            s.AddParticpant(p2);

            var r1 = s.CreateRound();
            s.AddRound(r1);

            s.CastVote(p1, "1");
            s.CastVote(p2, "2");

            var r2 = s.CreateRound();
            s.AddRound(r2);

            s.CastVote(p1, "3");
            s.CastVote(p2, "5");

            Assert.AreEqual("3", r2.Votes.Single(v => v.Participant == p1).Value);
            Assert.AreEqual("5", r2.Votes.Single(v => v.Participant == p2).Value);
            Assert.AreEqual(null, r2.Votes.Single(v => v.Participant == f).Value);
        }
    }
}
