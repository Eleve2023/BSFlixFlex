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

        [Inject] public required HttpClient HttpClient { get; set; }
        [Inject] public MyFavoriService MyFavoriService { get; set; }
        [Inject] public ApiTMBDService ApiTMBDService { get; set;}
        [SupplyParameterFromQuery][Parameter] public required int Id { get; set; }
        public required T Item { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //Item = await HttpClient.GetFromJsonAsync<T>($"3/{cinematography.ToString().ToLower()}/{Id}?language=fr-Fr");
            var resultDetail = await ApiTMBDService.GetDetail<T>(cinematography, Id);
            if (resultDetail.IsSuccess && resultDetail.Item != null)
                Item = resultDetail.Item;
            
            //var videoResponse = await HttpClient.GetFromJsonAsync<VideoResponse>($"3/{cinematography.ToString().ToLower()}/{Id}/videos?language=fr-Fr");
            //videoResults = videoResponse.Results;
            var resultvideos = await ApiTMBDService.GetVideos<T>(cinematography, Id);
            if(resultvideos.IsSuccess && resultvideos.Results != null)
                videoResults = resultvideos.Results!;
            
            if (Item != null && Item.Id is int id)
                isFavori = await MyFavoriService.IsFavoriAsync(id, cinematography);
            await base.OnInitializedAsync();
        }

        private void AddFavori()
        {
            MyFavoriService.AddFavori(Id, cinematography);
            isFavori = true;
        }
        private void RemoveFavori()
        {
            MyFavoriService.Remove(Id, cinematography);
            isFavori = false;
        }

    }
}
