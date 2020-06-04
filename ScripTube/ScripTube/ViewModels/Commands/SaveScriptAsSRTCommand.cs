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
    class SaveScriptAsSRTCommand : ICommand
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

        public SaveScriptAsSRTCommand(MainWindowViewModel viewModel)
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
                System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
                saveFileDialog.DefaultExt = "*.srt";
                saveFileDialog.Filter = "SRT 파일 (*.srt)|*.srt|모든 파일(*.*)|*.*";

                var subItemText = new List<string>();
                var subItemStartTime = new List<string>();
                var subItemEndTime = new List<string>();

                for (int i = 0; i < subtitle.Items.Count; i++)
                {
                    subItemText.Add(subtitle.Items[i].Text);
                    subItemStartTime.Add(subtitle.Items[i].StartTimeFormatSRT);
                    subItemEndTime.Add(subtitle.Items[i].EndTimeFormatSRT);
                }

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write);
                    StreamWriter streamWriter = new StreamWriter(fileStream);


                    for (int i = 0; i < subtitle.Items.Count; i++)
                    {
                        streamWriter.WriteLine(i);
                        streamWriter.WriteLine(subItemStartTime[i] + " --> " + subItemEndTime[i]);
                        streamWriter.WriteLine(subItemText[i]);
                    }


                    streamWriter.Flush();
                    streamWriter.Close();
                    fileStream.Close();
                }
            }

        }
    }
}

