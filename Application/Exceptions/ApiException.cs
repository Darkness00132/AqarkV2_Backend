namespace Application.Exceptions
{
    public abstract class ApiException : Exception
    {
        protected ApiException(string message) : base(message) { }
    }

    public sealed class BadRequestException : ApiException
    {
        public BadRequestException(string message)
            : base(message) { }
    }

    public sealed class UnauthorizedException : ApiException
    {
        public UnauthorizedException(string message =
            "غير مصرح. برجاء تسجيل الدخول ثم المحاولة مرة أخرى.")
            : base(message) { }
    }

    public sealed class ForbiddenException : ApiException
    {
        public ForbiddenException(string message)
            : base(message) { }
    }

    public sealed class NotFoundException : ApiException
    {
        public NotFoundException(string message)
            : base(message) { }
    }

    public sealed class ConflictException : ApiException
    {
        public ConflictException(string message)
            : base(message) { }
    }
}