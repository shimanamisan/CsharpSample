using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Catching.Unhandled.Exceptions
{
    public static class BackgroundWorker
    {
        /// <summary>
        /// タイマー
        /// </summary>
        public static Stopwatch _stopwatch;

        /// <summary>
        /// Staticコンストラクタ
        /// </summary>
        static BackgroundWorker()
        {
            _stopwatch = new Stopwatch();
        }

        /// <summary>
        /// スタート
        /// </summary>
        public static void Start()
        {
            _stopwatch.Start();
        }

        /// <summary>
        /// ストップ
        /// </summary>
        public static void Stop()
        {
            _stopwatch.Stop();
        }
    }
}
