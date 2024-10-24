namespace SurviveTheNights.Player
{
  public class JumpHacks
  {
    public static float BaseHeight = 1f;
    public static float ExtraHeight = 0.5f;
    public static float DefaultAirAcceleration = 20f;
    public static float MaxAirAcceleration = 25f;
    public static bool FDenabled = true;
    public static void Toggle(bool state)
    {
      if(state == true)
      {
        Main.BYPASS_RPC_SETPOS_FROM_SERVER = true;
        Main.BYPASS_PATCH_VELOCITY = true;
        Refs.LP_CharMotorDB.FDenabled = false;
        Refs.LP_CharMotorDB.jumping.baseHeight = BaseHeight;
        Refs.LP_CharMotorDB.jumping.extraHeight = ExtraHeight;
        Refs.LP_CharMotorDB.movement.defaultAirAcceleration = DefaultAirAcceleration;
        Refs.LP_CharMotorDB.movement.maxAirAcceleration = MaxAirAcceleration;
      }
      else
      {
        Refs.LP_CharMotorDB.jumping.baseHeight = 1f;
        Refs.LP_CharMotorDB.jumping.extraHeight = 0.5f;
        Refs.LP_CharMotorDB.movement.defaultAirAcceleration = 20f;
        Refs.LP_CharMotorDB.movement.maxAirAcceleration = 25f;
        Refs.LP_CharMotorDB.FDenabled = true;
        if(!Main.B_SpeedHack)
        {
          Main.BYPASS_RPC_SETPOS_FROM_SERVER = false;
          Main.BYPASS_PATCH_VELOCITY = false;
        }
      }
    }
  }
}