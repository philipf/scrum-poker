
namespace ScrumPokerLogic.Domain
{
    [Serializable]
    public class DuplicateParticipantException : BusinessException
    {

        public DuplicateParticipantException(string? message) : base(message)
        {
        }
    }
}