using Newtonsoft.Json;
using ScripTube.Models.Bookmark;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            return true;
        }

        public void Execute(object parameter)
        {
            var viewModel = parameter as MainWindowViewModel;
            if (viewModel != null)
            {
                var items = viewModel.BookmarkItems;
                if (items != null)
                {
                    string path = "https://i.pinimg.com/originals/0e/a5/d2/0ea5d20fdca383697c5af70ba588ef4a.jpg";
                    //JsonConvert.SerializeObject(product);

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(path);
                    bitmap.EndInit();

                    var item = new BookmarkItem() { Seconds = viewModel.CurrentVideoTime, Memo = "memo", ImagePath = path, BitmapImage = bitmap };

                    items.Add(item);
                }
            }
        }
    }
}
