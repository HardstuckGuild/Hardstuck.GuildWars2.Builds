namespace Hardstuck.GuildWars2.Builds
{
    /// <summary>
    /// Represents the reason why there is not enough permissions.
    /// </summary>
    public enum NotEnoughPermissionsReason
    {
        /// <summary>
        /// The API key is invalid
        /// </summary>
        Invalid,

        /// <summary>
        /// Missing "characters" permission
        /// </summary>
        Characters,

        /// <summary>
        /// Missing "builds" permission
        /// </summary>
        Builds,
    }
}
