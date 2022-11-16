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
    public void Test_Get_All_Commit_Dates()
    {
        //Arrange
        IEnumerable<DateTimeOffset> actual;
        var expected = new List<DateTimeOffset>{
            new DateTimeOffset(new DateTime(2022, 10, 11)),
            new DateTimeOffset(new DateTime(2022, 10, 11)),
            new DateTimeOffset(new DateTime(2022, 10, 07)),
            new DateTimeOffset(new DateTime(2022, 10, 06)),
            new DateTimeOffset(new DateTime(2022, 10, 06)),
            new DateTimeOffset(new DateTime(2022, 10, 06)),
            new DateTimeOffset(new DateTime(2022, 09, 25)),
            new DateTimeOffset(new DateTime(2022, 09, 25)),
            new DateTimeOffset(new DateTime(2021, 10, 21)),
            new DateTimeOffset(new DateTime(2021, 10, 21)),
            new DateTimeOffset(new DateTime(2021, 10, 15)),
            new DateTimeOffset(new DateTime(2021, 10, 15)),
            new DateTimeOffset(new DateTime(2021, 10, 15)),
            new DateTimeOffset(new DateTime(2021, 10, 04)),
            new DateTimeOffset(new DateTime(2021, 10, 04)),
            new DateTimeOffset(new DateTime(2021, 10, 03)),
            new DateTimeOffset(new DateTime(2021, 10, 02)),
            new DateTimeOffset(new DateTime(2021, 09, 24))
            };

        var gitFetcher = new GitFetcher(onlineRepoPath);
        


        //Act
        actual = gitFetcher.GetAllCommitDates();
         
        //Assert
        Assert.Equal(expected.First(), actual.First());
    }

    [Fact]
    public void Test_Commit_Frequency()
    {
        //Arrange 
        var gitFetcher = new GitFetcher(onlineRepoPath);

        //Act
        var expected = new List<Freq>{
            new Freq (2, new DateTime(11/10/2022)),
            new Freq (1, new DateTime(07/10/2022)),
            new Freq (3, new DateTime(06/10/2022)),
            new Freq (2, new DateTime(25/09/2022)),
            new Freq (2, new DateTime(21/10/2021)),
            new Freq (3, new DateTime(15/10/2021)),
            new Freq (2, new DateTime(04/10/2021)),
            new Freq (1, new DateTime(03/10/2021)),
            new Freq (1, new DateTime(02/10/2021)),
            new Freq (1, new DateTime(24/09/2021))
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
                new Freq (2, new DateTime(11/10/2022))
            }),
            new Author ("HelgeCPH", new List<Freq>{
                new Freq (1, new DateTime(07/10/2022))
            }),
            new Author ("Rasmus Lystrom", new List<Freq>{
                new Freq (3, new DateTime(06/10/2022)),
                new Freq (2, new DateTime(25/09/2022)),
                new Freq (2, new DateTime(21/10/2021)),
                new Freq (2, new DateTime(04/10/2021)),
                new Freq (1, new DateTime(03/10/2021)),
                new Freq (1, new DateTime(02/10/2021)),
                new Freq (1, new DateTime(24/09/2021))
            }),
            new Author ("Paolo Tell", new List<Freq>{
                new Freq (3, new DateTime(15/10/2021))
            })
        };

        var actual = gitFetcher.GitInsightAuth();

        //Assert
        Assert.Equal(expected, actual);
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