
namespace ScrumPokerLogic.Domain
{
    [Serializable]
    class InvalidVoteException : BusinessException
    {
        public InvalidVoteException(string? message) : base(message)
        {
        }
    }
}