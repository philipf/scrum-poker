namespace ScrumPokerLogic.Domain
{
    internal class BusinessException : Exception
    {
        public BusinessException(string? message) : base(message) { }
        
    }
}