namespace TweetFeedsEntities
{
    public class Tweets : BaseEntity
    {
      public string TweetOwner { get; set; }
      public List<string> Tweet { get; set; }
    }
}
