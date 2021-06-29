namespace FlatoutCMS.ConntentManagement.Content
{
    public interface IPageModel
    {
        string View { get; set; }
        string Title { get; set; }
        string Content { get; set; }
    }
}
