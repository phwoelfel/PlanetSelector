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
		private ApplicationLauncherButton appLauncherButton = null;

		private PlanetSelectorWindow psWindow;

		public void Start()
		{
			Log("start");
			KSPSettings.load();

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

			if (KSPSettings.get("showAppLauncher", true))
			{
				GameEvents.onGUIApplicationLauncherReady.Add(onGUIAppLauncherReady);
				GameEvents.onGUIApplicationLauncherDestroyed.Add(onGUIAppLauncherDestroyed);
			}
		}

		private void onGUIAppLauncherDestroyed()
		{
			if (appLauncherButton != null)
			{
				ApplicationLauncher.Instance.RemoveModApplication(appLauncherButton);
			}
		}


		private void onGUIAppLauncherReady()
		{
			if (appLauncherButton == null)
			{
				Texture2D btnTexture = new Texture2D(38, 38);
				btnTexture.LoadImage(System.IO.File.ReadAllBytes("GameData/PlanetSelector/icons/ps_app_button.png"));

				appLauncherButton = ApplicationLauncher.Instance.AddModApplication(
					onAppLaunchToggleOn, onAppLaunchToggleOff,
					onAppLaunchHoverOn, onAppLaunchHoverOff,
					null, null,
					ApplicationLauncher.AppScenes.MAPVIEW,
					(Texture)btnTexture);
			}
		}

		private void onAppLaunchHoverOn()
		{
			psWindow.windowHover = true;
		}

		private void onAppLaunchHoverOff()
		{
			psWindow.windowHover = false;
		}

		private void onAppLaunchToggleOn()
		{
			psWindow.windowVisible = !psWindow.windowVisible;
		}

		private void onAppLaunchToggleOff()
		{
			psWindow.windowVisible = !psWindow.windowVisible;
		}
		
		void OnDestroy()
		{
			Log("destroy");

			if (ToolbarManager.ToolbarAvailable)
			{
				psButton.Destroy();
			}

			if (KSPSettings.get("showAppLauncher", true))
			{
				if (appLauncherButton != null)
				{
					Log("removing app launcher button");
					ApplicationLauncher.Instance.RemoveModApplication(appLauncherButton);
				}
				GameEvents.onGUIApplicationLauncherDestroyed.Remove(onGUIAppLauncherDestroyed);
				GameEvents.onGUIApplicationLauncherReady.Remove(onGUIAppLauncherReady);
			}

		}
    }
}
