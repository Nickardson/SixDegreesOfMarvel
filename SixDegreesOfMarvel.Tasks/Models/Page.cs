namespace SixDegreesOfMarvel.Tasks.Models
{
    public class Page
    {
        public string Title { get; set; }

        public static string NormalizeTitle(string title)
        {
            return title.Replace('_', ' ');
        }
    }
}