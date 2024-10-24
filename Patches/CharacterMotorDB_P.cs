using a2z_Interactive;
using HarmonyLib;
using uLink;
using UnityEngine;

namespace SurviveTheNights.Patches
{
  internal class CharacterMotorDB_P
  {
    #region Bypass_Velocity
    [HarmonyPatch(typeof(CharacterMotorDB), "UpdateFunction", null)]
    public class CharacterMotorDB_UpdateFunction
    {
      public static bool Prefix(CharacterMotorDB __instance)
      {
        if(__instance.playerOwner == null)
        {
          __instance.playerOwner = __instance.GetComponentInChildren<PlayerOwner>();
        }
        if(PoolServers.instance.waitingOnStreamerLoad && __instance.terrainRayIndex < 0) { return false; }
        if(CharacterMotorDB.paused) { return false; }
        if(__instance.frozen) { return false; }
        if(__instance.diving)
        {
          var y = __instance.movement.velocity.y;
          __instance.SetVelocity((__instance.transform.forward * 20f) + (Vector3.up * y));
        }
        // #CHEAT# BYPASS VELOCITY
        if(__instance.movement.velocity.y > 3.5f)
        {
          if(!Main.BYPASS_PATCH_VELOCITY)
          {
            Debug.Log("Your Y velocity is being stopped because it was way too much! " + __instance.movement.velocity.y.ToString());
            __instance.movement.velocity.y = 0f;
          }
          MessageSystem.instance.AddMessageToChatClient("BYPASS::", "red", "VELOCITY:" + __instance.movement.velocity.y.ToString());
        }
        if(__instance.vegetationTimer < 10f)
        {
          __instance.vegetationTimer += Time.deltaTime;
        }
        if(__instance.vegetationTimer is > 0.25f and < 100f)
        {
          __instance.noLongerWalkingInVegetation = true;
        }
        if(__instance.noLongerWalkingInVegetation)
        {
          __instance.rustlingTimer = 3f;
          __instance.NormalSpeed();
          __instance.canSprint = true;
          var componentsInChildren = __instance.gameObject.GetComponentsInChildren<AimMode>();
          for(var i = 0; i < componentsInChildren.Length; i++)
          {
            componentsInChildren[i].canSprint = true;
          }
          __instance.vegetationTimer = 200f;
          __instance.noLongerWalkingInVegetation = false;
        }

        _ = __instance.movement.velocity;
        __instance.terrainRayIndex++;
        if(__instance.terrainRayIndex > 3)
        {
          __instance.terrainRayIndex = 0;
          __instance.terrainRay = new Ray(__instance.transform.position + (Vector3.up * 400f), Vector3.down);
          if(Physics.Raycast(__instance.terrainRay, out __instance.terrainHit, 1000f, LayerMask.GetMask(["Terrain"])) && __instance.terrainHit.collider != null && __instance.terrainHit.collider.name.Contains("TerrainV2"))
          {
            var position1 = __instance.transform.position;
            var num = Vector3.Distance(__instance.terrainHit.point, position1);
            var flag = position1.y < __instance.terrainHit.point.y;
            var flag2 = num is > 1.8f and <= 40f;
            var flag3 = num > 40f;
            if(OceanAndSunCulling.instance && OceanAndSunCulling.instance.insideColliderCount > 0)
            {
              flag2 = false;
            }
            if(flag && (flag2 || flag3))
            {
              _ = a2z_General.IsUnderCover(__instance.transform.position);
              __instance.transform.position = __instance.terrainHit.point + (Vector3.up * 0.8f);
              _ = __instance.ApplyInputVelocityChange(Vector3.zero);
            }
          }
        }
        _ = __instance.ApplyInputVelocityChange(__instance.movement.velocity);
        _ = __instance.ApplyGravityAndJumping(__instance.movement.velocity);
        _ = Vector3.zero;
        var position = __instance.tr.position;
        var vector2 = __instance.movement.velocity * Time.deltaTime;
        var num2 = Mathf.Max(__instance.controller.stepOffset, new Vector3(vector2.x, 0f, vector2.z).magnitude);
        if(PoolServers.currentState != StateNew.inGame)
        {
          num2 = 0f;
        }
        if(__instance.IsGrounded())
        {
          __instance.maxAllowedUpwardVelocity = 0.01f;
          vector2 -= num2 * Vector3.up;
        }
        __instance.groundNormal = Vector3.zero;
        vector2.y = Mathf.Min(vector2.y, __instance.maxAllowedUpwardVelocity);
        if(__instance.controller.enabled)
        {
          __instance.movement.collisionFlags = __instance.controller.Move(vector2);
        }
        __instance.movement.lastHitPoint = __instance.movement.hitPoint;
        __instance.lastGroundNormal = __instance.groundNormal;
        var vector3 = new Vector3(__instance.movement.velocity.x, 0f, __instance.movement.velocity.z);
        __instance.movement.velocity = (__instance.tr.position - position) / Time.deltaTime;
        var vector4 = new Vector3(__instance.movement.velocity.x, 0f, __instance.movement.velocity.z);
        if(vector3 == Vector3.zero)
        {
          __instance.movement.velocity = new Vector3(0f, __instance.movement.velocity.y, 0f);
        }
        else
        {
          var num3 = Vector3.Dot(vector4, vector3) / vector3.sqrMagnitude;
          __instance.movement.velocity = (vector3 * Mathf.Clamp01(num3)) + (__instance.movement.velocity.y * Vector3.up);
        }
        if(__instance.movement.velocity.y < __instance.movement.velocity.y - 0.001f)
        {
          if(__instance.movement.velocity.y < 0f)
          {
            var velocity = __instance.movement.velocity;
            velocity.y = __instance.movement.velocity.y;
            __instance.movement.velocity = velocity;
          }
          else
          {
            __instance.jumping.holdingJumpButton = false;
          }
        }
        if(__instance.grounded && !__instance.IsGroundedTest())
        {
          __instance.grounded = false;
          __instance.tr.position += num2 * Vector3.up;
        }
        else if(!__instance.grounded && __instance.IsGroundedTest() && !DrownChecking.instance.isUnderWater)
        {
          __instance.grounded = true;
          __instance.jumping.jumping = false;
          if(__instance.diving)
          {
            __instance.diving = false;
            __instance.SetVelocity(__instance.transform.forward * 6f);
            __instance.GetComponent<AudioSource>().volume = __instance.proneLandSoundVolume;
            __instance.GetComponent<AudioSource>().PlayOneShot(__instance.proneLandSound);
          }
          __instance.BroadcastMessage("Landed", SendMessageOptions.DontRequireReceiver);
          var num4 = -__instance.movement.velocity.y;
          if(__instance.playerOwner == null)
          {
            __instance.playerOwner = __instance.GetComponentInChildren<PlayerOwner>();
          }
          if(__instance.FDenabled)
          {
            if(num4 >= 8f)
            {
              // #CHEAT# NOFALL
              if(!Main.B_NoFall)
              {
                __instance.GetComponent<uLinkNetworkView>().RPC("ApplyFD", RPCMode.Server, num4);
              }
              MessageSystem.instance.AddMessageToChatClient("BYPASS::", "red", "FallDamage" + __instance.movement.velocity.y.ToString());
            }
            var num5 = Mathf.Clamp(-num4 / 20f, -1f, 0f);
            GunLook.jostleAmt = new Vector3(0f, num5 * 0.5f, 0f);
            CamSway.jostleAmt = new Vector3(0f, num5 * 0.2f, 0f);
            __instance.GetComponent<AudioSource>().volume = __instance.landSoundVolume;
            if(__instance.landSound)
            {
              __instance.GetComponent<AudioSource>().PlayOneShot(__instance.landSound);
            }
          }
        }
        __instance.velocityForNetwork = __instance.movement.velocity;
        _ = Vector3.zero;
        return false;
      }
    }
    #endregion
  }
}
