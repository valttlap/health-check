// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;

using DbUp;
using DbUp.Engine;

internal class Program
{
    private static int Main(string[] args)
    {
        try
        {
            if (args.Length <= 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("");
                Console.WriteLine("Usage: ./HealthCheck.DbUpdater.exe [Operation] [ConnectionString]");
                Console.WriteLine("");
                Console.WriteLine("Examples of [ConnectionString]:");
                Console.WriteLine("\"Server=127.0.0.1;Port=5432;Database=HealthCheck;Integrated Security=true;\"");
                Console.WriteLine("\"Server=127.0.0.1;Port=5432;Database=HealthCheck;User Id=myUsername;Password=myPassword;\"");
                Console.WriteLine("");
                Console.WriteLine("[Operation]         [Description]");
                Console.WriteLine(" update              Updates sql scripts that are not already executed");
                Console.WriteLine(" mark                Mark all scripts as executed");
                Console.WriteLine(" markinitial         Mark initial scripts as executed");
                Console.WriteLine(" info                List all unexecuted scripts");
                Console.WriteLine("");
                Console.ResetColor();
                return -1;
            }

            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
            var connectionString = args[1];

            EnsureDatabase.For.PostgresqlDatabase(connectionString);

            DatabaseUpgradeResult result;

            UpgradeEngine upgrader;

            if (args.FirstOrDefault() == "markinitial")
            {
                throw new NotImplementedException();
            }
            else
            {
                upgrader = DeployChanges.To
                  .PostgresqlDatabase(connectionString)
                  .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                  .LogToConsole()
                  .Build();
            }

            if (args.FirstOrDefault() == "mark")
            {
                result = upgrader.MarkAsExecuted();
            }
            else if (args.FirstOrDefault() == "info")
            {
                var scripts = upgrader.GetScriptsToExecute();

                Console.WriteLine("Scripts that need to be run:");
                foreach (var sc in scripts)
                {
                    Console.WriteLine(sc.Name);
                }

                return 0;
            }
            else
            {
                result = upgrader.PerformUpgrade();
            }

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message);
            Console.ResetColor();
        }

        return -1;
    }
}
