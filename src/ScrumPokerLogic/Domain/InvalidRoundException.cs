namespace ScrumPokerLogic.Domain;

[Serializable]
public class InvalidRoundException : BusinessException
{
    public InvalidRoundException(string? message) : base(message)
    {
    }
}