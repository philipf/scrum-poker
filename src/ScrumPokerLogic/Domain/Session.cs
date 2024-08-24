namespace ScrumPokerLogic.Domain
{
    public class Session
    {
        public string SessionId { get; private set; } = "N/A";
        public string SessionName { get; set; } = string.Empty;
        public List<VotingRound> Rounds { get; } = [];
        public List<Participant> Participants { get; } = [];

        public static readonly string[] DefaultVotingScale = ["0", "1", "2", "3", "5", "8", "13", "21", "34", "55", "89", "?", "Break"];
        public string[] VotingScale { get; private set;} = DefaultVotingScale;

        public DateTime StartTime = DateTime.UtcNow;

        // Return the latest round, which is the current round
        public VotingRound? CurrentRound => Rounds.Count == 0 ? null : Rounds[^1];

        public static Session NewSession(ISessionRepository repo,
                       Participant facilitator,
                       string? sessionId = null,
                       string? sessionName = null,
                       string[]? votingScale = null)
        {

            sessionId ??= Guid.NewGuid().ToString();

            Session? existingSession = repo.GetById(sessionId);
            if (existingSession != null)
            {
                throw new DuplicateSessionIdException($"The sessionId [{sessionId}] already exists");
            }

            var newSession = new Session();
            newSession.SessionName = sessionName ?? string.Empty;
            newSession.SessionId = sessionId;

            if (votingScale != null)
            {
                ValidateVotingScale(votingScale);
                newSession.VotingScale = votingScale;
            }

            newSession.AddFacilitator(facilitator);

            return newSession;
        }


        private static void ValidateVotingScale(string[] votingScale)
        {
            ArgumentNullException.ThrowIfNull(nameof(votingScale), "Voting scale cannot be null.");

            HashSet<string> uniqueValues = [];

            foreach (var value in votingScale)
            {
                if (!uniqueValues.Add(value))
                {
                    throw new ArgumentException($"Duplicate value found in voting scale: {value}");
                }
            }
        }

        private void AddFacilitator(Participant facilitator)
        {
            facilitator.IsFacilitator = true;
            AddParticpant(facilitator);
        }

        public VotingRound CreateRound()
        {
            var newRoundId = 1;
            if (Rounds.Count > 0)
            {
                newRoundId = CurrentRound!.RoundId + 1;
            }

            var round = new VotingRound(newRoundId, Participants, VotingScale);
            return round;
        }

        public void CastVote(Participant participant, string? vote)
        {
            ArgumentNullException.ThrowIfNull(participant, nameof(participant));

            var round = CurrentRound;
            if (CurrentRound == null)
            {
                throw new Exception("No rounds have been created");
            }

            var p = Participants.SingleOrDefault(p =>  p.Id == participant.Id);

            if (p == null)
            {
                throw new Exception($"Participant {participant.Id} - { participant.Name } has not been added to the session");
            }

            CurrentRound.AddParticipant(participant);
            CurrentRound.CastVote(participant, vote);
        }


        public void AddRound(VotingRound round)
        {
            if (HasDuplicateRoundId(round))
            {
                throw new InvalidRoundException($"Cannot add round because the round id {round.RoundId} already exists in the session");
            }

            Rounds.Add(round);
        }

        private bool HasDuplicateRoundId(VotingRound round)
        {
            return Rounds.Any(r => r.RoundId == round.RoundId);
        }

        public void AddParticpant(Participant particpant)
        {
            if (HasDuplicateParticpants(particpant))
            {
                throw new DuplicateParticipantException($"A participant named {particpant.Name} already exists in this session");
            }

            Participants.Add(particpant);

            CurrentRound?.AddParticipant(particpant);
        }

        private bool HasDuplicateParticpants(Participant particpant)
        {
            return Participants.Contains(particpant);
        }

        public override bool Equals(object? obj)
        {
            return obj is Session session &&
                   SessionId == session.SessionId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SessionId);
        }
    }
}

