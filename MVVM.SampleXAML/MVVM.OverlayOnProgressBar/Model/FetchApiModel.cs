using MVVM.OverlayOnProgressBar.Object;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MVVM.OverlayOnProgressBar.Model
{
    public class FetchApiModel
    {
        // INFO: https://qiita.com/TsuyoshiUshio@github/items/b2d23b37b410a2cfd330
        // INFO: https://gist.github.com/devlights/f2b04da2ba46f3e2e52e

        /// <summary>
        /// ダミーAPI
        /// </summary>
        /// <param name="progress">進捗通知クラス</param>
        /// <param name="cancelToken">キャンセル通知クラス</param>
        /// INFO: IProgress -> 進行状況の更新のプロバイダーを定義
        public async Task FetchApiEndPoint(IProgress<ProgressInfo> progress, CancellationTokenSource cancelToken)
        {

            for (int i = 0; i <= 100; i++)
            {
                // CancellationTokenSource.Tokenプロパティ: この CancellationToken に関連付けられている CancellationTokenSource を取得します。
                // ThrowIfCancellationRequestedメソッド： このトークンに対して取り消しが要求された場合、OperationCanceledException をスローします。
                cancelToken.Token.ThrowIfCancellationRequested();

                // 状況通知メソッド Report() に、現在の進捗状況を格納したインスタンスを渡す
                progress.Report(new ProgressInfo(i));

                // 進捗率を程よく遅らせる
                await Task.Delay(30);

            }
        }
    }

}
