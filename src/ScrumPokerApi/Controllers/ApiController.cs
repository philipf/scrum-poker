using Microsoft.AspNetCore.Mvc;
using ScrumPokerLogic.Domain;

namespace ScrumPokerApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class ApiController : ControllerBase
    {
        private static readonly List<Session> sessions = [];

        [HttpPost("session")]
        [ProducesResponseType(typeof(Session), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateSession(string facilitatorName, string sessionName = "")
        {
            try
            {
                var facilitator = new Participant(facilitatorName, true);
                var session = new Session(facilitator, sessionName);
                var r = session.CreateRound();
                session.AddRound(r);
                sessions.Add(session);

                return Ok(session);

            }
            catch (BusinessException bex)
            {
                return HandleBusinessException(bex);
            }
        }

        [HttpPost("join")]
        [ProducesResponseType(typeof(Session), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult JoinSession(Guid sessionId, string name)
        {
            try
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
            catch (BusinessException bex)
            {
                return HandleBusinessException(bex);
            }
        }

        [HttpPost("vote")]
        [ProducesResponseType(typeof(VotingRound), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CastVote(Guid sessionId, string name, string vote)
        {
            try
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
            catch (BusinessException bex)
            {
                return HandleBusinessException(bex);
            }
        }

        [HttpPost("new-round")]
        [ProducesResponseType(typeof(VotingRound), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult NewRound(Guid sessionId)
        {
            try
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
            catch (BusinessException bex)
            {
                return HandleBusinessException(bex);
            }
        }

        [HttpGet("round")]
        [ProducesResponseType(typeof(VotingRound), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetCurrentRound(Guid sessionId)
        {
            try
            {
                var session = sessions.SingleOrDefault(s => s.SessionId == sessionId);

                if (session == null)
                {
                    return NotFound($"{sessionId} was not found as an active session. Make sure to first create a session before calling the join operation.");
                }

                return Ok(session.CurrentRound);

            }
            catch (BusinessException bex)
            {
                return HandleBusinessException(bex);
            }
        }

        private IActionResult HandleBusinessException(BusinessException bex)
        {
            return BadRequest(new ProblemDetails
            {
                Status = 400,
                Title = "Business Exception",
                Detail = bex.Message,
                Instance = HttpContext.Request.Path
            });
        }

        [HttpGet("session")]
        [ProducesResponseType(typeof(Session), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetSession(Guid sessionId)
        {
            try
            {
                var session = sessions.SingleOrDefault(s => s.SessionId == sessionId);

                if (session == null)
                {
                    return NotFound($"{sessionId} was not found as an active session. Make sure to first create a session before calling the join operation.");
                }

                return Ok(session);
            }
            catch (BusinessException bex)
            {
                return HandleBusinessException(bex);
            }
        }
    }
}
