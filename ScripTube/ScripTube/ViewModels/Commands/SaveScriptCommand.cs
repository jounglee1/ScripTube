using ScripTube.Models.YouTube;
using ScripTube.Utils;
using System;
using System.IO;
using System.Text;
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

        public SaveScriptCommand()
        {
            if (EXTENSION_FILTER == null)
            {
                initializeFilters();
            }
        }

        private delegate void SaveMethod(StreamWriter streamWriter, Video video, Subtitle subtitle);

        private static SaveMethod[] METHODS = new SaveMethod[]
        {
            new SaveMethod(saveAsTXT),
            new SaveMethod(saveAsSMI),
            new SaveMethod(saveAsSRT),
            new SaveMethod(saveAsHTML),
            new SaveMethod(saveAsTXT),
        };

        private static string[] EXTENSIONS = new string[]
        {
            ".txt",
            ".smi",
            ".srt",
            ".html",
            ".*",
        };

        private static string[] EXTENSION_NAMES = new string[]
        {
            "텍스트",
            "SMI",
            "SRT",
            "HTML",
            "모든",
        };

        private static string EXTENSION_FILTER;

        private int mLastFilterIndex;

        public bool CanExecute(object parameter)
        {
            var values = (object[])parameter;
            return values != null && values[0] != null && values[1] != null;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;
            var video = values[0] as Video;
            var subtitle = values[1] as Subtitle;

            if (video == null || subtitle == null)
            {
                return;
            }

            var saveFileDialog = new System.Windows.Forms.SaveFileDialog();

            saveFileDialog.Filter = EXTENSION_FILTER;
            saveFileDialog.FilterIndex = mLastFilterIndex;
            saveFileDialog.FileName = getFilenameExceptIllegalCharacter(video.Title);

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                mLastFilterIndex = saveFileDialog.FilterIndex;

                string ext = Path.GetExtension(saveFileDialog.FileName).ToLower();

                FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write);
                StreamWriter streamWriter = new StreamWriter(fileStream);

                SaveMethod saveMethod = saveAsTXT;
                for (int i = 0; i < METHODS.Length; ++i)
                {
                    if (EXTENSIONS[i] == ext)
                    {
                        saveMethod = METHODS[i];
                        break;
                    }
                }
                // 저장 메서드 호출
                saveMethod(streamWriter, video, subtitle);

                streamWriter.Flush();
                streamWriter.Close();
                fileStream.Close();
            }
        }

        private static void initializeFilters()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < EXTENSIONS.Length; ++i)
            {
                sb.Append(string.Format("{0} 파일 (*{1})|*{2}|", EXTENSION_NAMES[i], EXTENSIONS[i], EXTENSIONS[i]));
            }
            --sb.Length;
            EXTENSION_FILTER = sb.ToString();
        }

        private static void saveAsHTML(StreamWriter streamWriter, Video video, Subtitle subtitle)
        {
            streamWriter.WriteLine("<!DOCTYPE HTML>");
            streamWriter.WriteLine("<HTML>");
            streamWriter.WriteLine("<HEAD>");
            streamWriter.WriteLine("<TITLE>" + video.Title + "</TITLE>");
            streamWriter.WriteLine("</HEAD>");
            streamWriter.WriteLine("<BODY>");

            foreach (var item in subtitle.Items)
            {
                streamWriter.WriteLine("<P>[{0}] {1}</P>",
                    TimeFormatUtil.GetHHMMSSOrMMSS(item.StartSeconds, item.IsOneHourExcessed),
                    item.Text);
                streamWriter.WriteLine("\r\n");
            }

            streamWriter.WriteLine("</BODY>");
            streamWriter.WriteLine("</HTML>");
        }

        private static void saveAsSMI(StreamWriter streamWriter, Video video, Subtitle subtitle)
        {
            streamWriter.WriteLine("<SAMI>");
            streamWriter.WriteLine("<HEAD>");
            streamWriter.WriteLine("<TITLE>{0}</TITLE>", video.Title);
            streamWriter.WriteLine("</HEAD>");
            streamWriter.WriteLine("<BODY>");

            foreach (var item in subtitle.Items)
            {
                streamWriter.WriteLine("<SYNC Start={0}>", item.StartSeconds * 1000);
                streamWriter.WriteLine("<P Class=KRCC>{0}</P>", item.Text);
                streamWriter.Write("\r\n");
            }

            streamWriter.WriteLine("</BODY>");
            streamWriter.WriteLine("</SAMI>");
        }

        private static void saveAsSRT(StreamWriter streamWriter, Video video, Subtitle subtitle)
        {
            for (int i = 0; i < subtitle.Items.Count; i++)
            {
                var item = subtitle.Items[i];

                streamWriter.WriteLine(i);
                streamWriter.WriteLine("{0} --> {1}",
                    TimeFormatUtil.GetHHMMSSOrMMSSPrecision(item.StartSeconds, item.IsOneHourExcessed),
                    TimeFormatUtil.GetHHMMSSOrMMSSPrecision(item.StartSeconds + item.DurationSeconds, item.IsOneHourExcessed));
                streamWriter.WriteLine(subtitle.Items[i].Text);
                streamWriter.WriteLine();
            }
        }

        private static void saveAsTXT(StreamWriter streamWriter, Video video, Subtitle subtitle)
        {
            foreach (var item in subtitle.Items)
            {
                streamWriter.WriteLine("[{0}] {1}", TimeFormatUtil.GetHHMMSSOrMMSS(item.StartSeconds, item.IsOneHourExcessed), item.Text);
            }
        }

        private static string getFilenameExceptIllegalCharacter(string filename)
        {
            var sb = new StringBuilder(filename);
            char c = '+';
            sb.Replace('\\', c);
            sb.Replace('/', c);
            sb.Replace(':', c);
            sb.Replace('*', c);
            sb.Replace('?', c);
            sb.Replace('<', c);
            sb.Replace('>', c);
            sb.Replace('|', c);
            return sb.ToString();
        }
    }
}
