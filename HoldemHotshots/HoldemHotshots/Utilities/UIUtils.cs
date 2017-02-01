using System;
using System.Collections.Generic;
using Urho.Gui;

namespace HoldemHotshots
{
	public static class UIUtils
	{
		private static void enableAndShow(UIElement element)
		{
			element.Visible = true;
			element.Enabled = true;
		}

		private static void disableAndHide(UIElement element)
		{
			element.Visible = false;
			element.Enabled = false;
		}

		//UI switching
		public static void ShowUI(List<UIElement> uiCollection) { foreach (var uiElement in uiCollection) UIUtils.enableAndShow(uiElement); }
		public static void HideUI(List<UIElement> uiCollection) { foreach (var uiElement in uiCollection) UIUtils.disableAndHide(uiElement); }
		public static void SwitchUI(List<UIElement> from, List<UIElement> to) { HideUI(from); ShowUI(to); }
	}
}
