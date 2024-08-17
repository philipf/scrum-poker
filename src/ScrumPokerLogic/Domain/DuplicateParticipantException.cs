
namespace ScrumPokerLogic.Domain
{
    [Serializable]
    internal class DuplicateParticipantException : BusinessException
    {

        public DuplicateParticipantException(string? message) : base(message)
        {
        }
    }
}