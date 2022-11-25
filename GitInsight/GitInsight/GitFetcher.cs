namespace GitInsight;
public class GitFetcher
{
    private DBController dbController;
    private string localRepoPath;
    private string shortLocalRepoPath;
    public GitFetcher(string repoUrl)
    {
        
        
    
        //An instance of GitFetcer will be made in Program.cs/main
        var httpsString = "https://github.com/";
        var workingDirectory = httpsString + repoUrl;
        
        
        shortLocalRepoPath = @"ClonedRepos";
        RemoveClones();
        localRepoPath = @"ClonedRepos";
        //var repoName = repoUrl.Split("/");

        dbController = new DBController(localRepoPath + @"\" + repoUrl);

        try
        {
            Console.WriteLine(localRepoPath);
            System.IO.Directory.CreateDirectory(localRepoPath + $@"\{repoUrl}");
            localRepoPath = $@"{localRepoPath}/{repoUrl}";
            Console.WriteLine(localRepoPath);
            
             
            
            var result = Repository.Clone(workingDirectory, localRepoPath, new CloneOptions()
            {
                //BranchName = "main",
                //BranchName = "master",
            });
            Console.WriteLine(result);
            dbController.StartConnectionDBLocal(result);
            
            
            Console.WriteLine("Repo already exists");
            
            
            
            Console.WriteLine("Everything is gucci");
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
        }


        
    }

    public void CheckDBData() 
    {
        
    }

    public IEnumerable<Freq> GitInsightFreq()
    {
        Console.WriteLine("Getting commits from: ", localRepoPath);
        using (var repo = new Repository(localRepoPath))
        {
            Console.WriteLine("Printing Path info:");
            Console.WriteLine(repo.Info.Path.ToString());
            Console.WriteLine(repo.Network.Remotes.First().PushUrl.ToString());
            
            //Console.WriteLine(repo.);
            var commits = repo.Commits.ToList();
            var commitsQuery =
                        from commit in commits
                        group commit by commit.Author.When.Date into g
                        select new {g, commit = g.ToList().Count()};
            
            
            foreach (var i in commitsQuery)
            {
                
                Console.WriteLine(i.commit + " " + i.g.First().Author.When.Date.ToString("dd/MM/yyyy"));
                yield return(new Freq(i.commit, i.g.First().Author.When.Date));
            }
        }
    }

    public IEnumerable<Author> GitInsightAuth() 
    {
        Console.WriteLine("Getting commits from: ", localRepoPath);
        using (var repo = new Repository(localRepoPath))
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

                var tempFreqs = new List<Freq>();
                foreach (var i in commitsQuery)
                {
                    Console.WriteLine("    " + i.commit + " " + i.g.First().Author.When.Date.ToString("dd/MM/yyyy"));
                    tempFreqs.Add(new Freq(i.commit, i.g.First().Author.When.Date));
                    
                }

                
                yield return new Author(author, tempFreqs);
                
            }
        }
    }    

      public IEnumerable<string> GetAllAuthors(List<Commit> commits)
    {
        return commits.DistinctBy(c => c.Author.Name).Select(c => c.Author.Name);
    }

    public int NumberOfCommitsAll()
    {
        using (var repo = new Repository(localRepoPath))
        {
            var numCommits = repo.Commits.ToList().Count();
            Console.WriteLine("Number of commits: ", numCommits);
            return numCommits;
        }
    }

    public void RemoveClones()
    {
        Console.WriteLine("Removing Local Repos");
        /*try 
        {
            System.IO.Directory.Delete(localRepoPath, true);
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
        }*/
        //Directory.Delete(localRepoPath + "/.git", true);
        

        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(shortLocalRepoPath);

        if (dir.Exists)
        {
            setAttributesNormal(dir);
            Console.WriteLine("Deleting cloned repos now");
            dir.Delete(true);
        }    
    }

    public void setAttributesNormal(DirectoryInfo dir) 
        {
            foreach (var subDir in dir.GetDirectories())
                setAttributesNormal(subDir);
            foreach (var file in dir.GetFiles())
            {
                Console.WriteLine("Writing file:");
                Console.WriteLine(file);
            
                //var currFile = 
                File.SetAttributes(file.FullName, FileAttributes.Normal);
                //file.Attributes = FileAttributes.Normal;
            }
        }
}