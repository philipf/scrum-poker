
namespace ScrumPokerLogic.Domain
{
    [Serializable]
    internal class InvalidParticipantException : Exception
    {
        public InvalidParticipantException()
        {
        }

        public InvalidParticipantException(string? message) : base(message)
        {
        }

        public InvalidParticipantException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}