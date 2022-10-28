public class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Please provide a Git repository from your local device");
        Console.Write("Here: ");
        var workingDirectory = Console.ReadLine();
        Console.WriteLine(""); //Whitespace
        string mode = "";
        bool doWhile = true;
        while (doWhile) 
        {
            Console.WriteLine("Please select either Author mode(a) or Frequency Mode(f)");
            mode = Console.ReadLine()!;
            if (mode.Equals("a"))
            {
                GitInsightAuth(workingDirectory!);
                doWhile = false;
            } 
            else if (mode.Equals("f"))
            {
                GitInsightFreq(workingDirectory!);
                doWhile = false;
            }
            else Console.WriteLine("Invalid input");    
        }
        // Console.WriteLine("Thats all for now folks!");
        // Console.WriteLine(GetAllCommitDates(workingDirectory));
        
    }

    public static void GitInsightFreq(string path) 
    {
        Console.WriteLine("Getting commits from: ", path);
        using (var repo = new Repository(path))
        {
            var commits = repo.Commits.ToList();
            var dates = GetAllCommitDates(path);
            var commitsQuery =
                        from commit in commits
                        group commit by commit.Author.When.Date into g
                        select new {g, commit = g.ToList().Count()};
            foreach (var i in commitsQuery)
            {
                Console.WriteLine(i.commit + " " + i.g.First().Author.When.Date.ToString("dd/MM/yyyy"));
                //Console.Write(" ");
                //Console.Write(i.g.First().Author.When.Date.ToString("dd/MM/yyyy")); // Will give you smth like 25/05/2011
                //Console.Write(i.g.First().Author.When.Date.Date);
                //Console.WriteLine("");
                
            }
        }
    }

    public static void GitInsightAuth(string path) 
    {
        Console.WriteLine("Getting commits from: ", path);
        using (var repo = new Repository(path))
        {
            var commits = repo.Commits.ToList();
            var authors = GetAllAuthors(commits);
            
            foreach (string author in authors)
            {
                var tempCommitList = new List<Commit>();
                Console.WriteLine("");
                Console.WriteLine(author);
                foreach (Commit commit in commits)
                {
                    if (commit.Author.Name.Equals(author))
                    {
                        tempCommitList.Add(commit);
                    }
                }
                var commitsQuery =
                        from commit in tempCommitList
                        group commit by commit.Author.When.Date into g
                        select new {g, commit = g.ToList().Count()};
                foreach (var i in commitsQuery)
                {
                    Console.WriteLine("    " + i.commit + " " + i.g.First().Author.When.Date.ToString("dd/MM/yyyy"));
                    //Console.Write(" ");
                    //Console.WriteLine(i.g.First().Author.When.Date.ToString("dd/MM/yyyy")); // Will give you smth like 25/05/2011
                }

            }
        }
    }

    public static IEnumerable<string> GetAllAuthors(List<Commit> commits)
    {
        return commits.DistinctBy(a => a.Author.Name).Select(a => a.Author.Name);
    }

    public static int NumberOfCommitsAll(string path)
    {
        using (var repo = new Repository(path))
        {
            var numCommits = repo.Commits.ToList().Count();
            Console.WriteLine("Number of commits: ", numCommits);
            return numCommits;
        }
    }

    public static IEnumerable<DateTimeOffset> GetAllCommitDates(string path)
    {
        using (var repo = new Repository(path))
        {
            var commits = repo.Commits.ToList();
            foreach (var i in commits)
            {
                var currDay = i.Author.When.Day;
                var currMonth = i.Author.When.Month;
                var currYear = i.Author.When.Year;
                yield return new DateTimeOffset(new DateTime(currYear, currMonth, currDay));
                //yield return i.Author.When;
            }
        }
    }

/*    public static void GetCommitsForDate (string path, DateTimeOffset date)
    {
        using (var repo = new Repository(path))
        {
            //var date = new DateTimeOffset(new DateTime());
            //var filter = new CommitFilter { SinceList = new[] { repo.Branches } };
            
            var commitLog = repo.Commits.QueryBy(new CommitFilter() {} );
            var commits = commitLog.Where(c => c.Committer.When == date);
            //commits^^ list of commits w the specific date
        }
    } */
    
}