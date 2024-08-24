
namespace ScrumPokerLogic.Domain
{
    public class DuplicateSessionIdException : BusinessException
    {

        public DuplicateSessionIdException(string? message) : base(message)
        {
        }
    }
}