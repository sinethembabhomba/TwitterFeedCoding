namespace TweetFeedsEntities
{
    public class User :BaseEntity
    {
        public string UserName { get; set; }
        public bool IsFollowing { get; set; }
    }
}
