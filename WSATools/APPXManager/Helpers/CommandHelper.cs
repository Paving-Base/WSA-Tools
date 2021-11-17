using APPXManager.Receivers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPXManager.Helpers
{
    public static class CommandHelper
    {
        public static async Task ExecuteRemoteCommandAsync(string command, IShellOutputReceiver receiver, CancellationToken cancellationToken)
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
