using ScripTube.Models.YouTube;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Input;

namespace ScripTube.ViewModels.Commands
{
    class ExecutePapagoCommand : ICommand
    {
        private static readonly string NEW_LINE = "%0A%0A";
        private static readonly string PAPAGO_URL = "https://papago.naver.com/?sk=auto&tk=ko&hn=1&st=";
        private static readonly int MAX_STRING_SIZE = 5000;

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
                
                var sb = new StringBuilder(PAPAGO_URL);
                int count = 0;

                foreach (var item in subtitleItems)
                {
                    string plainText = item.Text.Replace('\n', ' ').Trim();
                    string encodedText = HttpUtility.UrlEncode(plainText, Encoding.UTF8);
                    StringBuilder sbLine = new StringBuilder(encodedText);
                    sbLine.Replace('+', ' ');
                    string formattedText = sbLine.ToString();
                    if (count + formattedText.Length > MAX_STRING_SIZE)
                    {
                        sb.Length -= NEW_LINE.Length;
                        System.Diagnostics.Process.Start(sb.ToString());
                        count = 0;
                        sb.Clear();
                        sb.Append(PAPAGO_URL);
                    }
                    else
                    {
                        sb.Append(formattedText);
                        sb.Append(NEW_LINE);
                        count += formattedText.Length + NEW_LINE.Length;
                    }
                }
                sb.Length -= NEW_LINE.Length;

                if (sb.Length > PAPAGO_URL.Length)
                {
                    System.Diagnostics.Process.Start(sb.ToString());
                }
            }
        }
    }
}