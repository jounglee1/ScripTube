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
    class SaveScriptAsSMICommand : ICommand
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
            if (viewModel == null)
            {
                return;
            }

            var subtitle = viewModel.SelectedSubtitle as Subtitle;
            var video = viewModel.TargetVideo as Video;

            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.DefaultExt = "*.smi";
            saveFileDialog.Filter = "SMI 파일 (*.smi)|*.smi|모든 파일(*.*)|*.*";

            var subItemText = new List<string>();
            var subItemStartTime = new List<string>();
            string videoTitle = video.Title;

            for (int i = 0; i < subtitle.Items.Count; i++)
            {
                subItemText.Add(subtitle.Items[i].Text);
                subItemStartTime.Add((subtitle.Items[i].StartSeconds*1000).ToString());
            }

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write);
                StreamWriter streamWriter = new StreamWriter(fileStream);

                streamWriter.WriteLine("<SAMI>");
                streamWriter.WriteLine("<HEAD>");
                streamWriter.WriteLine("<TITLE>" + videoTitle + "</TITLE>");
                streamWriter.WriteLine("</HEAD>");
                streamWriter.WriteLine("<BODY>");


                for (int i = 0; i < subtitle.Items.Count; i++)
                {
                    streamWriter.WriteLine("<SYNC Start=" + subItemStartTime[i] + ">");
                    streamWriter.WriteLine("<P Class=KRCC>" + subItemText[i] + "</P>");
                    streamWriter.WriteLine("\r\n");
                }

                streamWriter.WriteLine("</BODY>");
                streamWriter.WriteLine("</SAMI>");

                streamWriter.Flush();
                streamWriter.Close();
                fileStream.Close();
            }
        }
    }
}

