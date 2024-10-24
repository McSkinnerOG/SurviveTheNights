using SurviveTheNights.Player;
using SurviveTheNights.Render;
using UnityEngine;
using UnityEngine.SceneManagement;

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
      Main.CurrentSceneName = SceneManager.GetActiveScene().name.ToLowerInvariant();
      if(Main.CurrentSceneName != "menuscene_farm") return;
      if(ShowMenu) { WindowRectMenu = GUI.Window(0, WindowRectMenu, DrawModMenuWindow, "Mod Menu"); }
      if(ShowMenu && ShowMenuEsp) { WindowRectEsp = GUI.Window(1, WindowRectEsp, DrawEspMenuWindow, "ESP Menu"); }
      if(ShowMenu && ShowMenuMovement) { WindowRectMovement = GUI.Window(2, WindowRectMovement, DrawMovementMenuWindow, "Movement"); }
      if(ShowMenu && ShowMenuPlayer) { WindowRectPlayer = GUI.Window(3, WindowRectPlayer, DrawPlayerMenuWindow, "Player"); }
      if(Main.B_Crosshair) { Crosshair.RenderCrosshair(1, false); }
      if(ESP.BEnabled) { ESP.RenderNetViews(); }
      if(ESP.BEspStorage) { ESP.RenderStorageESP(); }
    }
    public void DrawModMenuWindow(int windowID)
    {
      ShowMenuEsp = GUILayout.Toggle(ShowMenuEsp, "ESP MENU");
      ShowMenuMovement = GUILayout.Toggle(ShowMenuMovement, "MOVEMENT");
      ShowMenuPlayer = GUILayout.Toggle(ShowMenuPlayer, "PLAYER");
      Main.B_Crosshair = GUILayout.Toggle(Main.B_Crosshair, "CROSSHAIR");
      GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }
    public void DrawEspMenuWindow(int windowID)
    {
      ESP.BEnabled = GUILayout.Toggle(ESP.BEnabled, "ESP");
      ESP.BEspStorage = GUILayout.Toggle(ESP.BEspStorage, "ESP STORAGE");
      ESP.BDraw3DBox = GUILayout.Toggle(ESP.BDraw3DBox, "3D Box");
      ESP.BDrawDistance = GUILayout.Toggle(ESP.BDrawDistance, "Distance");
      ESP.BDrawPosition = GUILayout.Toggle(ESP.BDrawPosition, "Position");
      ESP.BDrawTracers = GUILayout.Toggle(ESP.BDrawTracers, "Tracers");
      ESP.BDrawFacingDirection = GUILayout.Toggle(ESP.BDrawFacingDirection, "Facing");
      ESP.BEspAnimal = GUILayout.Toggle(ESP.BEspAnimal, "Animals");
      if(ESP.BEspAnimal)
      {
        GUILayout.Label("Animals: " + ESP.FEspDistanceAnimals.ToString("F1"));
        ESP.FEspDistanceAnimals = GUILayout.HorizontalSlider(ESP.FEspDistanceAnimals, 1f, 1000f);
      }
      ESP.BEspItem = GUILayout.Toggle(ESP.BEspItem, "Items");
      if(ESP.BEspItem)
      {
        GUILayout.Label("Items: " + ESP.FEspDistanceItems.ToString("F1"));
        ESP.FEspDistanceItems = GUILayout.HorizontalSlider(ESP.FEspDistanceItems, 1f, 1000f);
      }
      ESP.BEspPlayer = GUILayout.Toggle(ESP.BEspPlayer, "Players");
      if(ESP.BEspPlayer)
      {
        GUILayout.Label("Players: " + ESP.FEspDistancePlayers.ToString("F1"));
        ESP.FEspDistancePlayers = GUILayout.HorizontalSlider(ESP.FEspDistancePlayers, 1f, 2000f);
      }
      ESP.BEspVehicle = GUILayout.Toggle(ESP.BEspVehicle, "Vehicles");
      if(ESP.BEspVehicle)
      {
        GUILayout.Label("Vehicles: " + ESP.FEspDistanceVehicles.ToString("F1"));
        ESP.FEspDistanceVehicles = GUILayout.HorizontalSlider(ESP.FEspDistanceVehicles, 1f, 2000f);
      }
      ESP.BEspWeapon = GUILayout.Toggle(ESP.BEspWeapon, "Weapons");
      if(ESP.BEspWeapon)
      {
        GUILayout.Label("Weapons: " + ESP.FEspDistanceWeapons.ToString("F1"));
        ESP.FEspDistanceWeapons = GUILayout.HorizontalSlider(ESP.FEspDistanceWeapons, 1f, 1000f);
      }
      ESP.BEspZombie = GUILayout.Toggle(ESP.BEspZombie, "Zombies");
      if(ESP.BEspZombie)
      {
        GUILayout.Label("Zombies: " + ESP.FEspDistanceZombies.ToString("F1"));
        ESP.FEspDistanceZombies = GUILayout.HorizontalSlider(ESP.FEspDistanceZombies, 1f, 1000f);
      }
      GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }
    public void DrawMovementMenuWindow(int windowID)
    {
      Main.B_NoSlow = GUILayout.Toggle(Main.B_NoSlow, "NOSLOWDOWN");
      Main.B_SpeedHack = GUILayout.Toggle(Main.B_SpeedHack, "SPEED CONFIG");
      if(Main.B_SpeedHack)
      {
        GUILayout.Label("Speed Forward: " + Speed.MaxSprintSpeed.ToString("F1"));
        Speed.MaxSprintSpeed = GUILayout.HorizontalSlider(Speed.MaxSprintSpeed, 1f, 50f);
        GUILayout.Label("Speed Side: " + Speed.SprintSidewaysSpeed.ToString("F1"));
        Speed.SprintSidewaysSpeed = GUILayout.HorizontalSlider(Speed.SprintSidewaysSpeed, 1f, 50f);
      }
      Main.B_JumpHack = GUILayout.Toggle(Main.B_JumpHack, "JUMP CONFIG");
      if(Main.B_JumpHack)
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
      Main.B_AntiJam = GUILayout.Toggle(Main.B_AntiJam, new GUIContent("ANTI-JAM"));
      Main.B_AvoidHordes = GUILayout.Toggle(Main.B_AvoidHordes, new GUIContent("AVOID-HORDES"));
      GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }
  }
}
