using ScripTube.Models.YouTube;
using ScripTube.Utils;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace ScripTube.ViewModels.Commands
{
    class CopyTimeAndSubtitleTextToClipboardCommand : ICommand
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
            IEnumerable enumerable = parameter as IEnumerable;
            if (enumerable != null)
            {
                var subtitleItems = enumerable.OfType<SubtitleItem>().OrderBy(s => s.StartSeconds).ToList();

                return subtitleItems.Count > 0;
            }

            return false;
        }

        public void Execute(object parameter)
        {
            IEnumerable enumerable = parameter as IEnumerable;
            if (enumerable != null)
            {
                var subtitleItems = enumerable.OfType<SubtitleItem>().OrderBy(s => s.StartSeconds).ToList();

                var sb = new StringBuilder();
                foreach (var item in subtitleItems)
                {
                    sb.Append(TimeFormatUtil.GetHHMMSSOrMMSS(item.StartSeconds, item.IsOneHourExcessed));
                    sb.Append(' ');
                    sb.Append(item.Text);
                    sb.Append(Environment.NewLine);
                    sb.Append(Environment.NewLine);
                }
                sb.Length -= Environment.NewLine.Length;

                Clipboard.SetText(sb.ToString());
            }
        }
    }
}
