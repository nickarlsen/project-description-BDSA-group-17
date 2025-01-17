using Microsoft.AspNetCore.Mvc;


namespace GitInsight;

[ApiController]
[Route("[controller]")]
public class CommitController : ControllerBase
{

    GitFetcher? gitFetcher;
    private readonly ILogger<CommitController> _logger;

    public CommitController(ILogger<CommitController> logger)
    {
        _logger = logger;
    }

    [HttpGet("Freq")]
    public IEnumerable<Freq> GetFreq(string repo)
    {
        gitFetcher = new GitFetcher(repo);
        var commits = gitFetcher.GitInsightFreq();
        
        foreach (var commit in commits)
        {
            yield return commit;
        }
        
    }

    [HttpGet("Auth")]
    public IEnumerable<Author> GetAuth(string repo)
    {
        gitFetcher = new GitFetcher(repo);
        var authorWithCommits = gitFetcher.GitInsightAuth();
        
        foreach (var authorWithCommit in authorWithCommits)
        {
            yield return authorWithCommit;
        }
    }
}
