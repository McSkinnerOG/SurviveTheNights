using HarmonyLib;

namespace SurviveTheNights.Modules.Combat
{
  public class AntiJam
  {
    public static bool Enabled = false;
    [HarmonyPatch(typeof(PlayerOwner), nameof(PlayerOwner.JamClient))]
    public class JamClientPatch
    {
      static bool Prefix(ref PlayerOwner __instance)
      {
        if(Enabled) { return false; } else { return true; }
      }
    }
  }
}
