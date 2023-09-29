using BSFlixFlex.Models;
using Microsoft.AspNetCore.Components;

namespace BSFlixFlex.Pages
{
    public abstract class Details<T> : ComponentBase where T : class
    {
        private readonly string cinematography;
        protected List<Video>? videoResults;

        public Details(Cinematography cinematography)
        {
            this.cinematography = cinematography.ToString().ToLower();
        }

        [Inject] public required HttpClient HttpClient { get; set; }
        [SupplyParameterFromQuery][Parameter] public required int Id { get; set; }
        public required T Item { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Item = await HttpClient.GetFromJsonAsync<T>($"3/{cinematography}/{Id}?language=fr-Fr");
            var videoResponse = await HttpClient.GetFromJsonAsync<VideoResponse>($"3/{cinematography}/{Id}/videos?language=fr-Fr");
            videoResults = videoResponse.Results;
            await base.OnInitializedAsync();
        }
        
    }
}
