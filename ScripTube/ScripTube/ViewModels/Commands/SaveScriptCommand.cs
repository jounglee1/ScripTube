using ScripTube.Models.YouTube;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace ScripTube.ViewModels.Commands
{
    class SaveScriptCommand : ICommand
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

        public MainWindowViewModel ViewModel { get; }

        public SaveScriptCommand(MainWindowViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var subtitle = parameter as Subtitle;
            if (subtitle != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.DefaultExt = "*.txt";
                saveFileDialog.Filter = "텍스트 파일 (*.txt)|*.txt|모든 파일(*.*)|*.*";

                var subItemText = new List<string>();
                var subItemTime = new List<string>();

                for (int i = 0; i < subtitle.Items.Count; i++)
                {
                    subItemText.Add(subtitle.Items[i].Text);
                    subItemTime.Add(subtitle.Items[i].StartTimeFormat);
                }

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write);
                    StreamWriter streamWriter = new StreamWriter(fileStream);

                    foreach (var subItems in subItemTime.Zip(subItemText, Tuple.Create)) // Zip으로 두 리스트를 병합 
                    {
                        streamWriter.WriteLine(subItems.Item1 + " | " + subItems.Item2);
                    }

                    streamWriter.Flush();
                    streamWriter.Close();
                    fileStream.Close();
                }
            }
        }
    }
}
