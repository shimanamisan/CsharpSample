using System;
using System.IO;
using System.Text;
using System.Windows;

namespace MVVM.SampleMahApps.Metro
{
    internal class FetchTextModel
    {
        private string _filePath;

        public FetchTextModel(string filePath)
        {
            this._filePath = filePath;
        }

        public string FetchTextData()
        {
            string text;

            try
            {
                // ファイルをオープンする
                using (StreamReader sr = new StreamReader(_filePath, Encoding.GetEncoding("UTF-8")))
                {
                    text = sr.ReadToEnd();
                }

                return text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                return string.Empty;
            }
        }
    }
}
