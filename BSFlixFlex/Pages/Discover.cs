using BSFlixFlex.Models;
using BSFlixFlex.Pages.Pagination;
using BSFlixFlex.Services;
using Microsoft.AspNetCore.Components;

namespace BSFlixFlex.Pages
{
    public abstract class Discover<T>(Cinematography cinematography) : ComponentBase where T : class
    {
        private readonly Cinematography cinematography = cinematography;
        protected GridPagingState pagingState = new(10);

        [Inject] public required IApiTMBDService ApiTMBDService { get; set; }
        public List<T>? TopRated { get; set; }
        public List<T>? Items { get; set; }
        public string? Search { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var listResponse = await ApiTMBDService.FetchTopRatedItemsAsync<T>(cinematography, 1, 5);
            TopRated = listResponse.Items;
            await FillItemsAsync(cinematography, 1, pagingState.ItemsPerPage);

            pagingState.PageChanged += PagingState_PageChanged;

            await base.OnInitializedAsync();
        }
        private async void PagingState_PageChanged(object? sender, GridPageChangedEventArgs e)
        {
            await FillItemsAsync(cinematography, pagingState.CurrentPage, e.ItemsPerPage);

            StateHasChanged();
        }

        private async Task FillItemsAsync(Cinematography cinematography, int clientPageNumber, int clientPageSize = 10)
        {
            ApiListResponse<T> listResponse;
            if (string.IsNullOrEmpty(Search))
                listResponse = await ApiTMBDService.FetchDiscoveryItemsAsync<T>(cinematography, clientPageNumber, clientPageSize);
            else
                listResponse = await ApiTMBDService.SearchItemsAsync<T>(cinematography, Search, clientPageNumber, clientPageSize);

            pagingState.TotalItems = listResponse.TotalItems;
            Items = listResponse.Items;
        }

        protected void OnSearch()
        {
            Items = null;
            pagingState.CurrentPage = 1;
        }
    }
}

