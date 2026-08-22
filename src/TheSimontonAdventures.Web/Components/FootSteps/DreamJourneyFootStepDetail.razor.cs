using Microsoft.AspNetCore.Components;
using TheSimontonAdventures.Web.Planning;

namespace TheSimontonAdventures.Web.Components;

/// <summary>Explores one authorized Journey FootStep inside Dream before Planner customization.</summary>
public partial class DreamJourneyFootStepDetail
{
    /// <summary>Gets or sets the exact immutable Journey FootStep version.</summary>
    [Parameter, EditorRequired]
    public AdventureTemplateBlueprint Template { get; set; } = null!;

    /// <summary>Gets or sets the Dream catalog return path.</summary>
    [Parameter]
    public string DreamPath { get; set; } = "/workspace";

    /// <summary>Gets or sets the Planner path that owns private Journey configuration.</summary>
    [Parameter]
    public string PlannerPath { get; set; } = "/workspace";

    private string CustomizePath =>
        $"{PlannerPath}?journeyFootStep={Uri.EscapeDataString(Template.VersionId.TemplateId)}";

    private IReadOnlyList<AdventureTemplateDay> PreviewDays => Template.Days
        .OrderBy(day => day.DayOffset)
        .Take(6)
        .ToArray();

    private string TravelStyle => Template.Transportation.Count == 0
        ? "Flexible"
        : string.Join(" + ", Template.Transportation
            .Select(segment => segment.Mode)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(3));

    private IReadOnlyList<AdventureTemplateActivity> ActivitiesFor(string dayKey) =>
        Template.Activities.Where(activity => string.Equals(
            activity.DayKey, dayKey, StringComparison.Ordinal)).ToArray();

    private int DestinationNumber(string destinationKey) =>
        Template.Destinations
            .Select((destination, index) => new { destination.Key, Number = index + 1 })
            .First(item => string.Equals(item.Key, destinationKey, StringComparison.Ordinal))
            .Number;

    private static string Monogram(string name) => string.Concat(
        name.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Take(2)
            .Select(word => char.ToUpperInvariant(word[0])));
}
