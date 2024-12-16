namespace ProniaProject.Utils.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string errorMessage) : base(errorMessage) { }

        public NotFoundException() : base("404 Not Found") { }

    }
}
