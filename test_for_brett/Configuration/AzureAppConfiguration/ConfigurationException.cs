using System.Runtime.Serialization;

namespace test_for_brett.Configuration;

[Serializable]
public class ConfigurationException : Exception
{
    public ConfigurationException() { }
    public ConfigurationException(string message) : base(message) { }
    public ConfigurationException(string message, System.Exception inner) : base(message, inner) { }
    protected ConfigurationException(SerializationInfo info, StreamingContext context) : base() { }
}