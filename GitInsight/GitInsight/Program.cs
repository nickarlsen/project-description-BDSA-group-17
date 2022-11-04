using Microsoft.Data.Sqlite;

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

        StartConnectionDBLocal();
        // Console.WriteLine("Thats all for now folks!");
        // Console.WriteLine(GetAllCommitDates(workingDirectory));
        
    }


    public static void StartConnectionDBLocal()
    {
        string cs = "Data Source=:memory:";
        //string cs = @"URI=file:C:\Users\nicka\OneDrive\Skrivebord\BDSA\Repos\project-description-BDSA-group-17\GitInsight\GitInsight\GitInsight.db";
        //string cs = @"C:\Users\nicka\OneDrive\Skrivebord\BDSA\Repos\project-description-BDSA-group-17\GitInsight\GitInsight\GitInsight.db";
        string stm = "SELECT SQLITE_VERSION()";

        var con = new SqliteConnection(cs);
        con.Open();

        var cmd = new SqliteCommand(stm, con);
        string version = cmd.ExecuteScalar().ToString();

        Console.WriteLine($"SQLite version: {version}");

        cmd.CommandText = "DROP TABLE IF EXISTS GitRepos";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"CREATE TABLE GitRepos (id INTEGER PRIMARY KEY, name TEXT)";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"INSERT INTO GitRepos (id, name) VALUES (1, 'assignment-05-group-12')";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"INSERT INTO GitRepos (id, name) VALUES (2, 'Magnus repo')";
        cmd.ExecuteNonQuery();

        var gitrepo = "assignment-05-group-12";
        stm = $"SELECT * FROM GitRepos WHERE name = '{gitrepo}'";
        cmd = new SqliteCommand(stm, con);
        SqliteDataReader rdr = cmd.ExecuteReader();

        while (rdr.Read())
        {
            Console.WriteLine($"{rdr.GetInt32(0)} {rdr.GetString(1)}");
        }

        

        stm = "SELECT * FROM GitRepos";

        cmd = new SqliteCommand(stm, con);
        rdr = cmd.ExecuteReader();

        while (rdr.Read())
        {
            Console.WriteLine($"{rdr.GetInt32(0)} {rdr.GetString(1)}");
        }

        
    }
    
    public async static void StartConnectionDB()
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