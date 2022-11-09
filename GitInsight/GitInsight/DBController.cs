
public class DBController
{

    public DBController()
    {
        //Makes an instance of DBController

    }

    public void StartConnectionDBLocal()
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

        //InsertRepoIntoDB("tobias/tobias", cmd);
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

    public void InsertRepoIntoDB(string url, SqliteCommand cmd)
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

    public void InsertCommits(string url, SqliteCommand cmd)
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

    public void UpdateCommits(string url, SqliteCommand cmd)
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

}