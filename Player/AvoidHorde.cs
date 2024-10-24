using HarmonyLib;
using uLink;

namespace SurviveTheNights.Player
{
  public class AvoidHorde
  {
    public static bool Enabled = false;
    [HarmonyPatch(typeof(AvoidingHorde), nameof(AvoidingHorde.Update))]
    public class AvoidingHordePatch
    {
      static bool Prefix(ref AvoidingHorde __instance)
      {
        if(Enabled)
        {
          if(ZombieHordesManager.instance == null || PlayerOwner.instance == null)
          {
            __instance.transform.GetChild(0).gameObject.SetActive(false);
            return false;
          }
          var curHorde = ZombieHordesManager.instance.curHorde;
          if(curHorde == null) { return false; }
          PlayerOwner.instance.networkView.RPC("FleeHordeServer", RPCMode.Server, []);
          MessageSystem.instance.AddMessageToChatClient($"AUTO AVOIDED HORDE!: ", "red", "(or at least attempted to.)");
          __instance.ResetTimer();
          return false;
        }
        else { return true; }
      }
    }
  }

}
