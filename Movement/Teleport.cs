using System.Collections;
using UnityEngine;

namespace SurviveTheNights.Movement
{
  public class Teleport
  {
    public static float F_cooldown = 2;
    public static bool B_cooldown = false;

    public static IEnumerator ToPlayerMarker()
    {
      if(WaypointUI.instance_PlayerMarker.target != Vector3.zero)
      {
        Main.BYPASS_PATCH_VELOCITY = true;
        Main.BYPASS_RPC_SETPOS_FROM_SERVER = true;
        Refs.LP_CharMotorDB.FDenabled = false;
        Refs.LP_GO.transform.position = new Vector3(WaypointUI.instance_PlayerMarker.target.x, WaypointUI.instance_PlayerMarker.target.y + 1f, WaypointUI.instance_PlayerMarker.target.z);
      }
      else
      {
        PlayerNotifications.instance.AddNotification("ERROR", "Please set a waypoint/marker first!", NotificationType.Horde, 1, Color.red);
      }
      yield return new WaitForSeconds(F_cooldown);
      B_cooldown = false;
      if(!JumpHacks.Enabled && !Speed.Enabled)
      {
        Refs.LP_CharMotorDB.FDenabled = true;
        Main.BYPASS_RPC_SETPOS_FROM_SERVER = false;
        Main.BYPASS_PATCH_VELOCITY = false;
      }
    }

    public static IEnumerator ToLookPositionOnClick()
    {
      if(B_cooldown == false)
      {
        var rayposition = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if(Physics.Raycast(rayposition, out var hit, 10000f))
        {
          Main.BYPASS_PATCH_VELOCITY = true;
          Main.BYPASS_RPC_SETPOS_FROM_SERVER = true;
          Refs.LP_CharMotorDB.FDenabled = false;
          Refs.LP_GO.transform.position = new Vector3(hit.point.x, hit.point.y + 2, hit.point.z);

          B_cooldown = true;
        }
        else
        {
          PlayerNotifications.instance.AddNotification("ERROR", "No Collider Detected within 10k Meters", NotificationType.Horde, 1, Color.red);
        }
        yield return new WaitForSeconds(F_cooldown);
        B_cooldown = false;
        if(!JumpHacks.Enabled && !Speed.Enabled)
        {
          Refs.LP_CharMotorDB.FDenabled = true;
          Main.BYPASS_RPC_SETPOS_FROM_SERVER = false;
          Main.BYPASS_PATCH_VELOCITY = false;
        }
      }
      else
      {
        PlayerNotifications.instance.AddNotification("ERROR", "COOLDOWN STILL ACTIVE!", NotificationType.Horde, 1, Color.red);
        yield break;
      }
    }
  }
}
