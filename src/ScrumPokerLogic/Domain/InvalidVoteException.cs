
namespace ScrumPokerLogic.Domain
{
    [Serializable]
    internal class InvalidVoteException : Exception
    {
        public InvalidVoteException()
        {
        }

        public InvalidVoteException(string? message) : base(message)
        {
        }

        public InvalidVoteException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}