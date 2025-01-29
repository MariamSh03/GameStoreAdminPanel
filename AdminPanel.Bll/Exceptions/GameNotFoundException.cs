namespace AdminPanel.Bll.Exceptions;

public class GameNotFoundException : Exception
{
    public GameNotFoundException()
        : base()
    {
    }

    public GameNotFoundException(string message)
        : base(message)
    {
    }

    public GameNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public GameNotFoundException(string key, string message)
        : base($"Game with key '{key}' not found. {message}")
    {
    }
}