using BSFlixFlex.Models;
using BSFlixFlex.Services;
using Microsoft.AspNetCore.Components;

namespace BSFlixFlex.Pages
{
    public partial class Details<T> : ComponentBase where T : IDiscovryCommonProperty
    {
        private readonly Cinematography cinematography;
        protected List<Video>? videoResults;
        protected bool isFavori;
        protected RenderFragment btmFavori;

        public Details(Cinematography cinematography)
        {
            this.cinematography = cinematography;
            btmFavori = BtmFavoriRender;
        }

        //[Inject] public required HttpClient HttpClient { get; set; }
        //[Inject] public required MyFavoriteService MyFavoriService { get; set; }
        [Inject] public required IApiTMBDService ApiTMBDService { get; set;}
        [SupplyParameterFromQuery][Parameter] public required int Id { get; set; }
        public required T Item { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var resultDetail = await ApiTMBDService.FetchItemDetailsAsync<T>(cinematography, Id);
            if (resultDetail.IsSuccess && resultDetail.Item != null)
                Item = resultDetail.Item;
            
            var resultvideos = await ApiTMBDService.FetchItemVideosAsync<T>(cinematography, Id);
            if(resultvideos.IsSuccess && resultvideos.Results != null)
                videoResults = resultvideos.Results!;
            await base.OnInitializedAsync();
        }
    }
}
