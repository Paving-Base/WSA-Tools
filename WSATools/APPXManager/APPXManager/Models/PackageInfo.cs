namespace APPXManager.Models
{
    public class PackageInfo
    {
        public bool IsBundle { get; set; }
        public bool IsFramework { get; set; }
        public bool NonRemovable { get; set; }
        public bool IsResourcePackage { get; set; }
        public bool IsDevelopmentMode { get; set; }
        public bool IsPartiallyStaged { get; set; }

        public string? Name { get; set; }
        public string? Status { get; set; }
        public string? Version { get; set; }
        public string? Publisher { get; set; }
        public string? RunspaceId { get; set; }
        public string? ResourceId { get; set; }
        public string? PublisherId { get; set; }
        public string? Architecture { get; set; }
        public string? Dependencies { get; set; }
        public string? SignatureKind { get; set; }
        public string? PackageFullName { get; set; }
        public string? InstallLocation { get; set; }
        public string? PackageFamilyName { get; set; }
        public string? PackageUserInformation { get; set; }
    }
}
