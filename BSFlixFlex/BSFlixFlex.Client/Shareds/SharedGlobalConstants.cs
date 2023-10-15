namespace BSFlixFlex.Client.Shareds
{
    public static class SharedGlobalConstants
    {
        public const string API_GROUP_CINEMATOGRAPHY = "api/cinematography";
        public const string API_GROUP_DISCOVERY = "discovery";
        public const string API_GROUP_SEARCH = "search";
        public const string API_GROUP_DETAIL = "detail";
        public const string API_GROUP_VIDEO = "video";
        public const string API_RELATIVE_PATH_MOVIE = "movie";
        public const string API_RELATIVE_PATH_TVSHOW = "tvshow";
        public const string API_RELATIVE_PATH_TOPRATED = "top_rated";
        public const string API_RELATIVE_PATH_MOVIE_TOPRATED = $"{API_RELATIVE_PATH_MOVIE}/{API_RELATIVE_PATH_TOPRATED}";
        public const string API_RELATIVE_PATH_TVSHOW_TOPRATED = $"{API_RELATIVE_PATH_TVSHOW}/{API_RELATIVE_PATH_TOPRATED}";

        //discover
        public const string API_PATH_MOVIE = $"{API_GROUP_CINEMATOGRAPHY}/{API_GROUP_DISCOVERY}/{API_RELATIVE_PATH_MOVIE}";
        public const string API_PATH_TVSHOW = $"{API_GROUP_CINEMATOGRAPHY}/{API_GROUP_DISCOVERY}/{API_RELATIVE_PATH_TVSHOW}";
        public const string API_PATH_MOVIE_TOP_RATED = $"{API_GROUP_CINEMATOGRAPHY}/{API_GROUP_DISCOVERY}/{API_RELATIVE_PATH_MOVIE_TOPRATED}";
        public const string API_PATH_TVSHOW_TOP_RATED = $"{API_GROUP_CINEMATOGRAPHY}/{API_GROUP_DISCOVERY}/{API_RELATIVE_PATH_TVSHOW_TOPRATED}";

        //detail
        public const string API_PATH_MOVIE_DETAIL = $"{API_GROUP_CINEMATOGRAPHY}/{API_GROUP_DETAIL}/{API_RELATIVE_PATH_MOVIE}";
        public const string API_PATH_TVSHOW_DETAIL = $"{API_GROUP_CINEMATOGRAPHY}/{API_GROUP_DETAIL}/{API_RELATIVE_PATH_TVSHOW}";

        //search
        public const string API_PATH_SEARCH_MOVIE = $"{API_GROUP_CINEMATOGRAPHY}/{API_GROUP_SEARCH}/{API_RELATIVE_PATH_MOVIE}";
        public const string API_PATH_SEARCH_TVSHOW = $"{API_GROUP_CINEMATOGRAPHY}/{API_GROUP_SEARCH}/{API_RELATIVE_PATH_TVSHOW}";

        //video
        public const string API_PATH_VIDEO_MOVIE = $"{API_GROUP_CINEMATOGRAPHY}/{API_GROUP_VIDEO}/{API_RELATIVE_PATH_MOVIE}";
        public const string API_PATH_VIDEO_TVSHOW = $"{API_GROUP_CINEMATOGRAPHY}/{API_GROUP_VIDEO}/{API_RELATIVE_PATH_TVSHOW}";
    }
}
