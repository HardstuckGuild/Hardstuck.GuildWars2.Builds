using System;

namespace Hardstuck.GuildWars2.Builds
{
    /// <summary>
    /// The exception is thrown when a specific API key permissions are not granted by the API key.
    /// </summary>
    public sealed class NotEnoughPermissionsException : Exception
    {
        /// <summary>
        /// The reason for missing permissions
        /// </summary>
        public NotEnoughPermissionsReason MissingPermission { get; }

        internal NotEnoughPermissionsException(string message, NotEnoughPermissionsReason reason) : base(message)
        {
            MissingPermission = reason;
        }
    }
}
