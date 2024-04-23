using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Csharp.SQLiteMigrator;
using System.Configuration;

static IServiceProvider CreateServices(string sourcePath)
{
    return new ServiceCollection()
        // 共通のFluentMigratorサービスを追加する
        .AddFluentMigratorCore()
        .ConfigureRunner(rb => rb
            // FluentMigrator に SQLite サポートを追加する。
            .AddSQLite()
            // 接続文字列を設定する
            .WithGlobalConnectionString($"Data Source={sourcePath}")
            // マイグレーションを含むアセンブリを定義する
            .ScanIn(typeof(UsersTable).Assembly).For.Migrations())
        // FluentMigrator の方法でコンソールへのロギングを有効にする。
        .AddLogging(lb => lb.AddFluentMigratorConsole())
        // サービス・プロバイダーの構築
        .BuildServiceProvider(false);
}

static void CopySQLiteResource()
{

    // ソースファイルのパス
    string sourceFile = AppDomain.CurrentDomain.BaseDirectory + "SqLite.db";

    // コピー先のパス
    string destinationFile = Path.Combine(ConfigurationManager.AppSettings.Get("OutputPath"), "DataGridSearch.db");

    if (!Directory.Exists(ConfigurationManager.AppSettings.Get("OutputPath")))
    {
        Directory.CreateDirectory(ConfigurationManager.AppSettings.Get("OutputPath"));
    }

    try
    {
        // コピー元のファイルが存在するか確認
        if (File.Exists(sourceFile))
        {
            // ファイルをコピー
            // 第三引数の true は、目的のファイルが存在する場合に上書きすることを意味する
            File.Copy(sourceFile, Path.Combine(destinationFile), true);

        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("ファイルが見つかりませんでした。");
    }

}

var dataSource = AppDomain.CurrentDomain.BaseDirectory + "SqLite.db";

var serviceProvider = CreateServices(dataSource);

using (var scope = serviceProvider.CreateScope())
{
    var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

    Console.WriteLine("Please specify a command: 'up' or 'down'.");

    // コンソール画面でユーザーからの入力を待つ
    var input = Console.ReadLine();

    switch (input)
    {
        case "up":
            runner.MigrateUp();
            break;

        case "down":
            runner.MigrateDown(0);
            break;

        default:
            Console.WriteLine("Invalid command. Please use 'up' or 'down'.");
            break;
    }

}

CopySQLiteResource();