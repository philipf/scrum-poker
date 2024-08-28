using Microsoft.AspNetCore.Mvc;
using ScrumPokerLogic.Domain;
using ScrumPokerLogic.Infra.Persistance.InMemory;

namespace ScrumPokerApi.Controllers
{
    /// <summary>
    /// API Controller for managing Scrum Poker sessions.
    /// </summary>
    [ApiController]
    [Route("api")]
    [Produces("application/json")]
    public class ApiController : ControllerBase
    {
        private readonly ISessionRepository sessionRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiController"/> class.
        /// </summary>
        /// <param name="sessionRepository">The session repository instance.</param>
        public ApiController(ISessionRepository sessionRepository)
        {
            this.sessionRepository = sessionRepository;
        }

        /// <summary>
        /// Creates a new Scrum Poker session.
        /// </summary>
        /// <param name="facilitatorName">The name of the facilitator who creates the session.</param>
        /// <param name="sessionId">Optional. The ID of the session. If not provided, a new one will be generated.</param>
        /// <param name="sessionName">Optional. The name of the session. Default is an empty string.</param>
        /// <returns>The newly created session.</returns>
        /// <response code="200">Returns the newly created session.</response>
        /// <response code="400">If there is a business exception or input validation fails.</response>
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

        /// <summary>
        /// Joins an existing Scrum Poker session.
        /// </summary>
        /// <param name="sessionId">The ID of the session to join.</param>
        /// <param name="name">The name of the participant joining the session.</param>
        /// <returns>The session with the new participant added.</returns>
        /// <response code="200">Returns the session with the new participant added.</response>
        /// <response code="404">If the session ID is not found.</response>
        /// <response code="400">If there is a business exception or input validation fails.</response>
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

        /// <summary>
        /// Casts a vote in the current round of a session.
        /// </summary>
        /// <param name="sessionId">The ID of the session where the vote is cast.</param>
        /// <param name="name">The name of the participant casting the vote.</param>
        /// <param name="vote">The vote being cast by the participant.</param>
        /// <returns>The current voting round with the updated vote.</returns>
        /// <response code="200">Returns the updated voting round.</response>
        /// <response code="404">If the session or participant is not found.</response>
        /// <response code="400">If there is a business exception or input validation fails.</response>
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

        [HttpPost("reveal")]
        [ProducesResponseType(typeof(VotingRound), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult RevealVotes(Guid sessionId, string name)
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

                session.RevealVotes(participant);

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

        /// <summary>
        /// Retrieves the current round of a session.
        /// </summary>
        /// <param name="sessionId">The ID of the session whose current round is retrieved.</param>
        /// <returns>The current voting round of the session.</returns>
        /// <response code="200">Returns the current voting round.</response>
        /// <response code="404">If the session ID is not found.</response>
        /// <response code="400">If there is a business exception or input validation fails.</response>
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

        /// <summary>
        /// Retrieves a session by its ID.
        /// </summary>
        /// <param name="sessionId">The ID of the session to retrieve.</param>
        /// <returns>The session with the specified ID.</returns>
        /// <response code="200">Returns the session with the specified ID.</response>
        /// <response code="404">If the session ID is not found.</response>
        /// <response code="400">If there is a business exception or input validation fails.</response>
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
    }
}
