

namespace GitInsight;
public class Program

{
    private static void Main(string[] args)
    {

        // var builder = WebApplication.CreateBuilder(args);
        var builder = WebApplication.CreateBuilder(args);


        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();

        /*
        Console.WriteLine("Please provide a Git repository url in the form \"owner/repository\"");
        Console.WriteLine("Here: ");
        var repoUrl = Console.ReadLine();
        var gitFetcher = new GitFetcher(repoUrl!);*/

        //TODO Make a directory locally and clone into that
        /*string mode = "";
        bool doWhile = true;
        while (doWhile)
        {
            Console.WriteLine("Please select either Author mode(a) or Frequency Mode(f)");
            mode = Console.ReadLine()!;
            if (mode.Equals("a"))
            {
                Console.WriteLine("You have selected Author mode");
                var commits = gitFetcher.GitInsightAuth();
                foreach (var commit in commits)
                {
                    Console.WriteLine(commit);
                }
                doWhile = false;
            }
            else if (mode.Equals("f"))
            {
                Console.WriteLine("You have selected Frequency mode");
                Console.WriteLine(gitFetcher);
                var commits = gitFetcher.GitInsightFreq();
                foreach (var commit in commits)
                {
                    Console.WriteLine(commit);
                }
                doWhile = false;
            }
            else Console.WriteLine("Invalid input");
        }

        
        gitFetcher.RemoveClones();*/
    }
}