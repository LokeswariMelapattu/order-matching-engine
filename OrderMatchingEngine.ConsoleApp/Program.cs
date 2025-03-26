
using OrderMatchingEngine.Domain;
using OrderMatchingEngine.ApplicationLogic;
using OrderMatchingEngine.DataAccess;
using System.Runtime.InteropServices;

namespace OrderMatchingEngine.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Register global handlers for background & async errors
            ExceptionHandler.RegisterGlobalExceptionHandlers();

            // Run the application with error handling
            await ExceptionHandler.RunWithExceptionHandling(async () =>
            {
                var app = new OrderMatchingApp();
                await app.Run();
            });

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

    }
}