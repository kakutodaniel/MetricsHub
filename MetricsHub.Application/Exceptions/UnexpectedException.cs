namespace MetricsHub.Application.Exceptions
{
    public class UnexpectedException : Exception
    {
        public string Event { get; }

        public UnexpectedException(string @event, Exception ex)
            : base("Unexpected Exception", ex)
        {
            this.Event = @event;
        }
    }
}
