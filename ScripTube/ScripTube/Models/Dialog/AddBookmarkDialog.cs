namespace ScripTube.Models.Dialog
{
    public class AddBookmarkDialog : MessageDialog
    {
        public string MemoText { get; set; }

        public AddBookmarkDialog()
        {
            Title = "북마크 생성";
        }
    }
}
