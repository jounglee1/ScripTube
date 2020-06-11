using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace ScripTube.Models.Dialog
{
    public class CreateBookmarkDialog : MessageDialog
    {
        public string MemoText { get; set; }

        public CreateBookmarkDialog()
        {
            Title = "북마크 생성";
        }
    }
}
