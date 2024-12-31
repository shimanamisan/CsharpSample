using System.IO;

namespace MVVM.MultiWindowSample.Models
{
    internal static class DirectoryAndFileHelper
    {
        /// <summary>
        /// アプリケーションのベースディレクトリのパスを保持する静的文字列変数
        /// </summary>
        public static string BASE_DIRECTORY = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 指定されたパスにディレクトリが存在しない場合、ディレクトリを作成する。
        /// </summary>
        /// <param name="path">ディレクトリを作成するパス</param>
        internal static void CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 指定されたディレクトリが存在するか確認する
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <returns>真偽値</returns>
        internal static bool IsExistsDirectory(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// 指定されたファイルパスが存在するか確認する
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static bool IsExistsFile(string path)
        {
            return File.Exists(path);
        }

    }
}
