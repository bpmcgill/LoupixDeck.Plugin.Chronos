using LoupixDeck.PluginSdk;

namespace LoupixDeck.Plugin.Chronos;

/// <summary>
/// LoupixDeck plugin for everything time-related: moon phase, time in any
/// timezone, date, day of week, week number, day of year, and countdowns.
/// </summary>
public sealed class ChronosPlugin : LoupixPlugin
{
    public override PluginMetadata Metadata { get; } = new()
    {
        Id = "chronos",
        Name = "Chronos",
        Version = new Version(1, 0, 0),
        SdkVersion = SdkInfo.Version,
        Author = "Ben McGill",
        Description = "Moon phase, time, date, day of week, week number, day of year, and countdowns."
    };

    public override void Initialize(IPluginHost host)
    {
        MoonPhaseImageCommand.LoadFrames(typeof(ChronosPlugin).Assembly);
        host.Logger.Info($"Chronos plugin loaded ({MoonPhaseImageCommand.FrameCount} moon frames).");
    }

    public override IEnumerable<IPluginCommand> GetCommands()
    {
        yield return new MoonPhaseImageCommand();
        yield return new MoonPhaseTextCommand();
        yield return new TimeCommand();
        yield return new DateCommand();
        yield return new DayOfWeekCommand();
        yield return new WeekCommand();
        yield return new DayOfYearCommand();
        yield return new CountdownCommand();
    }
}