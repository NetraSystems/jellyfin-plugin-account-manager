using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Library;
using MediaBrowser.Model.Tasks;
using Microsoft.Extensions.Logging;
using JellyfinAccountManager.Configuration;
using Jellyfin.Data;
using Jellyfin.Database.Implementations.Enums;

namespace JellyfinAccountManager.ScheduledTasks;

/// <summary>
/// Scheduled task that blocks inactive user accounts based on configuration.
/// </summary>
public class AccountManagerTask : IScheduledTask
{
    private readonly IUserManager _userManager;
    private readonly ILogger<AccountManagerTask> _logger;
    private readonly Plugin _plugin;

    public AccountManagerTask(IUserManager userManager, ILogger<AccountManagerTask> logger, Plugin plugin)
    {
        _userManager = userManager;
        _logger = logger;
        _plugin = plugin;
    }

    public string Name => "Check for Inactive Accounts";

    public string Description => "Blocks user accounts that have been inactive for the configured period.";

    public string Category => "Account Manager";

    public string Key => "AccountManagerInactiveCheck";

    public async Task ExecuteAsync(IProgress<double>? progress, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting inactive account check...");

        var config = _plugin.Configuration;
        var blockAfterDays = config.BlockAfterDays;

        if (blockAfterDays <= 0)
        {
            _logger.LogWarning("BlockAfterDays is set to {Days}, skipping account check.", blockAfterDays);
            return;
        }

        var cutoffDate = DateTime.UtcNow.AddDays(-blockAfterDays);
        _logger.LogInformation("Checking for accounts inactive since {CutoffDate} ({Days} days ago)", 
            cutoffDate, blockAfterDays);

        var users = _userManager.Users.ToList();
        var totalUsers = users.Count;
        var blockedCount = 0;

        for (int i = 0; i < users.Count; i++)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Task cancelled by user.");
                return;
            }

            var user = users[i];
            
            // Skip already disabled users
            if (user.HasPermission(PermissionKind.IsDisabled))
            {
                _logger.LogDebug("User {UserName} is already disabled, skipping.", user.Username);
                progress?.Report((double)(i + 1) / totalUsers * 100);
                continue;
            }

            // Check last activity date
            var lastActivityDate = user.LastActivityDate ?? DateTime.MinValue;

            if (lastActivityDate < cutoffDate)
            {
                _logger.LogInformation("Blocking user {UserName} (Last activity: {LastActivity})", 
                    user.Username, lastActivityDate);

                try
                {
                    // Disable the user
                    user.SetPermission(PermissionKind.IsDisabled, true);
                    await _userManager.UpdateUserAsync(user);
                    
                    blockedCount++;
                    _logger.LogInformation("Successfully blocked user {UserName}", user.Username);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to block user {UserName}", user.Username);
                }
            }
            else
            {
                _logger.LogDebug("User {UserName} is active (Last activity: {LastActivity})", 
                    user.Username, lastActivityDate);
            }

            progress?.Report((double)(i + 1) / totalUsers * 100);
        }

        _logger.LogInformation("Inactive account check completed. Blocked {BlockedCount} out of {TotalUsers} users.", 
            blockedCount, totalUsers);
    }

    public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
    {
        // Run daily at 3 AM
        return new[]
        {
            new TaskTriggerInfo
            {
                Type = TaskTriggerInfoType.DailyTrigger,
                TimeOfDayTicks = TimeSpan.FromHours(3).Ticks
            }
        };
    }
}