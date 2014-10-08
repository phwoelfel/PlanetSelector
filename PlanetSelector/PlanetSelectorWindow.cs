using PluginBaseFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PlanetSelector
{
	class PlanetSelectorWindow : BaseWindow
	{
		private Dictionary<String, int> planets = new Dictionary<String, int>();
		private PlanetariumCamera pCam;

		public PlanetSelectorWindow()
			: base("Planet Selector", 200, 400)
		{

		}

		public void Start()
		{
			LogDebug("window start");
			windowPosition.x = KSPSettings.get("PlanetSelectorWindow.x", (int)(Screen.width / 2 - windowWidth / 2));
			windowPosition.y = KSPSettings.get("PlanetSelectorWindow.y", (int)(Screen.height / 2 - windowHeight / 2));

			pCam = MapView.MapCamera;
			List<MapObject> targets = pCam.targets;

			foreach (MapObject mo in targets)
			{
				LogDebug("MapObject: " + mo.GetName() + " type: " + mo.type);

			}
		}

		protected override void preDrawGui()
		{

		}

		protected override void drawGui(int windowID)
		{
			GUILayout.BeginVertical();

			foreach (MapObject mo in pCam.targets)
			{
				if (GUILayout.Button(mo.GetName()))
				{
					if (!MapView.MapIsEnabled)
					{
						MapView.EnterMapView();
					}
					pCam.SetTarget(mo);
				}
			}
			
			GUILayout.EndVertical();
			GUI.DragWindow();
		}

		public void OnDestroy()
		{
			LogDebug("PlanetSelectorWindow destroy");
			saveSettings();
		}

		protected void saveSettings()
		{
			KSPSettings.set("PlanetSelectorWindow.x", (int)windowPosition.x);
			KSPSettings.set("PlanetSelectorWindow.y", (int)windowPosition.y);

			KSPSettings.save();
		}
	}
}
