using System;
using System.Net;

namespace OpenServiceBroker.Errors
{
    public class BrokerException : Exception
    {
        public string ErrorCode { get; }

        public HttpStatusCode HttpCode { get; }

        public BrokerException(string message, string errorCode, HttpStatusCode httpCode = (HttpStatusCode)422)
            : base(message)
        {
            ErrorCode = errorCode;
            HttpCode = httpCode;
        }

        public Error ToDto() => new Error
        {
            ErrorCode = ErrorCode,
            Description = Message
        };

        public static BrokerException FromDto(Error dto)
        {
            switch (dto.ErrorCode)
            {
                case AsyncRequiredException.ErrorCode:
                    return new AsyncRequiredException(dto.Description);
                case ConcurrencyException.ErrorCode:
                    return new ConcurrencyException(dto.Description);
                case ConflictException.ErrorCode:
                    return new ConflictException(dto.Description);
                case GoneException.ErrorCode:
                    return new GoneException(dto.Description);
                case NotFoundException.ErrorCode:
                    return new NotFoundException(dto.Description);
                case RequiresAppException.ErrorCode:
                    return new RequiresAppException(dto.Description);
                default:
                    return new BrokerException(dto.ErrorCode, dto.Description);
            }
        }
    }
}
