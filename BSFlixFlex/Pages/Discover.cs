using BSFlixFlex.Models;
using BSFlixFlex.Pages.Pagination;
using Microsoft.AspNetCore.Components;

namespace BSFlixFlex.Pages
{
    public abstract class Discover<T> : ComponentBase where T : class
    {
        private readonly string cinematography;
        protected GridPagingState pagingState;
        private int currentPage;
        private List<T>? waitingList;
        private string? _search;
        public Discover(Cinematography cinematography)
        {
            pagingState = new GridPagingState(10);
            this.cinematography = cinematography.ToString().ToLower();
        }


        [Inject] public required HttpClient HttpClient { get; set; }
        public List<T>? Popular { get; set; }
        public List<T>? Items { get; set; }
        public string? Search { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var resultListMovie = await HttpClient.GetFromJsonAsync<DiscoverResponse<T>>($"3/{cinematography}/popular?language=fr-Fr");
            Popular = resultListMovie.Results.Take(5).ToList();
            pagingState.PageChanged += PagingState_PageChanged;
            pagingState.CurrentPage = 1;
            await base.OnInitializedAsync();
        }
        private void PagingState_PageChanged(object? sender, GridPageChangedEventArgs e)
        {
            if (waitingList != null && waitingList.Count > 10 && pagingState.CurrentPage == currentPage + 1 && currentPage % 2 != 0)
            {
                Items = waitingList.Skip(10).ToList();
                currentPage = pagingState.CurrentPage;
            }
            else
            {
                try
                {
                    DiscoverResponse<T>? resultListMovie;
                    if (string.IsNullOrEmpty(Search))
                    {
                        resultListMovie = HttpClient.GetFromJsonAsync<DiscoverResponse<T>>($"4/discover/{cinematography}?page={(pagingState.CurrentPage + 1) / 2}&language=fr-Fr&sort_by=primary_release_date.desc&with_status=Planned").Result;

                    }
                    else
                    {
                        resultListMovie = HttpClient.GetFromJsonAsync<DiscoverResponse<T>>($"3/search/{cinematography}?query={_search}&page={(pagingState.CurrentPage + 1) / 2}&language=fr-Fr").Result;
                    }
                    pagingState.TotalItems = resultListMovie.TotalResults;
                    //suit a problem sur Api qui limite a 500 page
                    if (resultListMovie.TotalResults > 500)
                        pagingState.TotalItems = 10000;

                    currentPage = pagingState.CurrentPage;
                    waitingList = resultListMovie.Results.ToList();
                    if (currentPage % 2 != 0)
                        Items = waitingList.Take(10).ToList();
                    else
                        Items = waitingList.Skip(10).ToList();
                }
                catch (Exception)
                {

                }
            }
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

