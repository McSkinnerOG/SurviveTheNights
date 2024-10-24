using HarmonyLib;
using UnityEngine;

namespace SurviveTheNights.Patches
{
  internal class ULinkStrictPlatformer_Patch
  {
    #region BYPASS_RPC AdjustOwnerPos
    [HarmonyPatch(typeof(uLinkStrictPlatformer), nameof(uLinkStrictPlatformer.AdjustOwnerPos))]
    public class P_uLinkStrictPlatformer_AdjustOwnerPos
    {
      public static int BypassedCount = 0;

      private static bool Prefix(ref uLinkStrictPlatformer __instance, Vector3 pos)
      {
        if(!Main.BYPASS_RPC_SETPOS_FROM_SERVER) { return true; }
        BypassedCount++;
        if(Main.Debug_MessageToIngame) { MessageSystem.instance.AddMessageToChatClient($"BYPASS->RPC->AdjustOwnerPos[{BypassedCount}]: ", "red", "Stopped RPC AdjustOwnerPos."); }
        return false;
      }
    }
    #endregion
  }
}
