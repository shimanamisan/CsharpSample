using CoreGraphics;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;
using UIKit;

namespace TabbarAbsoluteActionButton
{
    internal class CustomShellItemRenderer : ShellItemRenderer
    {
        private UIButton? _middleView;
        private CustomTabBar? _tabBar;
        private nfloat _screenWidth;
        private nfloat _screenHeight;
        private nfloat _buttonSize;

        public CustomShellItemRenderer(IShellContext context) : base(context)
        { }

        public override async void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();
            if (View is null || ShellItem is not CustomTabBar { CustomActionButtonVisible: true } tabBar)
            {
                return;
            }

            _tabBar = tabBar;

            GetScreenSize();
            CreateOrUpdateMiddleView();
            PositionMiddleView();
            View.AddSubview(_middleView!);
        }

        /// <summary>
        /// 画面のサイズを取得
        /// </summary>
        private void GetScreenSize()
        {
            _screenWidth = UIScreen.MainScreen.Bounds.Width;
            _screenHeight = UIScreen.MainScreen.Bounds.Height;
            _buttonSize = _screenWidth * 0.15f;
        }

        /// <summary>
        /// Vuewを作成または更新する
        /// </summary>
        private void CreateOrUpdateMiddleView()
        {
            if (_middleView != null)
            {
                _middleView.RemoveFromSuperview();
            }
            else
            {
                CreateMiddleView();
            }
            UpdateMiddleViewProperties();
        }

        /// <summary>
        /// ボタンを作成する
        /// </summary>
        private void CreateMiddleView()
        {
            _middleView = new UIButton(UIButtonType.Custom);
            _middleView.AutoresizingMask = UIViewAutoresizing.FlexibleRightMargin |
                                           UIViewAutoresizing.FlexibleLeftMargin |
                                           UIViewAutoresizing.FlexibleBottomMargin;
            _middleView.Layer.MasksToBounds = false;
            _middleView.TouchUpInside += (_, _) => _tabBar!.CustomActionButtonCommand?.Execute(null);
        }

        /// <summary>
        /// Viewのプロパティを更新
        /// </summary>
        private void UpdateMiddleViewProperties()
        {
            _middleView!.BackgroundColor = _tabBar!.CustomActionButtonBackgroundColor?.ToPlatform();
            _middleView.Frame = new CGRect(CGPoint.Empty, new CGSize(_buttonSize, _buttonSize));
            _middleView.Layer.CornerRadius = _middleView.Frame.Width / 2;
            _middleView.SetTitle(_tabBar.CustomActionButtonText ?? "+", UIControlState.Normal);
            _middleView.SetTitleColor(UIColor.White, UIControlState.Normal);
        }

        /// <summary>
        /// Viewの位置を設定
        /// </summary>
        private void PositionMiddleView()
        {
            var bottomMargin = _screenHeight * 0.05f;
            var rightMargin = _screenWidth * 0.05f;

            // 画面の右下に来るように設定
            _middleView!.Frame = new CGRect(
                _screenWidth - _buttonSize - rightMargin,
                _screenHeight - _buttonSize - bottomMargin,
                _buttonSize,
                _buttonSize
            );
        }
    }
}
