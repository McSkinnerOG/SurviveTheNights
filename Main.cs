using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SurviveTheNights.Modules.Combat;
using SurviveTheNights.Modules.Movement;
using SurviveTheNights.Modules.Player;
using SurviveTheNights.Modules.Render;
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
    public static bool NoSlow = false;
    public static bool CrosshairCircle = false;
    public static bool NoFall = false;
    public static bool AutoCollect = false;
    #endregion
    public static bool BYPASS_PATCH_VELOCITY = true;
    public static bool BYPASS_RPC_SETPOS_FROM_SERVER = true;
    public MenuManager MenuManager;
    public Main()
    {
      Log = Logger;
      Harmony = new Harmony(GUID);
      Assembly = Assembly.GetExecutingAssembly();
      ModFolder = Path.GetDirectoryName(Assembly.Location);
    }
    public void Start() { Harmony.PatchAll(Assembly); }
    public void Awake() { MenuManager = gameObject.AddComponent<MenuManager>(); CreateChamsMaterial(); }
    public static Shader CreateChamsMaterial()
    {
      var shader = Shader.Find("Hidden/Internal-Colored");
      ESP.Material = new Material(shader);
      //material.hideFlags = HideFlags.HideAndDontSave;
      // Turn on alpha blending
      ESP.Material.SetColor("_Color", new(1, 1, 1, 1));
      ESP.Material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
      ESP.Material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
      // Turn backface culling off
      ESP.Material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
      // Turn off depth writes
      ESP.Material.SetInt("_ZWrite", 0);
      // makes the material draw on top of everything
      ESP.Material.SetInt("_ZTest", 0);
      return shader;
    }
    public void Update()
    {
      CurrentSceneName = SceneManager.GetActiveScene().name.ToLowerInvariant();
      if(CurrentSceneName is "menuscene_farm" or not "clientmanager") return;
      if(Refs.LP_Owner == null) return;
      if(PlayerOwner.instance == null) return;
      if(Refs.LP_CharMotorDB == null) return;
      if(Refs.LP_CharMotorDB.movement == null) return;
      if(Input.GetKeyDown(KeyCode.Insert)) { MenuManager.ShowMenu = !MenuManager.ShowMenu; }
      if(Input.GetKeyDown(KeyCode.Keypad0))
      {
        foreach(var i in Utils.GetObjectsInRadius(100))
        {
          var h = i.gameObject.GetComponent<HarvestMachine>();
          var s = h.settings;
          s.isInfinite = true;
          s.isNotDistanceDependant = true;
          h.TryHarvest();
        }
      }
      if(Input.GetKeyDown(KeyCode.Keypad1)) { AntiJam.Enabled = !AntiJam.Enabled; }
      if(Input.GetKeyDown(KeyCode.Keypad2)) { JumpHacks.Toggle(); }
      if(Input.GetKeyDown(KeyCode.Keypad3)) { Speed.SpeedHack(); }
      if(Input.GetKeyDown(KeyCode.Keypad4)) { NoFall = !NoFall; }
      if(Input.GetKeyDown(KeyCode.Keypad5)) { Crosshair.Enabled = !Crosshair.Enabled; }
      if(Input.GetKeyDown(KeyCode.Keypad6)) { AvoidHorde.Enabled = !AvoidHorde.Enabled; }
      if(Input.GetKeyDown(KeyCode.Keypad7))
      {
        foreach(var item in Utils.GetObjectsInRadius(1000).Where(x => x.gameObject.GetComponent<HarvestMachine>() != null))
        {
          item.gameObject.GetComponent<HarvestMachine>()?.TryHarvest();
        }
      }
      if(Input.GetKeyDown(KeyCode.Keypad8)) { ESP.EspStorage = !ESP.EspStorage; }
      if(Input.GetKeyDown(KeyCode.Keypad9)) { ESP.Enabled = !ESP.Enabled; }
      if(Input.GetMouseButtonUp(2)) { StartCoroutine(Teleport.ToLookPositionOnClick()); }
    }
    public void OnGUI()
    {
      if(ESP.Enabled) { ESP.RenderNetViews(); }
      if(ESP.EspStorage) { ESP.RenderStorageESP(); }
    }
  }
}
