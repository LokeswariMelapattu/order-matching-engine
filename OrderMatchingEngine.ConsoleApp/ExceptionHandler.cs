
using System;
using System.Threading.Tasks;

public static class ExceptionHandler
{
    /// <summary>
    /// Wraps an async function with exception handling
    /// </summary>
    public static async Task RunWithExceptionHandling(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"[Validation Error] {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"[Business Logic Error] {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Unhandled Exception] {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
        finally
        {
            Console.WriteLine("Application is shutting down gracefully.");
        }
    }

    /// <summary>
    /// Registers global exception handlers for background threads and async tasks
    /// </summary>
    public static void RegisterGlobalExceptionHandlers()
    {
        AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
        {
            Console.WriteLine($"[Critical Error] Unhandled Exception: {eventArgs.ExceptionObject}");
        };

        TaskScheduler.UnobservedTaskException += (sender, eventArgs) =>
        {
            Console.WriteLine($"[Task Error] Unobserved Exception: {eventArgs.Exception}");
            eventArgs.SetObserved(); // Prevents process termination due to unobserved exceptions
        };
    }
}

