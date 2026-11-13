using System.Globalization;
using LoupixDeck.PluginSdk;

namespace LoupixDeck.Plugin.Chronos;

/// <summary>Selectable timezones. The host renders this as a dropdown.</summary>
public enum Timezone
{
    UTC,
    America_New_York,
    America_Chicago,
    America_Denver,
    America_Los_Angeles,
    America_Phoenix,
    America_Anchorage,
    Pacific_Honolulu,
    Europe_London,
    Europe_Dublin,
    Europe_Lisbon,
    Europe_Paris,
    Europe_Berlin,
    Europe_Madrid,
    Europe_Rome,
    Europe_Amsterdam,
    Europe_Stockholm,
    Europe_Warsaw,
    Europe_Athens,
    Europe_Istanbul,
    Asia_Dubai,
    Asia_Kolkata,
    Asia_Singapore,
    Asia_Shanghai,
    Asia_Hong_Kong,
    Asia_Tokyo,
    Asia_Seoul,
    Australia_Perth,
    Australia_Sydney,
    Australia_Melbourne,
    Pacific_Auckland,
    America_Sao_Paulo,
    America_Mexico_City,
}

/// <summary>Selectable time formats. The host renders this as a dropdown.</summary>
public enum TimeFormat
{
    HHmm,
    HHmmss,
    hmmtt,
}

/// <summary>Selectable date formats. The host renders this as a dropdown.</summary>
public enum DateFormat
{
    IsoDate,
    MonthDay,
    FullDate,
    DayMonthYear,
}

/// <summary>Selectable day-of-week formats. The host renders this as a dropdown.</summary>
public enum DayOfWeekFormat
{
    Full,
    Short,
}

/// <summary>Months, 1-based. The host renders this as a dropdown.</summary>
public enum Month
{
    January = 1,
    February = 2,
    March = 3,
    April = 4,
    May = 5,
    June = 6,
    July = 7,
    August = 8,
    September = 9,
    October = 10,
    November = 11,
    December = 12,
}

internal static class Param
{
    public static T Enum<T>(string[] p, int i, T fallback) where T : struct, System.Enum
    {
        if (p.Length > i && System.Enum.TryParse(p[i], out T v))
            return v;
        return fallback;
    }
}

/// <summary>Shows the time in a configured timezone.</summary>
internal sealed class TimeCommand : IDisplayCommand
{
    private static readonly Dictionary<Timezone, string> IanaIds = new()
    {
        [Timezone.UTC] = "UTC",
        [Timezone.America_New_York] = "America/New_York",
        [Timezone.America_Chicago] = "America/Chicago",
        [Timezone.America_Denver] = "America/Denver",
        [Timezone.America_Los_Angeles] = "America/Los_Angeles",
        [Timezone.America_Phoenix] = "America/Phoenix",
        [Timezone.America_Anchorage] = "America/Anchorage",
        [Timezone.Pacific_Honolulu] = "Pacific/Honolulu",
        [Timezone.Europe_London] = "Europe/London",
        [Timezone.Europe_Dublin] = "Europe/Dublin",
        [Timezone.Europe_Lisbon] = "Europe/Lisbon",
        [Timezone.Europe_Paris] = "Europe/Paris",
        [Timezone.Europe_Berlin] = "Europe/Berlin",
        [Timezone.Europe_Madrid] = "Europe/Madrid",
        [Timezone.Europe_Rome] = "Europe/Rome",
        [Timezone.Europe_Amsterdam] = "Europe/Amsterdam",
        [Timezone.Europe_Stockholm] = "Europe/Stockholm",
        [Timezone.Europe_Warsaw] = "Europe/Warsaw",
        [Timezone.Europe_Athens] = "Europe/Athens",
        [Timezone.Europe_Istanbul] = "Europe/Istanbul",
        [Timezone.Asia_Dubai] = "Asia/Dubai",
        [Timezone.Asia_Kolkata] = "Asia/Kolkata",
        [Timezone.Asia_Singapore] = "Asia/Singapore",
        [Timezone.Asia_Shanghai] = "Asia/Shanghai",
        [Timezone.Asia_Hong_Kong] = "Asia/Hong_Kong",
        [Timezone.Asia_Tokyo] = "Asia/Tokyo",
        [Timezone.Asia_Seoul] = "Asia/Seoul",
        [Timezone.Australia_Perth] = "Australia/Perth",
        [Timezone.Australia_Sydney] = "Australia/Sydney",
        [Timezone.Australia_Melbourne] = "Australia/Melbourne",
        [Timezone.Pacific_Auckland] = "Pacific/Auckland",
        [Timezone.America_Sao_Paulo] = "America/Sao_Paulo",
        [Timezone.America_Mexico_City] = "America/Mexico_City",
    };

    private static readonly Dictionary<TimeFormat, string> Formats = new()
    {
        [TimeFormat.HHmm] = "HH:mm",
        [TimeFormat.HHmmss] = "HH:mm:ss",
        [TimeFormat.hmmtt] = "h:mm tt",
    };

    public CommandDescriptor Descriptor { get; } = new()
    {
        CommandName = "Chronos.Time",
        DisplayName = "Time in timezone",
        Group = "Chronos",
        ParameterTemplate = "({timezone},{label},{format})",
        Parameters =
        [
            new CommandParameter("timezone", typeof(Timezone)) { DefaultValue = nameof(Timezone.America_New_York) },
            new CommandParameter("label", typeof(string)) { DefaultValue = "" },
            new CommandParameter("format", typeof(TimeFormat)) { DefaultValue = nameof(TimeFormat.HHmm) },
        ],
    };

    public ButtonTargets SupportedTargets => ButtonTargets.TouchButton;
    public TimeSpan UpdateInterval => TimeSpan.FromSeconds(1);

    public string GetText(CommandContext ctx)
    {
        Timezone tz = Param.Enum(ctx.Parameters, 0, Timezone.America_New_York);
        string label = ctx.Parameters.Length > 1 ? ctx.Parameters[1] : "";
        TimeFormat fmt = Param.Enum(ctx.Parameters, 2, TimeFormat.HHmm);

        string tzId = IanaIds.GetValueOrDefault(tz, "UTC");
        string format = Formats.GetValueOrDefault(fmt, "HH:mm");

        TimeZoneInfo tzi;
        try
        {
            tzi = TimeZoneInfo.FindSystemTimeZoneById(tzId);
        }
        catch (Exception ex) when (ex is TimeZoneNotFoundException or InvalidTimeZoneException)
        {
            return "?";
        }

        string time = TimeZoneInfo.ConvertTime(DateTime.UtcNow, tzi).ToString(format);
        return string.IsNullOrWhiteSpace(label) ? time : $"{label}\n{time}";
    }

    public Task Execute(CommandContext ctx) => Task.CompletedTask;
}

/// <summary>Shows the current date.</summary>
internal sealed class DateCommand : IDisplayCommand
{
    private static readonly Dictionary<DateFormat, string> Formats = new()
    {
        [DateFormat.IsoDate] = "yyyy-MM-dd",
        [DateFormat.MonthDay] = "MMM d",
        [DateFormat.FullDate] = "MMM d, yyyy",
        [DateFormat.DayMonthYear] = "d MMM yyyy",
    };

    public CommandDescriptor Descriptor { get; } = new()
    {
        CommandName = "Chronos.Date",
        DisplayName = "Date",
        Group = "Chronos",
        ParameterTemplate = "({format})",
        Parameters =
        [
            new CommandParameter("format", typeof(DateFormat)) { DefaultValue = nameof(DateFormat.MonthDay) },
        ],
    };

    public ButtonTargets SupportedTargets => ButtonTargets.TouchButton;
    public TimeSpan UpdateInterval => TimeSpan.FromMinutes(1);

    public string GetText(CommandContext ctx)
    {
        DateFormat fmt = Param.Enum(ctx.Parameters, 0, DateFormat.MonthDay);
        string format = Formats.GetValueOrDefault(fmt, "MMM d");
        return DateTime.Now.ToString(format);
    }

    public Task Execute(CommandContext ctx) => Task.CompletedTask;
}

/// <summary>Shows the current day of the week.</summary>
internal sealed class DayOfWeekCommand : IDisplayCommand
{
    public CommandDescriptor Descriptor { get; } = new()
    {
        CommandName = "Chronos.DayOfWeek",
        DisplayName = "Day of week",
        Group = "Chronos",
        ParameterTemplate = "({format})",
        Parameters =
        [
            new CommandParameter("format", typeof(DayOfWeekFormat)) { DefaultValue = nameof(DayOfWeekFormat.Full) },
        ],
    };

    public ButtonTargets SupportedTargets => ButtonTargets.TouchButton;
    public TimeSpan UpdateInterval => TimeSpan.FromMinutes(1);

    public string GetText(CommandContext ctx)
    {
        DayOfWeekFormat fmt = Param.Enum(ctx.Parameters, 0, DayOfWeekFormat.Full);
        string format = fmt == DayOfWeekFormat.Full ? "dddd" : "ddd";
        return DateTime.Now.ToString(format);
    }

    public Task Execute(CommandContext ctx) => Task.CompletedTask;
}

/// <summary>Shows the ISO 8601 week number.</summary>
internal sealed class WeekCommand : IDisplayCommand
{
    public CommandDescriptor Descriptor { get; } = new()
    {
        CommandName = "Chronos.Week",
        DisplayName = "Week number",
        Group = "Chronos",
    };

    public ButtonTargets SupportedTargets => ButtonTargets.TouchButton;
    public TimeSpan UpdateInterval => TimeSpan.FromMinutes(1);

    public string GetText(CommandContext ctx)
    {
        int week = ISOWeek.GetWeekOfYear(DateTime.Now);
        return $"Week {week}";
    }

    public Task Execute(CommandContext ctx) => Task.CompletedTask;
}

/// <summary>Shows the day of the year (e.g. "Day 317 of 365").</summary>
internal sealed class DayOfYearCommand : IDisplayCommand
{
    public CommandDescriptor Descriptor { get; } = new()
    {
        CommandName = "Chronos.DayOfYear",
        DisplayName = "Day of year",
        Group = "Chronos",
    };

    public ButtonTargets SupportedTargets => ButtonTargets.TouchButton;
    public TimeSpan UpdateInterval => TimeSpan.FromMinutes(1);

    public string GetText(CommandContext ctx)
    {
        var now = DateTime.Now;
        int total = DateTime.IsLeapYear(now.Year) ? 366 : 365;
        return $"Day {now.DayOfYear} of {total}";
    }

    public Task Execute(CommandContext ctx) => Task.CompletedTask;
}

/// <summary>Counts the days until a recurring date (e.g. Christmas, a birthday).</summary>
internal sealed class CountdownCommand : IDisplayCommand
{
    public CommandDescriptor Descriptor { get; } = new()
    {
        CommandName = "Chronos.Countdown",
        DisplayName = "Days until date",
        Group = "Chronos",
        ParameterTemplate = "({month},{day},{label})",
        Parameters =
        [
            new CommandParameter("month", typeof(Month)) { DefaultValue = nameof(Month.December) },
            new CommandParameter("day", typeof(int)) { DefaultValue = "25" },
            new CommandParameter("label", typeof(string)) { DefaultValue = "" },
        ],
    };

    public ButtonTargets SupportedTargets => ButtonTargets.TouchButton;
    public TimeSpan UpdateInterval => TimeSpan.FromMinutes(1);

    public string GetText(CommandContext ctx)
    {
        Month month = Param.Enum(ctx.Parameters, 0, Month.December);
        int day = ctx.Parameters.Length > 1 && int.TryParse(ctx.Parameters[1], out int d) ? d : 25;
        string label = ctx.Parameters.Length > 2 ? ctx.Parameters[2] : "";

        var today = DateTime.Today;
        var target = NextOccurrence((int)month, day, today);
        int days = (target - today).Days;

        string text = days == 0 ? "Today!" : $"{days} days";
        return string.IsNullOrWhiteSpace(label) ? text : $"{label}\n{text}";
    }

    private static DateTime NextOccurrence(int month, int day, DateTime from)
    {
        int year = from.Year;
        for (int i = 0; i < 8; i++)
        {
            int maxDay = DateTime.DaysInMonth(year, month);
            int d = Math.Clamp(day, 1, maxDay);
            var candidate = new DateTime(year, month, d);
            if (candidate >= from)
                return candidate;
            year++;
        }
        return from;
    }

    public Task Execute(CommandContext ctx) => Task.CompletedTask;
}