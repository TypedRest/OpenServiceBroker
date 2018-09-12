using System;
using System.Net;

namespace OpenServiceBroker.Errors
{
    /// <summary>
    /// The request to the Service Broker failed.
    /// </summary>
    public class BrokerException : Exception
    {
        public string ErrorCode { get; }

        public HttpStatusCode HttpCode { get; }

        public BrokerException(string message, string errorCode, HttpStatusCode httpCode = (HttpStatusCode)422 /*UnprocessableEntity*/)
            : base(message)
        {
            ErrorCode = errorCode;
            HttpCode = httpCode;
        }

        /// <summary>
        /// Serializes the exception to an error response object.
        /// </summary>
        public Error ToResponse() => new Error
        {
            ErrorCode = ErrorCode,
            Description = Message
        };

        /// <summary>
        /// Deserializes the exception from an error response object.
        /// </summary>
        public static BrokerException FromResponse(Error dto, HttpStatusCode statusCode)
        {
            switch (dto.ErrorCode)
            {
                case ApiVersionNotSupportedException.ErrorCode:
                    return new ApiVersionNotSupportedException(dto.Description);
                case AsyncRequiredException.ErrorCode:
                    return new AsyncRequiredException(dto.Description);
                case BadRequestException.ErrorCode:
                    return new BadRequestException(dto.Description);
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
            }

            switch (statusCode)
            {
                case HttpStatusCode.PreconditionFailed:
                    return new ApiVersionNotSupportedException(dto.Description);
                case HttpStatusCode.BadRequest:
                    return new BadRequestException(dto.Description);
                case HttpStatusCode.Conflict:
                    return new ConflictException(dto.Description);
                case HttpStatusCode.Gone:
                    return new GoneException(dto.Description);
                case HttpStatusCode.NotFound:
                    return new NotFoundException(dto.Description);
            }

            return new BrokerException(dto.ErrorCode, dto.Description);
        }
    }
}
