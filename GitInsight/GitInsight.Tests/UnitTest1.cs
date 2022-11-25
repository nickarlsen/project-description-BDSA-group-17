

namespace GitInsight.tests;


public class UnitTest1
{
    private readonly string localRepoPath;
    private readonly string onlineRepoPath;

    public UnitTest1()
    {
        localRepoPath = $@"C:\Users\nicka\OneDrive\Skrivebord\BDSA\Repos\assignment-05-group-12";
        onlineRepoPath = "nickarlsen/assignment-05-group-12";
    }


    [Fact]
    public void Test_Number_Of_All_Commits()
    {
        //Arrange
        var gitFetcher = new GitFetcher(onlineRepoPath);
        var actual = 0;

        //Act
        actual = gitFetcher.NumberOfCommitsAll();
         
        //Assert
        Assert.Equal(18, actual);
    }


    [Fact]
    public void Test_Commit_Frequency()
    {
        //Arrange 
        var gitFetcher = new GitFetcher(onlineRepoPath);

        //Act
        var expected = new List<Freq>{
            new Freq (2, new DateTime(2022, 10, 11)),
            new Freq (1, new DateTime(2022, 10, 07)),
            new Freq (3, new DateTime(2022, 10, 06)),
            new Freq (2, new DateTime(2022, 09, 25)),
            new Freq (2, new DateTime(2021, 10, 21)),
            new Freq (3, new DateTime(2021, 10, 15)),
            new Freq (2, new DateTime(2021, 10, 04)),
            new Freq (1, new DateTime(2021, 10, 03)),
            new Freq (1, new DateTime(2021, 10, 02)),
            new Freq (1, new DateTime(2021, 09, 24))
        };

        var actual = gitFetcher.GitInsightFreq();

        //Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_Commit_AuthorMode()
    {
        //Arrange 
        var gitFetcher = new GitFetcher(onlineRepoPath);

        //Act
        var expected = new List<Author>{
            new Author ("nkar", new List<Freq>{
                new Freq (2, new DateTime(2022, 10, 11))
            }),
            new Author ("HelgeCPH", new List<Freq>{
                new Freq (1, new DateTime(2022, 10, 07))
            }),
            new Author ("Rasmus Lystrøm", new List<Freq>{
                new Freq (3, new DateTime(2022, 10, 06)),
                new Freq (2, new DateTime(2022, 09, 25)),
                new Freq (2, new DateTime(2021, 10, 21)),
                new Freq (2, new DateTime(2021, 10, 04)),
                new Freq (1, new DateTime(2021, 10, 03)),
                new Freq (1, new DateTime(2021, 10, 02)),
                new Freq (1, new DateTime(2021, 09, 24))
            }),
            new Author ("Paolo Tell", new List<Freq>{
                new Freq (3, new DateTime(2021, 10, 15))
            })
        };

        var actual = gitFetcher.GitInsightAuth();

        
        //Assert
        Assert.Equivalent(expected, actual);
        
    }

    [Fact]
    public void Test_Get_All_Unique_Authors() 
    {
        //Arrange
        var gitFetcher = new GitFetcher(onlineRepoPath);

        //List<Commit> commits;
        using (var repo = new Repository(localRepoPath))
        {
            var commits = repo.Commits.ToList();    
        
            //Act
            var expected = new List<string> {"nkar", "HelgeCPH", "Rasmus Lystrøm", "Paolo Tell"};
            var actual = gitFetcher.GetAllAuthors(commits).ToList();
            /*var actual = new List<string>();
            foreach (Signature author in authors)
            {
                actual.Add(author.Name);
            }*/

            //Assert
            Assert.Equal(expected, actual);

        }
        
    }
}