
namespace ScrumPokerLogic.Domain
{
    [Serializable]
    internal class InvalidRoundException : Exception
    {
        public InvalidRoundException()
        {
        }

        public InvalidRoundException(string? message) : base(message)
        {
        }

        public InvalidRoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}