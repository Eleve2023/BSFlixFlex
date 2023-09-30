using BSFlixFlex.Models;
using BSFlixFlex.Pages.Pagination;
using BSFlixFlex.Services;
using Microsoft.AspNetCore.Components;

namespace BSFlixFlex.Pages
{
    public abstract class Discover<T> : ComponentBase where T : class
    {
        private readonly Cinematography cinematography;
        protected GridPagingState pagingState;
        private int currentPage;
        private List<T>? waitingList;
        private string? _search;
        public Discover(Cinematography cinematography)
        {
            pagingState = new GridPagingState(10);
            this.cinematography = cinematography;
        }


        [Inject] public required HttpClient HttpClient { get; set; }
        [Inject] public required ApiTMBDService ApiTMBDService { get; set; }
        public List<T>? TopRated { get; set; }
        public List<T>? Items { get; set; }
        public string? Search { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var listResponse = await ApiTMBDService.GetTopRateAsync<T>(cinematography, 1, 5);
            TopRated = listResponse.Items;
            var listResponse2 = await ApiTMBDService.GetDiscoverAsync<T>(cinematography, 1, pagingState.ItemsPerPage);
            if (listResponse2.TotalItems > 10000)
                pagingState.TotalItems = 10000;
            else
                pagingState.TotalItems = listResponse2.TotalItems;
            Items = listResponse2.Items;
            //var resultListMovie = await HttpClient.GetFromJsonAsync<DiscoverResponse<T>>($"3/{cinematography.ToString().ToLower()}/top_rated?language=fr-Fr");
            //TopRated = resultListMovie.Results.Take(5).ToList();
            pagingState.PageChanged += PagingState_PageChanged;
            //pagingState.CurrentPage = 1;
            await base.OnInitializedAsync();
        }
        private async void PagingState_PageChanged(object? sender, GridPageChangedEventArgs e)
        {
            ListResponse<T> listResponse;
            if (string.IsNullOrEmpty(Search))
                 listResponse = await ApiTMBDService.GetDiscoverAsync<T>(cinematography, pagingState.CurrentPage, e.ItemsPerPage);
            else
                 listResponse = await ApiTMBDService.GetSearchAsync<T>(cinematography,Search, pagingState.CurrentPage, e.ItemsPerPage);

            if (listResponse.TotalItems > 10000)
                pagingState.TotalItems = 10000;
            else
                pagingState.TotalItems = listResponse.TotalItems;
            Items = listResponse.Items;
            
            StateHasChanged();
        }
        protected void OnSearch()
        {
            waitingList = null;
            _search = Search;
            pagingState.CurrentPage = 1;
        }
    }
}

