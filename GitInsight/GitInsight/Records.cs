public record JsonCommit(int Id, string repoOwner, string repoName, string Author, DateTime CommitDate);
public record GitRepo(string Name, string Owner);
public record Freq(int CountCommits, DateTime CommitDate);
public record Author(string Name, IEnumerable<Freq> Freqs);