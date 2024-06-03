using DataGrid_Sample.Enitities;
using DataGrid_Sample.Repositories;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace DataGrid_Sample.Csv
{
    public sealed class CSVClient : IDataGridRepository
    {
        /// <summary>
        /// 現在処理中の行が1行目か判定する
        /// </summary>
        private bool _isFirstLine = true;

        /// <summary>
        /// アプリケーションが実行されているディレクトリを取得し、指定した文字列を結合させる
        /// </summary>
        private string _currentDomain = AppDomain.CurrentDomain.BaseDirectory;

        public ObservableCollection<UserEnitity> Get()
        {
            var entity = new ObservableCollection<UserEnitity>();

            int indexCount = 0;

            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using (var reader = new StreamReader(_currentDomain + "dummyData.csv", Encoding.GetEncoding("Shift_JIS")))
                {
                    // 1.whileで1行ずつ読み込む
                    // 2.最初の行はスキップする
                    // 3.split(",") でカンマを基準に各要素を配列に変換する
                    // 4.UserEntityに必要な要素だけインデックスを指定して取り出す

                    while (reader.Peek() >= 0)
                    {
                        var line = reader.ReadLine();

                        if (_isFirstLine)
                        {
                            _isFirstLine = false;

                            continue;
                        }

                        var strArray = line.Split(",");

                        indexCount++;

                        entity.Add(new UserEnitity(
                                    indexCount,
                                    strArray[0],
                                    strArray[3],
                                    strArray[4],
                                    strArray[5],
                                    strArray[7],
                                    strArray[8])
                            );
                    }

                    _isFirstLine = true;

                    return entity;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
