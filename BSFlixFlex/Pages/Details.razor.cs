﻿using BSFlixFlex.Models;
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
            var resultDetail = await ApiTMBDService.GetDetail<T>(cinematography, Id);
            if (resultDetail.IsSuccess && resultDetail.Item != null)
                Item = resultDetail.Item;
            
            var resultvideos = await ApiTMBDService.GetVideos<T>(cinematography, Id);
            if(resultvideos.IsSuccess && resultvideos.Results != null)
                videoResults = resultvideos.Results!;
            
            if (Item != null && Item.Id is int id)
                isFavori = await MyFavoriService.IsFavoriAsync(id, cinematography);
            await base.OnInitializedAsync();
        }

        private async void AddFavori()
        {
            await MyFavoriService.AddFavoriAsync(Id, cinematography);
            isFavori = true;
        }
        private async void RemoveFavori()
        {
            await MyFavoriService.RemoveAsync(Id, cinematography);
            isFavori = false;
        }

    }
}
