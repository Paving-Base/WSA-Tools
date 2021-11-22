using APPXManager.Models;
using System.Collections.ObjectModel;

namespace APPXManager.Receivers
{
    /// <summary>
    /// Parses the output of the various <c>pm</c> commands.
    /// </summary>
    public class PackageManagerReceiver : MultiLineReceiver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PackageManagerReceiver"/> class.
        /// </summary>
        /// <param name="device">
        /// The device for which the package information is being received.
        /// </param>
        /// <param name="packageManager">
        /// The parent package manager.
        /// </param>
        public PackageManagerReceiver()
        {
            Packages = new ObservableCollection<PackageInfo>();
        }

        /// <summary>
        /// Gets the list of packages currently installed on the device. They key is the name of the
        /// package; the value the package path.
        /// </summary>
        public ObservableCollection<PackageInfo> Packages { get; private set; }

        /// <summary>
        /// Processes the new lines.
        /// </summary>
        /// <param name="lines">The lines.</param>
        protected override void ProcessNewLines(IEnumerable<string> lines)
        {
            PackageInfo Info = new PackageInfo();

            Packages.Clear();

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    if (!string.IsNullOrEmpty(Info.PackageFamilyName))
                    {
                        Packages.Add(Info);
                    }
                    Info = new PackageInfo();
                }
                else
                {
                    Info ??= new PackageInfo();

                    string[] parts = line.Split(':');

                    switch (parts[0].Trim())
                    {
                        case "Name":
                            Info.Name = parts[1].Trim();
                            break;
                        case "Status":
                            Info.Status = parts[1].Trim();
                            break;
                        case "Version":
                            Info.Version = parts[1].Trim();
                            break;
                        case "IsBundle":
                            Info.IsBundle = Convert.ToBoolean(parts[1].Trim());
                            break;
                        case "Publisher":
                            Info.Publisher = parts[1].Trim();
                            break;
                        case "RunspaceId":
                            Info.RunspaceId = parts[1].Trim();
                            break;
                        case "ResourceId":
                            Info.ResourceId = parts[1].Trim();
                            break;
                        case "PublisherId":
                            Info.PublisherId = parts[1].Trim();
                            break;
                        case "IsFramework":
                            Info.IsFramework = Convert.ToBoolean(parts[1].Trim());
                            break;
                        case "Architecture":
                            Info.Architecture = parts[1].Trim();
                            break;
                        case "Dependencies":
                            Info.Dependencies = parts[1].Trim();
                            break;
                        case "NonRemovable":
                            Info.NonRemovable = Convert.ToBoolean(parts[1].Trim());
                            break;
                        case "SignatureKind":
                            Info.SignatureKind = parts[1].Trim();
                            break;
                        case "PackageFullName":
                            Info.PackageFullName = parts[1].Trim();
                            break;
                        case "InstallLocation":
                            Info.InstallLocation = $"{parts[1].Trim()}:{parts[2].Trim()}";
                            break;
                        case "PackageFamilyName":
                            Info.PackageFamilyName = parts[1].Trim();
                            break;
                        case "IsResourcePackage":
                            Info.IsResourcePackage = Convert.ToBoolean(parts[1].Trim());
                            break;
                        case "IsDevelopmentMode":
                            Info.IsDevelopmentMode = Convert.ToBoolean(parts[1].Trim());
                            break;
                        case "IsPartiallyStaged":
                            Info.IsPartiallyStaged = Convert.ToBoolean(parts[1].Trim());
                            break;
                        case "PackageUserInformation":
                            Info.PackageUserInformation = parts[1].Trim();
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Adds a line to the output.
        /// </summary>
        /// <param name="line">
        /// The line to add to the output.
        /// </param>
        public override void AddOutput(string line)
        {
            if (line.StartsWith(" "))
            {
                string lastline = Lines.Last();
                line = lastline + line.Trim();
                Lines.Remove(lastline);
            }
            Lines.Add(line);
        }
    }
}
