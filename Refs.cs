using uLink;
using UnityEngine;
namespace SurviveTheNights
{
  public class Refs
  {
    #region LOCALPLAYER
    public static PlayerOwner LP_Owner => PlayerOwner.instance;
    public static GameObject LP_GO => LP_Owner.transform.gameObject;
    public static Transform LP_Transform => LP_GO.transform;
    public static Vector3 LP_Position => LP_Transform.position;
    public static FPSInputControllerDB LP_FPSInputController => LP_GO.GetComponent<FPSInputControllerDB>();
    public static PlayerIndependantItemHover LP_ItemHover => LP_GO.GetComponent<PlayerIndependantItemHover>();
    public static PlayerInteraction LP_Interaction => LP_GO.GetComponent<PlayerInteraction>();
    public static PlayerInventoryNew LP_Inventory => LP_GO.GetComponent<PlayerInventoryNew>();
    public static PlayerChatInputs LP_Chat => LP_GO.GetComponent<PlayerChatInputs>();
    public static MouseLookDBJS LP_MouseScriptX => LP_GO.GetComponent<MouseLookDBJS>();
    public static MouseLookDBJS LP_MouseScriptY => LP_GO.GetComponentInChildren<MouseLookDBJS>(true);
    public static PlayerFortification LP_Fortification => LP_GO.GetComponent<PlayerFortification>();
    public static PlayerWeapons LP_PlayerWeapons => LP_GO.GetComponentInChildren<PlayerWeapons>(true);
    public static SmartCrosshair LP_Crosshair => LP_GO.GetComponentInChildren<SmartCrosshair>(true);
    public static DrownChecking LP_DrownChecker => LP_GO.GetComponentInChildren<DrownChecking>(true);
    public static PlayerStats LP_Stats => LP_Owner.stats;
    public static PlayerVitals LP_Vitals => LP_Stats.playerStats;
    public static PlayerLevelling LP_Leveling => LP_GO.GetComponent<PlayerLevelling>();
    public static FlashLight LP_Flashlight => LP_Owner.flashLight;
    public static FlashlightLocal LP_FlashlightLocal => LP_GO.GetComponentInChildren<FlashlightLocal>();
    public static MovementValues LP_MovememtValues => MovementValues.singleton;
    public static CharacterMotorDB LP_CharMotorDB => LP_Owner.characterMotor;
    public static uLinkNetworkView LP_NetView => LP_GO.GetComponent<uLinkNetworkView>();
    public static uLinkStrictPlatformer LP_Platformer => LP_GO.GetComponent<uLinkStrictPlatformer>();
    public static SafeLogOffPlayer LP_LogOffPlayer => LP_GO.GetComponent<SafeLogOffPlayer>();
    public static FixedItemPlacement LP_ItemPlacement => LP_GO.GetComponent<FixedItemPlacement>();
    #endregion
    public static Vector2 MouseXY => new Vector2(LP_MouseScriptX.rotationX, LP_MouseScriptY.rotationY);
    public static Dictionary<NetworkViewID, NetworkViewBase> NetworkView_Dictionary => LP_NetView._network._enabledViews;
  }
}
