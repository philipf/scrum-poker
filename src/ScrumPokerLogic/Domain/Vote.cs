
namespace ScrumPokerLogic.Domain
{
    public class Vote
    {
        public string? Value { get; set; }
        public Participant Participant { get; set; }

        public DateTime VoteTime { get; set; }

        public Vote(Participant participant)
        {
            Participant = participant ?? throw new ArgumentNullException(nameof(participant));
            VoteTime = DateTime.UtcNow;
        }
    }
}