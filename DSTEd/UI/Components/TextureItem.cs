using System.Windows.Controls;

namespace DSTEd.UI.Components {
    class TextureItem : WorkspaceFileItem {
		public TextureItem(string FullPath,System.Windows.Input.MouseButtonEventHandler CustomPreviewCallback = null) : base(FullPath, CustomPreviewCallback) { }
    }
}
