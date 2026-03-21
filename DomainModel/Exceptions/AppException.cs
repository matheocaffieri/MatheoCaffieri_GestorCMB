using System;

namespace DomainModel.Exceptions
{
    public class AppException : Exception
    {
        public string MessageKey { get; }

        public AppException(string messageKey) : base(messageKey)
        {
            MessageKey = messageKey;
        }

        public AppException(string messageKey, Exception inner) : base(messageKey, inner)
        {
            MessageKey = messageKey;
        }
    }
}
