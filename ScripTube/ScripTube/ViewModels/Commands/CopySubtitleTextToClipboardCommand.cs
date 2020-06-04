﻿using ScripTube.Models.YouTube;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ScripTube.ViewModels.Commands
{
    class CopySubtitleTextToClipboardCommand : ICommand
    {
        private static string NEW_LINE = "%0A%0A";
        private static string PAPAGO_URL = "https://papago.naver.com/?sk=auto&tk=ko&hn=1&st=";
        private static int MAX_STRING_SIZE = 5000;

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