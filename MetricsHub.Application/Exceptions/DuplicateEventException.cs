namespace MetricsHub.Application.Exceptions
{
    public class DuplicateEventException : Exception
    {
        public DuplicateEventException(string eventId)
            : base($"Event with id '{eventId}' already exists.")
        {
        }
    }
}
