using BSFlixFlex.Client.Shareds.Interfaces;
using BSFlixFlex.Client.Shareds.Models;
using Microsoft.AspNetCore.Components;

namespace BSFlixFlex.Client.Shareds.Components.Shared
{
    public partial class Details<T> : ComponentBase where T : IDiscovryCommonProperty
    {
        private readonly Cinematography cinematography;
        protected List<Video>? videoResults;
        protected bool isFavori;
        protected RenderFragment btmFavori;
        protected bool load;
        public Details(Cinematography cinematography)
        {
            this.cinematography = cinematography;
            btmFavori = BtmFavoriRender;
        }

        //[Inject] public required HttpClient HttpClient { get; set; }
        //[Inject] public required MyFavoriteService MyFavoriService { get; set; }
        [Inject] public required IApiTMBDService ApiTMBDService { get; set; }
        [SupplyParameterFromQuery][Parameter] public required int Id { get; set; }
        public required T Item { get; set; }

        protected override async Task OnInitializedAsync()
        {

            var resultDetail = await ApiTMBDService.FetchItemDetailsAsync<T>(cinematography, Id);
            if (resultDetail.IsSuccess && resultDetail.Item != null)
                Item = resultDetail.Item;

            var resultvideos = await ApiTMBDService.FetchItemVideosAsync<T>(cinematography, Id);
            if (resultvideos.IsSuccess && resultvideos.Results != null)
                videoResults = resultvideos.Results!;
            await base.OnInitializedAsync();
        }
    }
}
