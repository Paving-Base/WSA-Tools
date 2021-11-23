using AdvancedSharpAdbClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WSATools.Core.Models;

namespace WSATools.Core.Receivers
{
    public class StorageReceiver : MultiLineReceiver
    {
        public StorageReceiver()
        {
            Storages = new ObservableCollection<StorageInfo>();
        }

        public ObservableCollection<StorageInfo> Storages { get; private set; }

        /// <summary>
        /// Processes the new lines.
        /// </summary>
        /// <param name="lines">The lines.</param>
        protected override void ProcessNewLines(IEnumerable<string> lines)
        {
            Storages.Clear();

            foreach (string? line in lines)
            {
                if (!string.IsNullOrEmpty(line) && !line.StartsWith("Filesystem"))
                {
                    string[] parts = Regex.Split(line, "\\s+", RegexOptions.IgnoreCase);
                    StorageInfo Info = new StorageInfo()
                    {
                        Mountedon = parts[5].Trim(),
                        Filesystem = parts[0].Trim(),
                        Size = Convert.ToInt32(parts[1].Trim()),
                        Used = Convert.ToInt32(parts[2].Trim()),
                        Available = Convert.ToInt32(parts[3].Trim()),
                        Use = Convert.ToInt32(parts[4].Trim().Replace("%", string.Empty))
                    };
                    Storages.Add(Info);
                }
            }
        }
    }
}
