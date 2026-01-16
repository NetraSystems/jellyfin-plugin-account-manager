using MediaBrowser.Model.Plugins;

namespace JellyfinAccountManager.Configuration;

public class PluginConfiguration : BasePluginConfiguration
{
    /// <summary>
    /// Gets or sets the number of days after which inactive accounts should be blocked.
    /// Default is 30 days.
    /// </summary>
    public int BlockAfterDays { get; set; } = 30;
}