namespace TweetFeedsEntities
{
    public class TwitterFeeds
    {
        public TwitterFeeds()
        {
            Tweets = new List<Tweets>();
            User = new List<User>();
            Followers = new List<Followers>();
        }
        public List<User> User { get; set; }
        public List<Tweets> Tweets { get; set; }
        public ErrorMessages ErrorMessages { get; set; }
        public string Results { get; set; }
        public List<Followers> Followers { get; set; }
    }
}
