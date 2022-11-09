using Microsoft.Data.Sqlite;

public class Program

{
    private static void Main(string[] args)
    {
        Console.WriteLine("Please provide a Git repository url in the form \"owner/repository\"");
        Console.WriteLine("Here: ");
        var repoUrl = Console.ReadLine();
        var gitFetcher = new GitFetcher(repoUrl!);

        //TODO Make a directory locally and clone into that
        string mode = "";
        bool doWhile = true;
        while (doWhile)
        {
            Console.WriteLine("Please select either Author mode(a) or Frequency Mode(f)");
            mode = Console.ReadLine()!;
            if (mode.Equals("a"))
            {
                gitFetcher.GitInsightAuth();
                doWhile = false;
            }
            else if (mode.Equals("f"))
            {
                gitFetcher.GitInsightFreq();
                doWhile = false;
            }
            else Console.WriteLine("Invalid input");
        }
    }
}