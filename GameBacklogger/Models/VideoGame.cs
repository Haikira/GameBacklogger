namespace GameBackloggerApi.Models
{
    public class VideoGame
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int HowLongToBeat { get; set; }
    }
}
