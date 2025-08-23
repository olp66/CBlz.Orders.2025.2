using Microsoft.AspNetCore.Components;
using MudBlazor;
using Orders2.Frontend.Components.Pages.Cities;
using Orders2.Frontend.Components.Pages.Shared;
using Orders2.Frontend.Repositories;
using Orders2.Shared.Entities;
using System.Net;

namespace Orders2.Frontend.Components.Pages.States;

public partial class StateDetails
{
    private State? state;
    private List<City>? cities;

    private MudTable<City> table = new();
    private readonly int[] pageSizeOptions = { 10, 25, 50, 5, int.MaxValue };
    private int totalRecords = 0;
    private bool loading;
    private const string baseUrl = "api/cities";
    private string infoFormat = "{first_item}-{last_item} de {all_items}";

    [Parameter] public int StateId { get; set; }

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty; 

    protected override async Task OnInitializedAsync()
    {
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        await LoadTotalRecordsAsync();
    }

    private async Task<bool> LoadStateAsync()
    {
        var responseHttp = await Repository.GetAsync<State>($"/api/states/{StateId}");
        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("/countries");
                return false;
            }

            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(message!, Severity.Error);
            return false;
        }
        state = responseHttp.Response;
        return true;
    }

    private async Task<bool> LoadTotalRecordsAsync()
    {
        loading = true;
        if (state is null)
        {
            var ok = await LoadStateAsync();
            if (!ok)
            {
                NoState();
                return false;
            }
        }

        var url = $"{baseUrl}/totalRecords?id={StateId}";

        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"&filter={Filter}";
        }
        var responseHttp = await Repository.GetAsync<int>(url);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(message!, Severity.Error);
            return false;
        }
        totalRecords = responseHttp.Response;
        loading = false;
        return true;
    }

    private async Task<TableData<City>> LoadListAsync(TableState state, CancellationToken cancellationToken)
    {
        int page = state.Page + 1;
        int pageSize = state.PageSize;
        var url = $"{baseUrl}/paginated?id={StateId}&page={page}&recordsnumber={pageSize}";

        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"&filter={Filter}";
        }

        var responseHttp = await Repository.GetAsync<List<City>>(url);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(message!, Severity.Error);
            return new TableData<City> { Items = [], TotalItems = 0 };
        }
        if (responseHttp.Response == null)
        {
            return new TableData<City> { Items = [], TotalItems = 0 };
        }
        return new TableData<City>
        {
            Items = responseHttp.Response,
            TotalItems = totalRecords
        };
    }

    private async Task SetFilterValue(string value)
    {
        Filter = value;
        await LoadAsync();
        await table.ReloadServerData();
    }

    private void ReturnAction()
    {
        NavigationManager.NavigateTo($"/countries/details/{state?.CountryId}");
    }

    private async Task ShowModalAsync(int id = 0, bool isEdit = false)
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            CloseButton = true
        };
        IDialogReference? dialog;
        if (isEdit)
        {
            var parameters = new DialogParameters
            {
                { "Id", id }
            }; dialog = await DialogService.ShowAsync<CityEdit>("Editar Ciudad", parameters, options);
        }
        else
        {
            var parameters = new DialogParameters
                {
                    { "StateId", StateId }
                };
            dialog = await DialogService.ShowAsync<CityCreate>("Nuevo Ciudad", parameters, options);
        }

        var result = await dialog.Result;
        if (result!.Canceled!)
        {
            await LoadTotalRecordsAsync();
            await table.ReloadServerData();
        }
    }

    private void NoState()
    {
        NavigationManager.NavigateTo("/countries");
    }

    private async Task DeleteAsync(City city)
    {
        var parameters = new DialogParameters
            {
                { "Message", $"�Est�s seguro de que quieres eliminar la ciudad {city.Name}?" }
            };
        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.ExtraSmall,
            CloseOnEscapeKey =
true
        };
        var dialog = await DialogService.ShowAsync<ConfirmDialog>("Confirmaci�n", parameters, options);
        var result = await dialog.Result;
        if (result!.Canceled)
        {
            return;
        }

        var responseHttp = await Repository.DeleteAsync($"api/cities/{city.Id}");
        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(message!, Severity.Error);
                return;
            }
            else
            {
                var message = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(message!, Severity.Error);
            }
            return;
        }
        await LoadAsync();
        await table.ReloadServerData();
        Snackbar.Add("Ciudad eliminada.", Severity.Success);
    }
}