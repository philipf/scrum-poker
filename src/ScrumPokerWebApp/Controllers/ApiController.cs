using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ScrumPokerLogic.Domain;

namespace ScrumPokerWebApp.Controllers
{
    [ApiController]
    [Route("api")]
    public class ApiController : ControllerBase
    {
        private static readonly List<Session> sessions = [];

        [HttpPost("session")]
        [ProducesResponseType(typeof(Session), StatusCodes.Status200OK)]
        public IActionResult CreateSession(string facilitatorName, string sessionName = "")
        {
            var facilitator = new Participant(facilitatorName, true);
            var session = new Session(facilitator, sessionName);
            var r = session.CreateRound();
            session.AddRound(r);
            sessions.Add(session);
            return Ok(session);
        }

        [HttpPost("join")]
        [ProducesResponseType(typeof(Session), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult JoinSession(Guid sessionId, string name)
        {
            var session = sessions.SingleOrDefault(s => s.SessionId == sessionId);

            if (session == null)
            {
                return NotFound($"{sessionId} was not found as an active session. Make sure to first create a session before calling the join operation.");
            }

            var participant = new Participant(name, false);
            session.AddParticpant(participant);

            return Ok(session);
        }

        [HttpPost("vote")]
        [ProducesResponseType(typeof(VotingRound), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult CastVote(Guid sessionId, string name, string vote)
        {
            var session = sessions.SingleOrDefault(s => s.SessionId == sessionId);

            if (session == null)
            {
                return NotFound($"{sessionId} was not found as an active session. Make sure to first create a session before calling the join operation.");
            }

            var participant = session.Participants.SingleOrDefault(p => p.Name == name);

            if (participant == null)
            {
                return NotFound($"{name} was not found as participant in this session. Make sure to join the session first");
            }

            session.CastVote(participant, vote);

            return Ok(session.CurrentRound);
        }

        [HttpPost("new-round")]
        [ProducesResponseType(typeof(VotingRound), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult NewRound(Guid sessionId)
        {
            var session = sessions.SingleOrDefault(s => s.SessionId == sessionId);

            if (session == null)
            {
                return NotFound($"{sessionId} was not found as an active session. Make sure to first create a session before calling the join operation.");
            }

            var r = session.CreateRound();
            session.Rounds.Add(r);

            return Ok(r);
        }

        [HttpGet("round")]
        [ProducesResponseType(typeof(VotingRound), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCurrentRound(Guid sessionId)
        {
            var session = sessions.SingleOrDefault(s => s.SessionId == sessionId);

            if (session == null)
            {
                return NotFound($"{sessionId} was not found as an active session. Make sure to first create a session before calling the join operation.");
            }

            return Ok(session.CurrentRound);
        }

        [HttpGet("session")]
        [ProducesResponseType(typeof(Session), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetSession(Guid sessionId)
        {
            var session = sessions.SingleOrDefault(s => s.SessionId == sessionId);

            if (session == null)
            {
                return NotFound($"{sessionId} was not found as an active session. Make sure to first create a session before calling the join operation.");
            }

            return Ok(session);
        }
    }
}
