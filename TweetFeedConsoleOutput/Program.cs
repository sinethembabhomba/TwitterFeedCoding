
using Microsoft.Extensions.DependencyInjection;
using TweetFeedsEntities;
using TweetFeedsRepository.Contracts;
using TweetFeedsRepository.Repo;

//setup our DI
var serviceProvider = new ServiceCollection()
                     .AddSingleton<ITweetsFeedsRepo,TweetsFeedsRepo>()
                     .BuildServiceProvider();

var tweeterFeeds = serviceProvider.GetService<ITweetsFeedsRepo>();

string firstArgument = "";
string secondArgument = "";

TwitterFeeds myTweets = new();

var inputValue = String.Empty;
Console.WriteLine("-------------- Twitter Feed Coding Assignment ------------");
do
{
    try
    {
        Console.WriteLine();
        Console.WriteLine("Choose an option from the following list:");
        Console.WriteLine("\t - User");
        Console.WriteLine("\t - Tweet");
        Console.WriteLine("\t - User and Tweet");
        Console.WriteLine("\t - Exit to stop application ");
        Console.WriteLine();
        Console.Write("Your option? : ");

        inputValue = Console.ReadLine();
        if (string.IsNullOrEmpty(inputValue))
        {
            throw new FormatException("No input provided");
        }

        if(!inputValue.Equals("User", StringComparison.InvariantCultureIgnoreCase)  &&
            !inputValue.Equals("Tweet", StringComparison.InvariantCultureIgnoreCase) &&
            !inputValue.Equals("User and Tweet", StringComparison.InvariantCultureIgnoreCase)&&
            !inputValue.Equals("Exit", StringComparison.InvariantCultureIgnoreCase))
        {
            throw new FormatException("Please choose valid option");
        }
        
        string[] input = inputValue.Split("and");

        Console.WriteLine();

        if (input.Length == 1)
        {
            var givenFileName = tweeterFeeds.GetUsersOrUserAndTheirTweet(inputValue);

            if(givenFileName != null)
            {
                foreach(var item in givenFileName.User)
                {
                    Console.WriteLine(item.UserName);
                }
            }
        }
        else
        {
            foreach(var item in input)
            {
                var checkedValue = item.ToLower();

                if(firstArgument == "")
                {
                    firstArgument = item;
                }
                else if(secondArgument == "")
                {
                    secondArgument = item;
                }
            }

            if(firstArgument != "" && secondArgument != "") 
            {
                myTweets = tweeterFeeds.GetUsersAndTheirListOfTweetsAndTheirFollowingTweets(firstArgument, secondArgument);
            }
            else
            {
                throw new FormatException("Provided incorrect input");
            }

            if (myTweets != null)
            {
                foreach (var item in myTweets.Tweets)
                {
                    Console.WriteLine(item.TweetOwner);
            
                    foreach (var tweets in item.Tweet)
                    {
                        Console.WriteLine("    " + tweets);
                    }
                    Console.WriteLine();
                }
            }
        }
        Console.WriteLine();
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine(ex.Message);
        Console.WriteLine();
    }
}
while (!inputValue.ToString().Equals("Exit",StringComparison.InvariantCultureIgnoreCase));
Console.Read();