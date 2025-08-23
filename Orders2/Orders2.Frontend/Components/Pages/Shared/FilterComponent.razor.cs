using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Orders2.Frontend.Components.Pages.Shared;

public partial class FilterComponent
{
    [Parameter] public string FilterValue { get; set; } = string.Empty;
    [Parameter] public EventCallback<string> ApplyFilter { get; set; } 
 
    private async Task CleanFilter()
    {
        FilterValue = string.Empty;
        await ApplyFilter.InvokeAsync(FilterValue);
    }

    private async Task OnFilterApply()
    {
        await ApplyFilter.InvokeAsync(FilterValue);
    }
}