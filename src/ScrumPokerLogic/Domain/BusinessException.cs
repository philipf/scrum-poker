namespace ScrumPokerLogic.Domain
{
    public class BusinessException : Exception
    {
        public BusinessException(string? message) : base(message) { }
        
    }
}