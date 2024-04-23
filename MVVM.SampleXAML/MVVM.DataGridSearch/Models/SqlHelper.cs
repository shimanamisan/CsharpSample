using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace MVVM.DataGridSearch.Models
{
    /// <summary>
    /// データベース接続用のヘルパークラス
    /// </summary>
    internal static class SqlHelper
    {
        /// <summary>
        /// 接続文字列
        /// </summary>
        internal static string _connectionString;

        /// <summary>
        /// static なコンストラクター
        /// このクラスの _connectionString などのメンバー変数にアクセスしようとしたときに最初に実行される
        /// </summary>
        static SqlHelper()
        {

            var sourceDirectory = ConfigurationManager.AppSettings.Get("SQLiteFilePath");

            if (!DirectoryAndFileHelper.IsExistsDirectory(sourceDirectory))
            {
                throw new FileNotFoundException("SQLiteが格納されたディレクトリが存在しません");
            }

            var sourceDilePath = Path.Combine(sourceDirectory, "DataGridSearch.db");

            if (!DirectoryAndFileHelper.IsExistsFile(sourceDilePath))
            {
                throw new FileNotFoundException("SQLiteのファイルが存在しません");
            }

            var builder = new SqlConnectionStringBuilder();

            builder.DataSource = sourceDilePath;

            // 接続文字列を生成
            _connectionString = builder.ToString();

        }

        internal static IEnumerable<T> Query<T>(string sql)
        {
            return Query<T>(sql, null);
        }

        internal static IEnumerable<T> ParamQuery<T>(string sql, object param)
        {
            return Query<T>(sql, param);
        }

        internal static IEnumerable<T> Query<T>(string sql, object param)
        {
           
            using (var connection = new SqliteConnection(_connectionString))
            {
                // IEnumerable を List に変換する
                // Dapperの機能でSQL文の実行結果を T に指定するエンティティにマッピングをする
                // SELECTのカラム数とエンティティの引数の値、型など揃っていないとエラーになるので注意
                // INFO: https://learn.microsoft.com/ja-jp/dotnet/framework/data/adonet/sql-server-data-type-mappings

                return connection.Query<T>(sql, param);
            }
        }

    }
}
