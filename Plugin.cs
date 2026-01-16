using MediaBrowser.Common.Plugins;
using JellyfinAccountManager.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Model.Serialization;

namespace JellyfinAccountManager;

public class Plugin : BasePlugin<PluginConfiguration>
{
    public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer) 
        : base(applicationPaths, xmlSerializer)
    {
    }

    public override string Name => "Account Manager";

    public override Guid Id => Guid.Parse("b174a844-ac6c-42bc-a58a-8e93c7cb7912");

    public override string Description => "Automatically blocks user accounts after a configured period of inactivity.";
}