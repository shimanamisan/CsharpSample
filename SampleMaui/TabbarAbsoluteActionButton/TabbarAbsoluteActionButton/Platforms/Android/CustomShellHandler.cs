using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace TabbarAbsoluteActionButton
{
    internal class CustomShellHandler : ShellRenderer
    {
        protected override IShellItemRenderer CreateShellItemRenderer(ShellItem item)
        {
            return new CustomShellItemRenderer(this);
        }
    }
}
