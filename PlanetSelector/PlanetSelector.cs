using PluginBaseFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PlanetSelector
{
	[KSPAddon(KSPAddon.Startup.Flight, false)]
    class PlanetSelector : PluginBase
    {
		private IButton psButton;

		private PlanetSelectorWindow psWindow;

		public void Start()
		{
			Log("start");
			Settings.load();

			psWindow = gameObject.AddComponent<PlanetSelectorWindow>();

			if (ToolbarManager.ToolbarAvailable)
			{
				LogDebug("add toolbar button");
				psButton = ToolbarManager.Instance.add("PS", "PSButton");
				psButton.TexturePath = "PlanetSelector/icons/ps_toolbar_button";
				psButton.ToolTip = "Planet Selector Window";
				psButton.OnClick += (e) =>
				{
					psWindow.windowVisible = !psWindow.windowVisible;
				};
			}
		}
		
		void OnDestroy()
		{
			Log("destroy");

			if (ToolbarManager.ToolbarAvailable)
			{
				psButton.Destroy();
			}
		}
    }
}
