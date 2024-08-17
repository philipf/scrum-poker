
namespace ScrumPokerLogic.Domain
{
    [Serializable]
    internal class InvalidRoundException : BusinessException
    {
        public InvalidRoundException(string? message) : base(message)
        {
        }
    }
}