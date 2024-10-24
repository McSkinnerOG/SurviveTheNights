using SurviveTheNights.Modules.Combat;
using SurviveTheNights.Modules.Movement;
using SurviveTheNights.Modules.Player;
using SurviveTheNights.Modules.Render;
using UnityEngine;

namespace SurviveTheNights
{
  public class MenuManager : MonoBehaviour
  {
    // WindowRects
    public static Rect WindowRectMenu = new(20, 20, 250, 350);
    public static Rect WindowRectMovement = new(20, 20, 250, 350);
    public static Rect WindowRectPlayer = new(20, 20, 250, 350);
    public static Rect WindowRectEsp = new(200, 20, 250, 650);
    // Menu Toggles
    public static bool ShowMenu = false;
    public static bool ShowMenuEsp = false;
    public static bool ShowMenuPlayer = false;
    public static bool ShowMenuMovement = false;

    public void OnGUI()
    {
      if(Main.CurrentSceneName == "menuscene_farm") return;
      if(ShowMenu) { WindowRectMenu = GUI.Window(0, WindowRectMenu, DrawModMenuWindow, "Mod Menu"); }
      if(ShowMenu && ShowMenuEsp) { WindowRectEsp = GUI.Window(1, WindowRectEsp, DrawEspMenuWindow, "ESP Menu"); }
      if(ShowMenu && ShowMenuMovement) { WindowRectMovement = GUI.Window(2, WindowRectMovement, DrawMovementMenuWindow, "Movement"); }
      if(ShowMenu && ShowMenuPlayer) { WindowRectPlayer = GUI.Window(3, WindowRectPlayer, DrawPlayerMenuWindow, "Player"); }
      if(Crosshair.Enabled) { Crosshair.RenderCrosshair(1, false); }

    }
    public void DrawModMenuWindow(int windowID)
    {
      ShowMenuEsp = GUILayout.Toggle(ShowMenuEsp, "ESP MENU");
      ShowMenuMovement = GUILayout.Toggle(ShowMenuMovement, "MOVEMENT");
      ShowMenuPlayer = GUILayout.Toggle(ShowMenuPlayer, "PLAYER");
      Crosshair.Enabled = GUILayout.Toggle(Crosshair.Enabled, "CROSSHAIR");
      GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }
    public void DrawEspMenuWindow(int windowID)
    {
      ESP.Enabled = GUILayout.Toggle(ESP.Enabled, "ESP");
      ESP.EspStorage = GUILayout.Toggle(ESP.EspStorage, "ESP STORAGE");
      ESP.Draw3DBox = GUILayout.Toggle(ESP.Draw3DBox, "3D Box");
      ESP.DrawDistance = GUILayout.Toggle(ESP.DrawDistance, "Distance");
      ESP.DrawPosition = GUILayout.Toggle(ESP.DrawPosition, "Position");
      ESP.DrawTracers = GUILayout.Toggle(ESP.DrawTracers, "Tracers");
      ESP.DrawFacingDirection = GUILayout.Toggle(ESP.DrawFacingDirection, "Facing");
      ESP.EspAnimal = GUILayout.Toggle(ESP.EspAnimal, "Animals");
      if(ESP.EspAnimal)
      {
        GUILayout.Label("Animals: " + ESP.EspDistanceAnimals.ToString("F1"));
        ESP.EspDistanceAnimals = GUILayout.HorizontalSlider(ESP.EspDistanceAnimals, 1f, 500f);
      }
      ESP.EspItem = GUILayout.Toggle(ESP.EspItem, "Items");
      if(ESP.EspItem)
      {
        GUILayout.Label("Items: " + ESP.EspDistanceItems.ToString("F1"));
        ESP.EspDistanceItems = GUILayout.HorizontalSlider(ESP.EspDistanceItems, 1f, 500f);
      }
      ESP.EspPlayer = GUILayout.Toggle(ESP.EspPlayer, "Players");
      if(ESP.EspPlayer)
      {
        GUILayout.Label("Players: " + ESP.EspDistancePlayers.ToString("F1"));
        ESP.EspDistancePlayers = GUILayout.HorizontalSlider(ESP.EspDistancePlayers, 10f, 5000f);
      }
      ESP.EspVehicle = GUILayout.Toggle(ESP.EspVehicle, "Vehicles");
      if(ESP.EspVehicle)
      {
        GUILayout.Label("Vehicles: " + ESP.EspDistanceVehicles.ToString("F1"));
        ESP.EspDistanceVehicles = GUILayout.HorizontalSlider(ESP.EspDistanceVehicles, 1f, 2000f);
      }
      ESP.EspWeapon = GUILayout.Toggle(ESP.EspWeapon, "Weapons");
      if(ESP.EspWeapon)
      {
        GUILayout.Label("Weapons: " + ESP.EspDistanceWeapons.ToString("F1"));
        ESP.EspDistanceWeapons = GUILayout.HorizontalSlider(ESP.EspDistanceWeapons, 1f, 500f);
      }
      ESP.EspZombie = GUILayout.Toggle(ESP.EspZombie, "Zombies");
      if(ESP.EspZombie)
      {
        GUILayout.Label("Zombies: " + ESP.EspDistanceZombies.ToString("F1"));
        ESP.EspDistanceZombies = GUILayout.HorizontalSlider(ESP.EspDistanceZombies, 1f, 500f);
      }
      GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }
    public void DrawMovementMenuWindow(int windowID)
    {
      Main.NoSlow = GUILayout.Toggle(Main.NoSlow, "NOSLOWDOWN");
      Speed.Enabled = GUILayout.Toggle(Speed.Enabled, "SPEED CONFIG");
      if(Speed.Enabled)
      {
        GUILayout.Label("Speed Forward: " + Speed.MaxSprintSpeed.ToString("F1"));
        Speed.MaxSprintSpeed = GUILayout.HorizontalSlider(Speed.MaxSprintSpeed, 1f, 50f);
        GUILayout.Label("Speed Side: " + Speed.SprintSidewaysSpeed.ToString("F1"));
        Speed.SprintSidewaysSpeed = GUILayout.HorizontalSlider(Speed.SprintSidewaysSpeed, 1f, 50f);
      }
      JumpHacks.Enabled = GUILayout.Toggle(JumpHacks.Enabled, "JUMP CONFIG");
      if(JumpHacks.Enabled)
      {
        GUILayout.Label("Height Base: " + JumpHacks.BaseHeight.ToString("F1"));
        JumpHacks.BaseHeight = GUILayout.HorizontalSlider(JumpHacks.BaseHeight, 1f, 50f);
        GUILayout.Label("Height Extra: " + JumpHacks.ExtraHeight.ToString("F1"));
        JumpHacks.ExtraHeight = GUILayout.HorizontalSlider(JumpHacks.ExtraHeight, 1f, 50f);
      }
      GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }
    public void DrawPlayerMenuWindow(int windowID)
    {
      AntiJam.Enabled = GUILayout.Toggle(AntiJam.Enabled, new GUIContent("ANTI-JAM"));
      AvoidHorde.Enabled = GUILayout.Toggle(AvoidHorde.Enabled, new GUIContent("AVOID-HORDES"));
      GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }
  }
}
