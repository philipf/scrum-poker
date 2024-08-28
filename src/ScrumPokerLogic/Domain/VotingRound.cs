
using System;

namespace ScrumPokerLogic.Domain
{
    public class VotingRound
    {
        public string Status { get; set; }

        public int RoundId { get; }
        public string[] VotingScale { get; }
        public List<Vote> Votes { get; } = [];

        public DateTime StartTime = DateTime.UtcNow;

        public VotingRound(int roundId, List<Participant> participants, string[] votingScale)
        {
            if (roundId <= 0)
                throw new InvalidRoundException($"The roundId {roundId} has an invalid value");

            ArgumentNullException.ThrowIfNull(votingScale, nameof(votingScale));

            RoundId = roundId;
            VotingScale = votingScale;
            SetStatus(VotingStatus.New);
            InitVotes(participants);
        }

        private void InitVotes(List<Participant> participants)
        {
            ArgumentNullException.ThrowIfNull(participants);

            Votes.Clear();

            foreach (Participant participant in participants)
            {
                var vote = new Vote(participant);
                Votes.Add(vote);
            }
        }

        public void AddParticipant(Participant participant)
        {
            ArgumentNullException.ThrowIfNull(participant);

            var participants = Votes.Select(vote => vote.Participant);

            // Add the participant to the voting round with null vote
            if (!participants.Contains(participant))
            {
                CastVote(participant, null);
            }
        }

        public void CastVote(Participant participant, string? voteValue)
        {
            ArgumentNullException.ThrowIfNull(participant);

            if (Status == VotingStatus.Revealed || Status == VotingStatus.Closed)
            {
                throw new InvalidVoteException($"Votes are now longer allowed. The current round status is {Status}");
            }

            if (voteValue != null)
            {
                ValidateVoteInScale(voteValue);
            }

            SetStatus(VotingStatus.InProgress);

            var v = Votes.Single(v => v.Participant == participant);
            v.Value = voteValue;
            v.VoteTime = DateTime.UtcNow;
        }

        private void ValidateVoteInScale(string vote)
        {
            if (!VotingScale.Contains(vote))
                throw new InvalidVoteException($"The vote [{vote}] is not a valid vote because it not in the voting scale [{string.Join(',', VotingScale)}]");
        }

        internal void SetStatus(string votingStatus)
        {
            Status = votingStatus;
        }
    }
}