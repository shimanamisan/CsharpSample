using System.Windows.Input;
using Maui.BindableProperty.Generator.Core;

namespace TabbarAbsoluteActionButton
{
    public partial class CustomTabBar : TabBar
    {
        [AutoBindable]
        private ICommand? customActionButtonCommand;

        [AutoBindable]
        private string? customActionButtonText;

        [AutoBindable]
        private bool customActionButtonVisible;

        [AutoBindable]
        public Color? customActionButtonBackgroundColor;

        [AutoBindable]
        private double customActionButtonTextSize;
    }
}
