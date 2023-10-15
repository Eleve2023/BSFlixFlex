namespace BSFlixFlex.Client.Shareds
{
    public static class SharedGlobalConstants
    {
        public const string API_PATH_MOVIE = "api/movie";
        public const string API_PATH_MOVIE_ID = API_PATH_MOVIE + "/{id:int}";
        public const string API_PATH_MOVIE_TOP_RATED = API_PATH_MOVIE + "/top_rated";
        public const string API_PATH_TVSHOW = "api/tvshow";
        public const string API_PATH_TVSHOW_ID = API_PATH_TVSHOW + "/{id:int}";
        public const string API_PATH_TVSHOW_TOP_RATED = API_PATH_TVSHOW+ "/top_rated";
        public const string API_PATH_SEARCH = "api/search";
        public const string API_PATH_SEARCH_MOVIE = API_PATH_SEARCH + "/movie";
        public const string API_PATH_SEARCH_TVSHOW = API_PATH_SEARCH + "/tvshow";
        public const string API_PATH_VIDEO = "api/videos";
        public const string API_PATH_VIDEO_MOVIE = API_PATH_VIDEO + "/movie";
        public const string API_PATH_VIDEO_MOVIE_ID = API_PATH_VIDEO_MOVIE + "/{id:int}";
        public const string API_PATH_VIDEO_TVSHOW = API_PATH_VIDEO+ "/tvshow";
        public const string API_PATH_VIDEO_TVSHOW_ID = API_PATH_VIDEO_TVSHOW + "/{id:int}";
    }
}
