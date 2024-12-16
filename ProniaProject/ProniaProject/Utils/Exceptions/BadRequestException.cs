namespace ProniaProject.Utils.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string errorMessage) : base(errorMessage) { }

        public BadRequestException() : base("Wrong Request") { }
    }
}
