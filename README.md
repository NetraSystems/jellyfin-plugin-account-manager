# Jellyfin Account Manager

Automatically manage inactive user accounts in your Jellyfin server.

## Features

- 🔒 **Automatic Account Blocking**: Disables user accounts after a configured period of inactivity
- ⏰ **Scheduled Task**: Runs daily at 3 AM by default (configurable in Jellyfin)
- 📊 **Detailed Logging**: Track all account management actions
- ⚙️ **Easy Configuration**: Simple configuration through Jellyfin's plugin settings

## Installation

### Method 1: Via Jellyfin Plugin Repository (Recommended)

1. Open Jellyfin Dashboard
2. Go to **Plugins** → **Repositories**
3. Add this repository URL: `https://raw.githubusercontent.com/NetraSystems/jellyfin-plugin-account-manager/main/manifest.json`
4. Go to **Plugins** → **Catalog**
5. Find "Account Manager" and click **Install**
6. Restart Jellyfin

### Method 2: Manual Installation

1. Download the latest `JellyfinAccountManager.dll` from [Releases](https://github.com/YOUR-USERNAME/jellyfin-plugin-account-manager/releases)
2. Place it in your Jellyfin plugins directory:
   - Linux: `/var/lib/jellyfin/plugins/`
   - Windows: `C:\ProgramData\Jellyfin\Server\plugins\`
3. Restart Jellyfin

## Configuration

1. Go to **Dashboard** → **Plugins** → **Account Manager**
2. Set **Block After Days** to your desired inactivity period (default: 30 days)
3. Click **Save**

## Usage

The plugin runs automatically as a scheduled task. You can also:

- **Manual Run**: Dashboard → Scheduled Tasks → "Check for Inactive Accounts" → Run
- **View Logs**: Dashboard → Logs (filter for "JellyfinAccountManager")
- **Modify Schedule**: Dashboard → Scheduled Tasks → "Check for Inactive Accounts" → Configure

## How It Works

1. The plugin checks all users every day at 3 AM
2. For each user, it compares their last activity date with the configured threshold
3. If a user hasn't been active for the specified number of days, the plugin disables their account
4. Already disabled users are skipped
5. All actions are logged for your review

## Requirements

- Jellyfin 10.11.0 or higher
- .NET 9.0

## Building from Source
```bash
dotnet build -c Release
```

The compiled DLL will be in `bin/Release/net9.0/JellyfinAccountManager.dll`

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

MIT License - See LICENSE file for details

## Support

If you encounter any issues or have questions:
- Open an issue on [GitHub](https://github.com/YOUR-USERNAME/jellyfin-plugin-account-manager/issues)
- Check the Jellyfin logs for detailed error messages

## Changelog

### v1.0.0 (Initial Release)
- Automatic account blocking based on inactivity
- Configurable inactivity period
- Scheduled task runs daily at 3 AM
- Detailed logging of all actions
