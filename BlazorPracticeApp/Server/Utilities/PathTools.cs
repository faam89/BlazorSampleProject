using System.IO;


namespace BlazorPracticeApp.Server.Utilities
{
    public static class PathTools
    {
        public static readonly string PeopleImageAddress = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/People/");
        public static readonly string PeopleShowImageAddress = "/Images/People/";

        public static readonly string MoviePosterAddress = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Movies/");
        public static readonly string MoviePosterShowAddress =  "/Images/Movies/";
    }
}
