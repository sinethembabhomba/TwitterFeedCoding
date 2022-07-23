using System;
using System.Collections.Generic;
using System.IO;
using TweetFeedsRepository.Contracts;
using TweetFeedsRepository.Repo;
using Xunit;

namespace TwitterFeedCodingUnitTest
{
    public class TwitterFeedUnitTest : IDisposable
    {
        readonly string tweet = @"C:\datatxt\tweet.tx";
        readonly string user = @"C:\datatxt\user.tx";
        private  ITweetsFeedsRepo _tweetsFeedsRepo = new TweetsFeedsRepo();
        List<string> actuallyResults = new();
        List<string> expected = new();
        List<string> expectedTweet = new();

        // setup
        public TwitterFeedUnitTest()
        {
          
            // Create a new file     
            using (StreamWriter sw = File.CreateText(tweet))
            {
                sw.WriteLine("Alan> If you have a procedure with 10 parameters, you probably missed some.");
                sw.WriteLine("Ward> There are only two hard things in Computer Science: cache invalidation, naming things and off-by-1 errors.");
                sw.WriteLine("Alan> Random numbers should not be generated with a method chosen at random.");
            }

            // Create a new file     
            using (StreamWriter sw = File.CreateText(user))
            {
                sw.WriteLine("Ward follows Alan");

                sw.WriteLine("Alan follows Martin");

                sw.WriteLine("Ward follows Martin, Alan");
            }

            expected = new(){ "Ward follows Alan", "Alan follows Martin", "Ward follows Martin, Alan" };

            expectedTweet = new()
            {
                "Alan> If you have a procedure with 10 parameters, you probably missed some.",
                "Ward> There are only two hard things in Computer Science: cache invalidation, naming things and off-by-1 errors.",
                "Alan> Random numbers should not be generated with a method chosen at random."
            };

        }
        [Fact]
       public void GetTweetByOneArgument_GivenUserFileNamedUser_ReturnsTweetUsersAndWhoTheyAreFollowing()
        {
            
                //Arrange
                var argument = "user";
             
                //Act
                var result = _tweetsFeedsRepo.GetUsersOrUserAndTheirTweet(argument);

            foreach (var user in result.User)
            {
                actuallyResults.Add(user.UserName);
            }


            //Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.User.Count);
            Assert.Equal(expected,actuallyResults);
                
        }

       [Fact]
       public void GetTweetByOneArgument_GivenTweetFileNamedTweet_ReturnsUsersTheirTweets()
        {
            //Arrange
            var argument = "tweet";

            //Act
            var result = _tweetsFeedsRepo.GetUsersOrUserAndTheirTweet(argument);

            foreach (var user in result.User)
            {
                actuallyResults.Add(user.UserName);
            }

            //Assert
            Assert.NotNull(result);
            Assert.Equal(3,result.User.Count);
            Assert.Equal(expectedTweet,actuallyResults);
        }

       [Fact]
       public void GetTweetByUserAndTweet_InvokingYourProgramWithUserAndTweetAsArguments_ReturnsUsersTheirTweetsAndTheirFollowingsTweets()
        {
            // Arrange

            var tweet = "tweet";
            var user = "user";

            //Act

            var result = _tweetsFeedsRepo.GetUsersAndTheirListOfTweetsAndTheirFollowingTweets(user,tweet);

            Assert.NotNull(result);
            Assert.Equal(3, result.Tweets.Count);

        }

       [Fact]
       public void InvalidFileName_PassInvalidFileName_ReturnsErrorMessageStatingFileIsPathIsInvalid()
        {
            // Arrange
            var tweet = "tweetInvalid";

            //Act
            var result = _tweetsFeedsRepo.GetUsersOrUserAndTheirTweet(tweet);

            Assert.NotNull(result);
            Assert.Equal("Could not find file 'C:\\datatxt\\tweetInvalid.txt'.", result.ErrorMessages.Error);
        }

        public void Dispose()
        {
            // Check if file already exists. If yes, delete it.     
            if (File.Exists(tweet))
            {
                File.Delete(tweet);
            }

            // Check if file already exists. If yes, delete it.     
            if (File.Exists(user))
            {
                File.Delete(user);
            }

            actuallyResults = new List<string>();
            expected = new List<string>();
            expectedTweet = new();
        }
    }
}
