
namespace ScrumPokerLogic.Domain
{
    [Serializable]
    internal class InvalidVoteException : BusinessException
    {
        public InvalidVoteException(string? message) : base(message)
        {
        }
    }
}