using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
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
    public Main()
    {
      Log = Logger;
      Harmony = new Harmony(GUID);
      Assembly = Assembly.GetExecutingAssembly();
      ModFolder = Path.GetDirectoryName(Assembly.Location);
    }
    public void Start() { Harmony.PatchAll(Assembly); }
  }
}
