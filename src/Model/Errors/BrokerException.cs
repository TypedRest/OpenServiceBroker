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
        /// If an update or deprovisioning operation failed, this flag indicates whether or not the Service Instance is still usable. If true, the Service Instance can still be used, false otherwise. This field MUST NOT be present for errors of other operations. Defaults to true.
        /// </summary>
        public bool? InstanceUsable { get; set; }

        /// <summary>
        /// If an update operation failed, this flag indicates whether this update can be repeated or not. If true, the same update operation MAY be repeated and MAY succeed; if false, repeating the same update operation will fail again. This field MUST NOT be present for errors of other operations. Defaults to true.
        /// </summary>
        public bool? UpdateRepeatable { get; set; }

        /// <summary>
        /// Serializes the exception to an error response object.
        /// </summary>
        public Error ToResponse() => new()
        {
            ErrorCode = ErrorCode,
            Description = Message,
            InstanceUsable= InstanceUsable,
            UpdateRepeatable = UpdateRepeatable
        };

        /// <summary>
        /// Deserializes the exception from an error response object.
        /// </summary>
        public static BrokerException FromResponse(Error dto, HttpStatusCode statusCode)
        {
            var exception = dto.ErrorCode switch
            {
                ApiVersionNotSupportedException.ErrorCode => new ApiVersionNotSupportedException(dto.Description),
                AsyncRequiredException.ErrorCode => new AsyncRequiredException(dto.Description),
                BadRequestException.ErrorCode => new BadRequestException(dto.Description),
                ConcurrencyException.ErrorCode => new ConcurrencyException(dto.Description),
                ConflictException.ErrorCode => new ConflictException(dto.Description),
                GoneException.ErrorCode => new GoneException(dto.Description),
                NotFoundException.ErrorCode => new NotFoundException(dto.Description),
                RequiresAppException.ErrorCode => new RequiresAppException(dto.Description),
                MaintenanceInfoConflictException.ErrorCode => new MaintenanceInfoConflictException(dto.Description),
                _ => statusCode switch
                {
                    HttpStatusCode.PreconditionFailed => new ApiVersionNotSupportedException(dto.Description),
                    HttpStatusCode.BadRequest => new BadRequestException(dto.Description),
                    HttpStatusCode.Conflict => new ConflictException(dto.Description),
                    HttpStatusCode.Gone => new GoneException(dto.Description),
                    HttpStatusCode.NotFound => new NotFoundException(dto.Description),
                    _ => new BrokerException(dto.ErrorCode, dto.Description)
                }
            };
            exception.InstanceUsable = dto.InstanceUsable;
            exception.UpdateRepeatable = dto.UpdateRepeatable;
            return exception;
        }
    }
}
