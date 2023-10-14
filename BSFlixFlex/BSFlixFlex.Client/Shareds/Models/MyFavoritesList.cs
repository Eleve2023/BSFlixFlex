using System.Text.Json.Serialization;

namespace BSFlixFlex.Client.Shareds.Models
{
    
    public class MyFavoriteItem
    {       
        [JsonInclude]
        public MovieDetails? MovieDetail { get; private set; }
        [JsonInclude]
        public TvShowDetails? TvShowDetail { get; private set; }

        //public MyFavoriteItem() { }
        [JsonConstructor]
        public MyFavoriteItem(IDiscovryCommonProperty discovryCommonProperty)
        {
            if (discovryCommonProperty is MovieDetails movieDetails)
            {
                MovieDetail = movieDetails;
            }
            else if (discovryCommonProperty is TvShowDetails tvShowDetails)
            {
                TvShowDetail = tvShowDetails;
            }
            else
                throw new NotImplementedException();
        }

        //public MyFavoriteItem(MovieDetails movieDetails)
        //{
        //    MovieDetail = movieDetails;
        //}
        
        //public MyFavoriteItem(TvShowDetails tvShowDetails)
        //{
        //    TvShowDetail = tvShowDetails;
        //}
        [JsonIgnore]
        public IDiscovryCommonProperty MyFavorite
        {
            get
            {
                return MovieDetail is not null ? MovieDetail: TvShowDetail!;
            }
        }
    }

}
