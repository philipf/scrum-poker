using Microsoft.AspNetCore.Mvc;
using ScrumPokerLogic.Domain;
using ScrumPokerLogic.Infra.Persistance.InMemory;

namespace ScrumPokerApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class ApiController : ControllerBase
    {
        private readonly ISessionRepository sessionRepository;

        public ApiController(ISessionRepository sessionRepository)
        {
            this.sessionRepository = sessionRepository;
        }

        [HttpPost("session")]
        [ProducesResponseType(typeof(Session), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateSession(string facilitatorName, string? sessionId, string sessionName = "")
        {
            try
            {

                var facilitator = new Participant(facilitatorName, true);
                var session = Session.NewSession(new SessionRepository(), facilitator, sessionId, sessionName);
                var r = session.CreateRound();
                session.AddRound(r);
                sessionRepository.Add(session);

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
        public IActionResult JoinSession(string sessionId, string name)
        {
            try
            {
                Session? session = sessionRepository.GetById(sessionId);

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
        public IActionResult CastVote(string sessionId, string name, string vote)
        {
            try
            {
                Session? session = sessionRepository.GetById(sessionId);

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
        public IActionResult NewRound(string sessionId)
        {
            try
            {
                Session? session = sessionRepository.GetById(sessionId);

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
        public IActionResult GetCurrentRound(string sessionId)
        {
            try
            {
                Session? session = sessionRepository.GetById(sessionId);

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
        public IActionResult GetSession(string sessionId)
        {
            try
            {
                Session? session = sessionRepository.GetById(sessionId);

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
