// See https://aka.ms/new-console-template for more information
using APPXManager.Helpers;
using APPXManager.Receivers;

internal class Program
{
    private static void Main()
    {
        ConsoleOutputReceiver receiver = new ConsoleOutputReceiver();
        CommandHelper.ExecuteRemoteCommandAsync("dir", receiver, CancellationToken.None).Wait();
        Console.WriteLine(receiver.ToString());
    }
}