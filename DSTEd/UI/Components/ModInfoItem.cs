using System.Windows.Controls;

namespace DSTEd.UI.Components {
    class ModInfoItem : WorkspaceFileItem {
		public ModInfoItem(string FullPath, System.Windows.Input.MouseButtonEventHandler CustomPreviewCallback = null) : base(FullPath, CustomPreviewCallback) { }
	}
}
