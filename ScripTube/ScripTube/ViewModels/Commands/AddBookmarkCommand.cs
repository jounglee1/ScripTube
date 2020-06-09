using Newtonsoft.Json;
using ScripTube.Models.Bookmark;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ScripTube.ViewModels.Commands
{
    class AddBookmarkCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            var viewModel = parameter as MainWindowViewModel;
            return (viewModel != null && viewModel.BookmarkItems != null);
        }

        public void Execute(object parameter)
        {
            var viewModel = parameter as MainWindowViewModel;
            if (viewModel != null)
            {
                var items = viewModel.BookmarkItems;
                if (items != null)
                {
                    string filename = Path.Combine(Directory.GetCurrentDirectory(), "Cache", viewModel.TargetVideo.ID + viewModel.CurrentVideoTime.GetHashCode() + ".png");
                    viewModel.TargetThumbnail = new Thumbnail(filename);

                    var item = new BookmarkItem() { Seconds = viewModel.CurrentVideoTime, Memo = "memo", ImagePath = filename };

                    items.Add(item);

                    viewModel.TargetVideo.BookmarkTray.SaveAsJson();
                }
            }
        }
    }
}
