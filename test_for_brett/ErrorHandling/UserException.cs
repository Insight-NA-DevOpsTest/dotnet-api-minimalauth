
using System.Runtime.Serialization;

namespace test_for_brett.ErrorHandling;

/// <summary>
/// An Exception whose message is meant to be returned to the caller.
/// Stack traces and inner exceptions will still be hidden.
/// UserException is intercepted by the ExceptionMiddleware and the
/// message added to the message property in the output to the caller.
/// </summary>
[System.Serializable]
public class UserException : System.Exception
{
    public UserException() { }
    public UserException(string message) : base(message) { }
    public UserException(string message, System.Exception inner) : base(message, inner) { }
    protected UserException(SerializationInfo info, StreamingContext context) : base() { }
}