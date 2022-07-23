using TweetFeedsEntities;
namespace TweetFeedsRepository.Contracts
{
    public interface ITweetsFeedsRepo
    {
        TwitterFeeds GetUsersOrUserAndTheirTweet(string tweet);
        TwitterFeeds GetUsersAndTheirListOfTweetsAndTheirFollowingTweets(string user, string tweet);
    }
}
