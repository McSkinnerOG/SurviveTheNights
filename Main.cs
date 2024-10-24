using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SurviveTheNights.Player;
using SurviveTheNights.Render;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace SurviveTheNights
{
  [BepInPlugin(GUID, MODNAME, VERSION)]
  public class Main : BaseUnityPlugin
  {
    #region[Declarations] 
    public const string MODNAME = "SurviveTheNights", AUTHOR = "VALIDUSER", GUID = AUTHOR + "_" + MODNAME, VERSION = "1.0.0";
    internal readonly ManualLogSource Log;
    internal readonly Harmony Harmony;
    internal readonly Assembly Assembly;
    public readonly string ModFolder;
    #endregion
    public static bool Debug_MessageToConsole = true;
    public static bool Debug_MessageToIngame = true;
    public static string CurrentSceneName = "";
    #region [ HACK REQUIRED DECLARATIONS ]   
    public static bool B_AntiJam = false;
    public static bool B_AvoidHordes = false;
    public static bool B_NoSlow = false;
    public static bool B_SpeedHack = false;
    public static bool B_JumpHack = false;
    public static bool B_Crosshair = false;
    public static bool B_CrosshairCircle = false;
    public static bool B_NoFall = false;
    public static bool B_AutoCollect = false;
    #endregion
    public static bool BYPASS_PATCH_VELOCITY = false;         // DONE 
    public static bool BYPASS_RPC_SETPOS_FROM_SERVER = false; // DONE  
    public MenuManager MenuManager;
    public Main()
    {
      Log = Logger;
      Harmony = new Harmony(GUID);
      Assembly = Assembly.GetExecutingAssembly();
      ModFolder = Path.GetDirectoryName(Assembly.Location);
    }
    public void Start() { Harmony.PatchAll(Assembly); }
    public void Awake() { MenuManager = gameObject.AddComponent<MenuManager>(); }
    public void Update()
    {
      CurrentSceneName = SceneManager.GetActiveScene().name.ToLowerInvariant();
      if(CurrentSceneName is "menuscene_farm" or not "clientmanager") return;
      if(Input.GetKeyDown(KeyCode.F2)) { MenuManager.ShowMenu = !MenuManager.ShowMenu; }
      if(Input.GetKeyDown(KeyCode.Keypad0)) { B_SpeedHack = !B_SpeedHack; }
      if(Input.GetKeyDown(KeyCode.Keypad1)) { B_JumpHack = !B_JumpHack; JumpHacks.Toggle(B_JumpHack); }
      if(Input.GetKeyDown(KeyCode.Keypad2)) { B_NoSlow = !B_NoSlow; }
      if(Input.GetKeyDown(KeyCode.Keypad4)) { B_AntiJam = !B_AntiJam; }
      if(Input.GetKeyDown(KeyCode.Keypad5)) { B_Crosshair = !B_Crosshair; }
      if(Input.GetKeyDown(KeyCode.Keypad6))
      {
        foreach(var item in Utils.GetObjectsInRadius(1000).Where(x => x.gameObject.GetComponent<HarvestMachine>() != null))
        {
          item.gameObject.GetComponent<HarvestMachine>()?.TryHarvest();
        }
      }
      if(Input.GetKeyDown(KeyCode.Keypad7)) { }
      if(Input.GetKeyDown(KeyCode.Keypad8)) { ESP.BEspStorage = !ESP.BEspStorage; }
      if(Input.GetKeyDown(KeyCode.Keypad9)) { ESP.BEnabled = !ESP.BEnabled; }
      //AntiDrown.MoveDrownChecker();
      Speed.SpeedHack(B_SpeedHack);
      JumpHacks.Toggle(B_JumpHack);
    }
  }
}
