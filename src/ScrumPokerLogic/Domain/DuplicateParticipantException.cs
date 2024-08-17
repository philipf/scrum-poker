
namespace ScrumPokerLogic.Domain
{
    [Serializable]
    internal class DuplicateParticipantException : Exception
    {
        public DuplicateParticipantException()
        {
        }

        public DuplicateParticipantException(string? message) : base(message)
        {
        }

        public DuplicateParticipantException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}