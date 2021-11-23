using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WSATools.Core
{
    public sealed class Command
    {
        private static Command instance;
        public static Command Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Command();
                }

                return instance;
            }
        }
        private Command() { }
        public bool Shell(string cmd, out string message)
        {
            try
            {
                Process? process = new Process();
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.FileName = @"powershell.exe";
                process.StartInfo.Arguments = cmd;
                process.Start();
                message = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                process.Close();
                process.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        public bool Excute(string cmd, out string message)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                process.StandardInput.WriteLine($"{cmd}&exit");
                process.StandardInput.AutoFlush = true;
                message = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                process.Close();
                process.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        public bool Excute(IEnumerable<string> cmds, out string message)
        {
            try
            {
                int total = cmds.Count();
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                for (int idx = 0; idx < total; idx++)
                {
                    string? cmd = cmds.ElementAt(idx);
                    if (idx == total - 1)
                    {
                        process.StandardInput.WriteLine(cmd + "&exit");
                    }
                    else
                    {
                        process.StandardInput.WriteLine(cmd);
                    }
                }
                process.StandardInput.AutoFlush = true;
                message = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                process.Close();
                process.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
    }
}