
namespace ScrumPokerLogic.Domain
{
    public class Participant
    {
        public bool IsFacilitator { get; internal set; }
        public bool IsObserver { get; }
        public Guid Id { get; }

        public DateTime JoinedTime { get; }

        public string Name { get; set; }

        public Participant(string name, bool isFacilitator = false, bool isObserver = false)
        {
            ValidateParticipant(name, isFacilitator);

            Id = Guid.NewGuid();
            Name = name;
            IsFacilitator = isFacilitator;
            IsObserver = isObserver;
            JoinedTime = DateTime.UtcNow;
        }

        private static void ValidateParticipant(string name, bool isFacilitator)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                if (isFacilitator)
                {
                    throw new InvalidParticipantException($"The name of the facilitator cannot be empty");
                }
                else
                {
                    throw new InvalidParticipantException($"The name of the participant cannot be empty");
                }
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Participant participant &&
                   Name == participant.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}