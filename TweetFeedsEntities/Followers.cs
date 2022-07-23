namespace TweetFeedsEntities
{
    public class Followers : BaseEntity
    {
        public Followers()
        {
            MyFollowers = new List<User>();
        }
        public string AccountHolder { get; set; }
        public List<User> MyFollowers { get; set; }
    }
}
