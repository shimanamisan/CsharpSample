using Dapper;
using MVVM.DataGridPagination.Entities;
using MVVM.DataGridPagination.Infrastructure.Helpers;
using MVVM.DataGridPagination.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVVM.DataGridPagination.Infrastructure.Implements
{
    public sealed class Users : IUsersRepository<UserEntity>
    {
        /// <summary>
        /// ユーザーをページネーションして非同期的に取得する
        /// </summary>
        /// <param name="pageNumber">取得するページ番号（1から始まる）</param>
        /// <param name="pageSize">1ページあたりに取得するユーザー数</param>
        /// <returns>ユーザーエンティティ</returns>
        public async Task<IEnumerable<UserEntity>> GetPaginateAsync(int pageNumber, int pageSize)
        {
            var parameters = new DynamicParameters();

            // @PageSize や @Offset: パラメータ（プレースホルダー）
            // LIMIT @PageSize: 指定された pageSize 分の数のデータを返す
            // OFFSET @Offset: 指定された offset 分のデータを飛ばす
            // ※pageSize は常に500が渡ってくる
            var sql = @"SELECT * FROM Users ORDER BY Id LIMIT @PageSize OFFSET @Offset";

            // データを取得する位置を計算する
            // 例: (2 - 1) * 500 = 500;
            // 　→ 500行分のデータを飛ばして501行目からデータを取得する
            var offset = (pageNumber - 1) * pageSize;

            // パラメータにバインドする値を指定
            parameters.Add("@PageSize", pageSize);
            parameters.Add("@Offset", offset);

            // クエリ文を実行
            return await SqlHelper.Query<UserEntity>(sql, parameters);
        }

        /// <summary>
        /// 全てのユーザーを非同期的に取得する
        /// </summary>
        /// <returns>ユーザーエンティティ</returns>
        public async Task<IEnumerable<UserEntity>> GetUsersAsync()
        {
            var sql = @"SELECT * FROM Users";

            return await SqlHelper.Query<UserEntity>(sql);
        }
    }
}
