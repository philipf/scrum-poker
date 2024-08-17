
namespace ScrumPokerLogic.Domain
{
    [Serializable]
    internal class InvalidParticipantException : BusinessException
    {
        public InvalidParticipantException(string? message) : base(message)
        {
        }
    }
}