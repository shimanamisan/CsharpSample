using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;
using Button = Android.Widget.Button;
using View = Android.Views.View;

namespace TabbarAbsoluteActionButton
{
    internal class CustomShellItemRenderer : ShellItemRenderer
    {
        private CustomTabBar? _tabBar;
        private int _screenWidth;
        private int _screenHeight;

        public CustomShellItemRenderer(IShellContext shellContext) : base(shellContext)
        { }

        /// <summary>
        /// ShellItemのViewを作成
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns>作成されたView、またはカスタマイズが不要な場合は基本クラスのView</returns>
        public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            // カスタムTabBarではなく既存のTabBarを返す場合の条件
            if (Context is null || ShellItem is not CustomTabBar { CustomActionButtonVisible: true } tabBar)
            {
                return view;
            }

            _tabBar = tabBar;
            // TabBarのルートとなるレイアウトを作成
            var rootLayout = CreateRootLayout();
            rootLayout.AddView(view);

            // 画面幅を取得
            GetScreenSize();

            var middleView = CreateCustomActionButton();
            var backgroundView = CreateBackgroundView(middleView.LayoutParameters);

            if (backgroundView != null)
            {
                rootLayout.AddView(backgroundView);
            }
            rootLayout.AddView(middleView);

            return rootLayout;
        }

        /// <summary>
        /// ルートレイアウトとして機能する新しい FrameLayout を作成
        /// </summary>
        /// <returns>作成された FrameLayout インスタンス</returns>
        private FrameLayout CreateRootLayout()
        {
            return new FrameLayout(Context)
            {
                // LayoutParamsに幅と高さの値を指定
                LayoutParameters = new FrameLayout.LayoutParams(
                        ViewGroup.LayoutParams.MatchParent, // 親のViewと同じサイズになる定数を指定
                        ViewGroup.LayoutParams.MatchParent
                        )
            };
        }

        /// <summary>
        /// 画面のサイズを取得し、内部フィールドに保存
        /// </summary>
        private void GetScreenSize()
        {
            var displayMetrics = Context?.Resources?.DisplayMetrics;
            _screenWidth = displayMetrics?.WidthPixels ?? 0;
            _screenHeight = displayMetrics?.HeightPixels ?? 0;
        }

        /// <summary>
        /// カスタムアクションボタンを作成
        /// </summary>
        /// <returns>設定されたカスタムアクションボタン</returns>
        private Button CreateCustomActionButton()
        {
            // ボタンのサイズを画面幅の割合で指定（例: 画面幅の15%）
            int middleViewSize = (int)(_screenWidth * 0.15);
            var middleViewLayoutParams = CreateMiddleViewLayoutParams(middleViewSize);

            var middleView = new Button(Context)
            {
                LayoutParameters = middleViewLayoutParams,
                Text = _tabBar!.CustomActionButtonText ?? "+",
            };

            SetButtonBackground(middleView, middleViewSize);
            SetButtonTextProperties(middleView);
            SetButtonClickEvent(middleView);

            return middleView;
        }

        /// <summary>
        /// ボタンを表示するViewの位置を設定する
        /// </summary>
        /// <param name="size">ビューのサイズ</param>
        /// <returns>作成されたレイアウトパラメータ</returns>
        private FrameLayout.LayoutParams CreateMiddleViewLayoutParams(int size)
        {
            return new FrameLayout.LayoutParams(size,
                                                size,
                                                GravityFlags.Bottom | GravityFlags.End)
            {
                // マージンも画面サイズに対する割合で指定
                BottomMargin = (int)(_screenHeight * 0.05), // 画面高さの5%
                RightMargin = (int)(_screenWidth * 0.05) // 画面幅の5%
            };
        }

        /// <summary>
        /// ボタンの背景を設定します。
        /// </summary>
        /// <param name="button">設定対象のボタン</param>
        /// <param name="size">ボタンのサイズ</param>
        private void SetButtonBackground(Button button, int size)
        {
            var buttonBackgroundDrawable = new GradientDrawable();
            buttonBackgroundDrawable.SetColor(_tabBar!.CustomActionButtonBackgroundColor.ToPlatform(Colors.Transparent));
            buttonBackgroundDrawable.SetShape(ShapeType.Rectangle);
            buttonBackgroundDrawable.SetCornerRadius(size / 2f);
            button.SetBackground(buttonBackgroundDrawable);
        }

        /// <summary>
        /// ボタンのテキストプロパティを設定します。
        /// </summary>
        /// <param name="button">設定対象のボタン</param>
        private void SetButtonTextProperties(Button button)
        {
            button.SetTextColor(Android.Graphics.Color.White);
            if (_tabBar!.CustomActionButtonTextSize > 0)
            {
                button.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)_tabBar.CustomActionButtonTextSize);
            }
            button.SetPadding(0, 0, 0, 0);
        }

        /// <summary>
        /// ボタンのテキストプロパティを設定
        /// </summary>
        /// <param name="button">設定対象のボタン</param>
        private void SetButtonClickEvent(Button button)
        {
            button.Click += (_, _) => _tabBar!.CustomActionButtonCommand?.Execute(null);
        }

        /// <summary>
        /// 背景Viewを作成します。
        /// </summary>
        /// <param name="layoutParams">レイアウトパラメータ</param>
        /// <returns>作成された背景View、または null（背景色が設定されていない場合）</returns>
        private View? CreateBackgroundView(ViewGroup.LayoutParams layoutParams)
        {
            if (_tabBar!.CustomActionButtonBackgroundColor is null)
            {
                return null;
            }

            var backgroundView = new View(Context)
            {
                LayoutParameters = layoutParams
            };

            var backgroundDrawable = new GradientDrawable();
            backgroundDrawable.SetShape(ShapeType.Rectangle);
            backgroundDrawable.SetCornerRadius(((FrameLayout.LayoutParams)layoutParams).Width / 2f);
            backgroundDrawable.SetColor(_tabBar.CustomActionButtonBackgroundColor.ToPlatform(Colors.Transparent));
            backgroundView.SetBackground(backgroundDrawable);

            return backgroundView;
        }

    }
}
