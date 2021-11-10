namespace WSATools.ViewModels
{
    public sealed class ListItem
    {
        public ListItem(string content)
        {
            Content = content;
        }
        public ListItem(string content, object value) : this(content)
        {
            Tag = value;
        }
        public string Content { get; set; }
        public object Tag { get; set; }
    }
}
