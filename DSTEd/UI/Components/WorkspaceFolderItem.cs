﻿using DSTEd.Core.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DSTEd.UI.Components
{
	class WorkspaceFolderItem : WorkspaceFSItem
	{
		protected FileNode file = null;

		public WorkspaceFolderItem(FileNode file) : base(file.Path)
		{
			this.file = file;
			Header = file.Name;
			FontWeight = FontWeights.Bold;
		}
	}
}
