using Microsoft.Data.Sqlite;

public class Program
{
    private static void Main(string[] args)
    {
        

        Console.WriteLine("Please provide a Git repository url in the form \"owner/repository\"");
        Console.WriteLine("Here: ");
        var httpsString = "https://github.com/";
        var repoUrl = Console.ReadLine();
        var workingDirectory = httpsString + repoUrl;

        //TODO Make a directory locally and clone into that
        var localRepoPath = @"C:\Users\nicka\OneDrive\Skrivebord\BDSA\Repos";
        try
        {
            System.IO.Directory.CreateDirectory(localRepoPath + $@"\{repoUrl}");
            localRepoPath += $@"\{repoUrl}";
            var result = Repository.Clone(workingDirectory, localRepoPath, new CloneOptions()
            {
                BranchName = "main",
            });
            
            
            string mode = "";
            bool doWhile = true;
            while (doWhile) 
            {
                Console.WriteLine("Please select either Author mode(a) or Frequency Mode(f)");
                mode = Console.ReadLine()!;
                if (mode.Equals("a"))
                {
                    GitInsightAuth(localRepoPath!);
                    doWhile = false;
                } 
                else if (mode.Equals("f"))
                {
                    GitInsightFreq(localRepoPath!);
                    doWhile = false;
                }
                else Console.WriteLine("Invalid input");    
            }
            StartConnectionDBLocal();
        }
        catch (System.Exception e)
        {
            
            Console.Write(e.StackTrace);
        }
        


        
        
        
    }

    public static void StartConnectionDBLocal()
    {
        var ds = new SqliteConnectionStringBuilder
        {
            DataSource = @"C:\Users\nicka\OneDrive\Skrivebord\BDSA\Repos\project-description-BDSA-group-17\GitInsight\GitInsight\GitInsight.db"
        };
        
        string stm = "SELECT SQLITE_VERSION()";

        var con = new SqliteConnection(ds.ConnectionString);
        con.Open();

        var cmd = new SqliteCommand(stm, con);
        
        //TODO Read and load the .sql file into db
        /*cmd.CommandText = "Drop Table if exists GitRepos";
        cmd.ExecuteNonQuery();

        cmd.CommandText = "Drop Table if exists Commits";
        cmd.ExecuteNonQuery();

        cmd.CommandText = "Create Table Commits (id INTEGER Primary Key,repoId INTEGER REFERENCES GitRepos(id),author VARCHAR(50),commitDate DATE)";
        cmd.ExecuteNonQuery();

        cmd.CommandText = "Create Table GitRepos (author VARCHAR(50),name VARCHAR(50),Primary Key(author, name))";
        cmd.ExecuteNonQuery();*/


        var gitrepo = "assignment-05-group-12";
        stm = $"SELECT * FROM GitRepos WHERE name = '{gitrepo}'";
        cmd = new SqliteCommand(stm, con);
        SqliteDataReader rdr = cmd.ExecuteReader();

        while (rdr.Read())
        {
            Console.WriteLine($"{rdr.GetString(0)} {rdr.GetString(1)}");
        }
        
        stm = "SELECT * FROM GitRepos";

        cmd = new SqliteCommand(stm, con);
        rdr = cmd.ExecuteReader();

        while (rdr.Read())
        {
            Console.WriteLine($"{rdr.GetString(0)} {rdr.GetString(1)}");
        }
        rdr.Close();

        InsertRepoIntoDB("tobias/tobias", cmd);
    }

    public static void InsertRepoIntoDB(string url, SqliteCommand cmd)
    {
        var urlArray = url.Split("/");
        var author = urlArray.First();
        var name = urlArray.Last();

        var stm = $"SELECT * FROM GitRepos WHERE name = '{author}' AND author = '{name}'";
        cmd.CommandText = stm;
        SqliteDataReader rdr = cmd.ExecuteReader();

        
        
        if (rdr.HasRows)
        {
            rdr.Close();
            Console.WriteLine("Repo already exists in DB");
            UpdateCommits(url, cmd);
        }
        else
        {
            Console.WriteLine("Repo does not exist in DB");
            rdr.Close();
            cmd.CommandText = $"INSERT INTO GitRepos (author, name) VALUES ('{author}', '{name}')";
            cmd.ExecuteNonQuery();

            InsertCommits(url, cmd);
        }
    }
    

    public static void InsertCommits(string url, SqliteCommand cmd)
    {
        var urlArray = url.Split("/");
        var urlAuthor = urlArray.First();
        var name = urlArray.Last();

        var repo = new Repository(url);
        var commits = repo.Commits;
        foreach (var commit in commits)
        {
            var author = commit.Author.Name;
            var date = commit.Author.When;
            var repoKey = (urlArray.First(), urlArray.Last());
            cmd.CommandText = $"INSERT INTO Commits (repoKey, author, commitDate) VALUES ({repoKey}, '{author}', '{date}')";
            cmd.ExecuteNonQuery();
        }
    }
    public static void UpdateCommits(string url, SqliteCommand cmd)
    {    
        var urlArray = url.Split("/");
        var urlAuthor = urlArray.First();
        var name = urlArray.Last();

        var stm = $"SELECT Count(*) FROM Commits";
        cmd.CommandText = stm;
        SqliteDataReader rdr = cmd.ExecuteReader();

        
        if (!rdr.HasRows)
        {
            rdr.Close();
            InsertCommits(url, cmd);
            
        }
        else
        {

            rdr.Close();

            var repo = new Repository(url);
            var commits = repo.Commits;
            if (commits.Count() == rdr.GetInt32(0))
            {
                Console.WriteLine("No new commits");
            }
            else
            {
                Console.WriteLine("New commits");
                var neededCommits = rdr.GetInt32(0) - commits.Count();
                for (int i = 0; i < neededCommits; i++)
                {
                    var commit = commits.ElementAt(i);
                    var author = commit.Author.Name;
                    var date = commit.Author.When;
                    var repoKey = (urlArray.First(), urlArray.Last());
                    cmd.CommandText = $"INSERT INTO Commits (repoKey, author, commitDate) VALUES ({repoKey}, '{author}', '{date}')";
                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine("commits already exists in DB");
        } 
    }
    
    //Attempted method to integrate Docker DB, Potentially used in the future
    public static void StartConnectionDB()
    {
        Console.WriteLine("Connecting to database...");
        //var connString = "postgres://postgres:postgrespw@localhost:49156"; 
        //var connString = "Host=host.docker.internal;Username=postgres;Password=postgrespw;Database=GitInsight";
        //var connString = "Host=postgres;Username=postgres;Password=postgrespw;Database=GitInsight";
        //var connString = "Server=localpg;Database=GitInsight;Username=postgres;Host=postgres;Password=postgrespw;";
        //Port=49156
        /*var connStringBuilder = new NpgsqlConnectionStringBuilder();

        Console.WriteLine("1");
        connStringBuilder.Host = "host.docker.internal";
        connStringBuilder.Username = "postgres";
        connStringBuilder.Password = "postgrespw";
        connStringBuilder.Database = "GitInsight";
        connStringBuilder.Port = 49156;
        
        Console.WriteLine(connStringBuilder.ConnectionString);
        await using var conn = new NpgsqlConnection(connStringBuilder.ConnectionString);*/
        Console.WriteLine("2");
        //await conn.OpenAsync();
        //conn.Open();

        Console.WriteLine("Connected to database!");

        // Insert some data
        /*await using (var cmd = new NpgsqlCommand("INSERT INTO gitrepos (id, name) VALUES (1, 'repo1')", conn))
        {
            cmd.Parameters.AddWithValue("Hello world");
            await cmd.ExecuteNonQueryAsync();
        }*/


        Console.WriteLine("Reading data from table gitrepos");
        // Retrieve all rows
        /*await using (var cmd = new NpgsqlCommand("SELECT name FROM gitrepos", conn))
        await using (var reader = await cmd.ExecuteReaderAsync())
        
        {
            while (await reader.ReadAsync())
            {
                Console.WriteLine("printing select statement:");
                Console.WriteLine(reader.GetString(0));
            }
        }*/
    } 

    
    
    public static void GitInsightFreq(string path)
    {
        Console.WriteLine("Getting commits from: ", path);
        using (var repo = new Repository(path))
        {
            Console.WriteLine("Printing Path info:");
            Console.WriteLine(repo.Info.Path.ToString());
            Console.WriteLine(repo.Network.Remotes.First().PushUrl.ToString());
            
            //Console.WriteLine(repo.);
            var commits = repo.Commits.ToList();
            var dates = GetAllCommitDates(path);
            var commitsQuery =
                        from commit in commits
                        group commit by commit.Author.When.Date into g
                        select new {g, commit = g.ToList().Count()};
            foreach (var i in commitsQuery)
            {
                
                Console.WriteLine(i.commit + " " + i.g.First().Author.When.Date.ToString("dd/MM/yyyy"));
                
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
                    
                }
            }
        }
    }

    public static IEnumerable<string> GetAllAuthors(List<Commit> commits)
    {
        return commits.DistinctBy(c => c.Author.Name).Select(c => c.Author.Name);
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
                
            }
        }
    }
}