
namespace ScrumPokerLogic.Domain
{
    [Serializable]
    public class InvalidParticipantException : BusinessException
    {
        public InvalidParticipantException(string? message) : base(message)
        {
        }
    }
}