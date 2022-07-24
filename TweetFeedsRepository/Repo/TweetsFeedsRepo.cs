using TweetFeedsEntities;
using TweetFeedsRepository.Contracts;
namespace TweetFeedsRepository.Repo
{
    public class TweetsFeedsRepo : ITweetsFeedsRepo
    {
        #region Path

        readonly string _path = @"C:\MockTweetData\";
        #endregion

        #region Tweets Action

        public TwitterFeeds GetUsersOrUserAndTheirTweet(string filename)
        {
            List<string> list = new();

            TwitterFeeds UserNametweets = new();
       
            try
            {
                using (StreamReader sr = new(_path+filename+".txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                }

                foreach (var users in list)
                {
                    UserNametweets.User.Add(new User() { UserName = users });
                   
                }

                return UserNametweets;
            }
            catch (Exception ex)
            {
                return _ = new TwitterFeeds() 
                { 
                    ErrorMessages = new ErrorMessages() 
                    { 
                        Error = ex.Message 
                    } 
                };
            }
        }

        public TwitterFeeds GetUsersAndTheirListOfTweetsAndTheirFollowingTweets(string user, string tweet)
        {
         
            TwitterFeeds UserNametweets = new();
            List<User> users = GetMyAccountHolders(user).OrderBy(a => a.UserName).ToList();
            List<Followers> tetMyFollowers = GetMyFollowers(user).Followers.OrderBy(a => a.AccountHolder).ToList();
            List<Tweets> getMyTweets = GetMyTweets(tweet).Tweets.ToList();
            List<string> MyTweetsSummary = new();


            try
            {
           
                foreach (var activeUser in users)
                {
                    if(getMyTweets.Any(a=> a.TweetOwner.Equals(activeUser.UserName)))
                    {
                        foreach(var mytweet in getMyTweets)
                        {
                            if(mytweet.TweetOwner == activeUser.UserName)
                            {
                                foreach (var dispalyTweet in mytweet.Tweet)
                                {
                                    MyTweetsSummary.Add("@" + activeUser.UserName + " : " + dispalyTweet);
                                }
                            }
                            else
                            {
                                foreach(var myFollowing in tetMyFollowers)
                                {
                                    if(myFollowing.AccountHolder == activeUser.UserName)
                                    {
                                        if (myFollowing.MyFollowers.Any(a => a.IsFollowing == true))
                                        {
                                            foreach(var confirmMyFollowing in myFollowing.MyFollowers)
                                            {
                                                foreach (var dispalyTweet in mytweet.Tweet)
                                                {
                                                    if (mytweet.TweetOwner == confirmMyFollowing.UserName)
                                                    {
                                                        var tweeToDisplay = "@" + mytweet.TweetOwner + " : " + dispalyTweet;

                                                        if (!MyTweetsSummary.Contains(tweeToDisplay)) 
                                                        {
                                                            MyTweetsSummary.Add(tweeToDisplay); 
                                                        }
                                                        
                                                    }
                                                }
                                            }
                                            
                                        }
                                    }
                                   
                                }
                            }
                        }
                    }

                    UserNametweets.Tweets.Add(new Tweets()
                    {
                        TweetOwner = activeUser.UserName,
                        Tweet = MyTweetsSummary
                    });

                    MyTweetsSummary = new();
                }

                return UserNametweets;
            }
            catch (Exception ex)
            {
                return _ = new TwitterFeeds()
                {
                    ErrorMessages = new ErrorMessages()
                    {
                        Error = ex.Message
                    }
                };
            }
        }
        #endregion

        #region helper Methods

        private List<User> GetMyAccountHolders(string user)
        {
            List<User> UserNametweets = new();
            List<string> Tweets = new();
            var cmbinePath = _path+user+".txt";

            using (StreamReader sr = new(cmbinePath.Replace(" ", "")))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] words = line.Split(' ', ',');

                    for (int i = 0; i < words.Length; i++)
                    {
                        if (words[i] != "follows")
                        {
                            if (!Tweets.Contains(words[i]))
                            {
                                Tweets.Add(words[i]);
                            }
                        }
                    }
                }
            }

            foreach (var item in Tweets)
            {
                if (item != "")
                {
                    UserNametweets.Add(new User() { UserName = item });
                }
            }

            return UserNametweets;
        }

        private TwitterFeeds GetMyFollowers(string user)
        {
            TwitterFeeds UserNametweets = new();
            var cmbinePath = _path + user + ".txt";

            using (StreamReader sr = new(cmbinePath.Replace(" ", "")))
            {
                string line;
                string accountHolder = "";
                var MyFollowers = new List<User>();

                while ((line = sr.ReadLine()) != null)
                {
                    string[] words = line.Split(' ', ',');

                    for (int i = 0; i < words.Length; i++)
                    {

                        if (words[i] != "follows" && i == 0)
                        {

                            accountHolder = words[i];
                        }
                        else if (words[i] != "follows" && i > 0)
                        {
                            if (words[i] != "")
                            {
                                MyFollowers.Add(new User() { UserName = words[i], IsFollowing = true });
                            }
                        }
                    }

                    UserNametweets.Followers.Add(new Followers()
                    {
                        AccountHolder = accountHolder,
                        MyFollowers = MyFollowers
                    });

                    MyFollowers = new List<User>();

                }
            }
            return UserNametweets;
        }

        private TwitterFeeds GetMyTweets(string tweet)
        {
            TwitterFeeds UserNametweets = new();

            var cmbinePath = _path+tweet+".txt";
           
            using (StreamReader sr = new(cmbinePath.Replace(" ","")))
            {
                string line;
                string tweetOwner = "";

                var MyFollowers = new List<string>();

                while ((line = sr.ReadLine()) != null)
                {
                    string[] words = line.Split("> ");

                    for (int i = 0; i < words.Length; i++)
                    {

                        if (i == 0)
                        {
                            tweetOwner = words[i];
                        }
                        else if (i > 0)
                        {
                            MyFollowers.Add(words[i]);
                        }
                    }

                    UserNametweets.Tweets.Add(new Tweets()
                    {
                        TweetOwner = tweetOwner,
                        Tweet = MyFollowers
                    });

                    MyFollowers = new List<string>();

                }
            }
            return UserNametweets;
        }
        #endregion
    }
}