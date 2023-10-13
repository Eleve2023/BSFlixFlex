using BSFlixFlex.Client.Shareds.Components.Pagination;
using BSFlixFlex.Client.Shareds.Interfaces;
using BSFlixFlex.Client.Shareds.Models;
using Microsoft.AspNetCore.Components;

namespace BSFlixFlex.Components.Shared
{
    public abstract class Discover<T>(Cinematography cinematography) : ComponentBase where T : class
    {
        private readonly Cinematography cinematography = cinematography;
        protected GridPagingState pagingState = new(10);
        protected bool loadTopRated = true;
        protected bool loadItems = true;
        [Inject] public required IApiTMBDService ApiTMBDService { get; set; }
        public List<T>? TopRated { get; set; }
        public List<T>? Items { get; set; }
        public string? Search { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await FillTopRated();
            await FillItemsAsync(cinematography, 1, pagingState.ItemsPerPage);

            pagingState.PageChanged += PagingState_PageChanged;

            await base.OnInitializedAsync();
        }

        private async Task FillTopRated()
        {
            loadTopRated = true;
            StateHasChanged();
            var listResponse = await ApiTMBDService.FetchTopRatedItemsAsync<T>(cinematography, 1, 5);
            TopRated = listResponse.Items;
            //await Task.Delay(5000);
            loadTopRated = false;
            StateHasChanged();
        }

        private async Task FillItemsAsync(Cinematography cinematography, int clientPageNumber, int clientPageSize = 10)
        {
            loadItems = true;
            StateHasChanged();
            ApiListResponse<T> listResponse;
            if (string.IsNullOrEmpty(Search))
                listResponse = await ApiTMBDService.FetchDiscoveryItemsAsync<T>(cinematography, clientPageNumber, clientPageSize);
            else
                listResponse = await ApiTMBDService.SearchItemsAsync<T>(cinematography, Search, clientPageNumber, clientPageSize);

            pagingState.TotalItems = listResponse.TotalItems;
            Items = listResponse.Items;
            //await Task.Delay(5000);
            loadItems = false;
            StateHasChanged();
        }

        private async void PagingState_PageChanged(object? sender, GridPageChangedEventArgs e)
        {
            await FillItemsAsync(cinematography, pagingState.CurrentPage, e.ItemsPerPage);
            // StateHasChanged();
        }



        protected void OnSearch()
        {
            Items = null;
            pagingState.CurrentPage = 1;
        }
    }
}

