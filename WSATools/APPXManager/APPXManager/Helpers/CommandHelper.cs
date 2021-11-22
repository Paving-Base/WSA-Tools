using APPXManager.Exceptions;
using APPXManager.Receivers;
using System.Diagnostics;

namespace APPXManager.Helpers
{
    public static class CommandHelper
    {
        /// <summary>
        /// Executes a shell command on the remote device
        /// </summary>
        /// </param>
        /// <param name="command">The command to execute</param>
        /// <param name="rcvr">The shell output receiver</param>
        public static void ExecuteShellCommand(string command, IShellOutputReceiver rcvr)
        {
            try
            {
                ExecuteShellCommandAsync(command, rcvr, CancellationToken.None).Wait();
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Count == 1)
                {
                    throw ex.InnerException;
                }
                else
                {
                    throw;
                }
            }
        }

        /// <inheritdoc/>
        public static async Task ExecuteShellCommandAsync(string command, IShellOutputReceiver receiver, CancellationToken cancellationToken)
        {
            var start = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                Arguments = command,
                CreateNoWindow = true
            };

            using var process = Process.Start(start);

            process.EnableRaisingEvents = true;

            try
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    // Previously, we would loop while reader.Peek() >= 0. Turns out that this would
                    // break too soon in certain cases (about every 10 loops, so it appears to be a timing
                    // issue). Checking for reader.ReadLine() to return null appears to be much more robust
                    // -- one of the integration test fetches output 1000 times and found no truncations.
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var line = await reader.ReadLineAsync().ConfigureAwait(false);

                        if (line == null)
                        {
                            break;
                        }

                        if (receiver != null)
                        {
                            receiver.AddOutput(line);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // If a cancellation was requested, this main loop is interrupted with an exception
                // because the socket is closed. In that case, we don't need to throw a ShellCommandUnresponsiveException.
                // In all other cases, something went wrong, and we want to report it to the user.
                if (!cancellationToken.IsCancellationRequested)
                {
                    throw new ShellCommandUnresponsiveException(e);
                }
            }
            finally
            {
                if (receiver != null)
                {
                    receiver.Flush();
                }
            }
        }
    }
}
