using System;

namespace Hardstuck.GuildWars2.Builds
{
    /// <summary>
    /// The exception is thrown when an API key is not set.
    /// </summary>
    public class NoAPIKeySetException : Exception
    {
        internal NoAPIKeySetException() : base("API key is not set.") { }
    }
}
