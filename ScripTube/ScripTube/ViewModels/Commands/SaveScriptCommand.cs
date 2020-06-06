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
            return values[0] != null && values[1] != null;
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

                // 등록되지 않은 확장자는 txt 저장 방식을 따른다
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
            var subItemText = new List<string>();
            var subItemTime = new List<string>();
            string videoTitle = video.Title;

            for (int i = 0; i < subtitle.Items.Count; i++)
            {
                subItemText.Add(subtitle.Items[i].Text);
                subItemTime.Add(subtitle.Items[i].StartTimeFormat);
            }

            streamWriter.WriteLine("<!DOCTYPE HTML>");
            streamWriter.WriteLine("<HTML>");
            streamWriter.WriteLine("<HEAD>");
            streamWriter.WriteLine("<TITLE>" + videoTitle + "</TITLE>");
            streamWriter.WriteLine("</HEAD>");
            streamWriter.WriteLine("<BODY>");

            for (int i = 0; i < subtitle.Items.Count; i++)
            {
                streamWriter.WriteLine("<P>[{0}] {1}</P>", subItemTime[i], subItemText[i]);
                streamWriter.WriteLine("\r\n");
            }

            streamWriter.WriteLine("</BODY>");
            streamWriter.WriteLine("</HTML>");
        }

        private static void saveAsSMI(StreamWriter streamWriter, Video video, Subtitle subtitle)
        {
            var subItemText = new List<string>();
            var subItemStartTime = new List<string>();
            string videoTitle = video.Title;

            for (int i = 0; i < subtitle.Items.Count; i++)
            {
                subItemText.Add(subtitle.Items[i].Text);
                subItemStartTime.Add((subtitle.Items[i].StartSeconds * 1000).ToString());
            }

            streamWriter.WriteLine("<SAMI>");
            streamWriter.WriteLine("<HEAD>");
            streamWriter.WriteLine("<TITLE>{0}</TITLE>", videoTitle);
            streamWriter.WriteLine("</HEAD>");
            streamWriter.WriteLine("<BODY>");


            for (int i = 0; i < subtitle.Items.Count; i++)
            {
                streamWriter.WriteLine("<SYNC Start={0}>", subItemStartTime[i]);
                streamWriter.WriteLine("<P Class=KRCC>{0}</P>", subItemText[i]);
                streamWriter.WriteLine("\r\n");
            }

            streamWriter.WriteLine("</BODY>");
            streamWriter.WriteLine("</SAMI>");
        }

        private static void saveAsSRT(StreamWriter streamWriter, Video video, Subtitle subtitle)
        {
            var subItemText = new List<string>();
            var subItemStartTime = new List<string>();
            var subItemEndTime = new List<string>();

            for (int i = 0; i < subtitle.Items.Count; i++)
            {
                subItemText.Add(subtitle.Items[i].Text);
                subItemStartTime.Add(subtitle.Items[i].StartTimeFormatSRT);
                subItemEndTime.Add(subtitle.Items[i].EndTimeFormatSRT);
            }

            for (int i = 0; i < subtitle.Items.Count; i++)
            {
                streamWriter.WriteLine(i);
                streamWriter.WriteLine("{0} --> {1}", subItemStartTime[i], subItemEndTime[i]);
                streamWriter.WriteLine(subItemText[i]);
            }
        }

        private static void saveAsTXT(StreamWriter streamWriter, Video video, Subtitle subtitle)
        {
            var subItemText = new List<string>();
            var subItemTime = new List<string>();

            for (int i = 0; i < subtitle.Items.Count; i++)
            {
                subItemText.Add(subtitle.Items[i].Text);
                subItemTime.Add(subtitle.Items[i].StartTimeFormat);
            }

            for (int i = 0; i < subtitle.Items.Count; i++)
            {
                streamWriter.WriteLine("[{0}] {1}", subItemTime[i], subItemText[i]);
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
