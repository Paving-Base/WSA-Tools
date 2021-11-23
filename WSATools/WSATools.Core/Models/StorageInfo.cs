namespace WSATools.Core.Models
{
    public class StorageInfo
    {
        public int Use { get; set; }
        public int Size { get; set; }
        public int Used { get; set; }
        public int Available { get; set; }
        public string? Mountedon { get; set; }
        public string? Filesystem { get; set; }
    }
}
