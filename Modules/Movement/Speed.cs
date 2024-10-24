namespace SurviveTheNights.Modules.Movement
{
  public class Speed
  {
    public static bool Enabled = true;
    public static float MaxSprintSpeed = 5.8f;
    public static float SprintSidewaysSpeed = 3f;
    public static float DefaultAirAcceleration = 20f;
    public static float SprintAirAcceleration = 25f;
    public static float MaxAirAcceleration = 25f;
    public static float SprintGroundAcceleration = 50f;
    public static void SpeedHack()
    {
      Enabled = !Enabled;
      if(Enabled)
      {
        Main.BYPASS_RPC_SETPOS_FROM_SERVER = true;
        Main.BYPASS_PATCH_VELOCITY = true;
        Refs.LP_CharMotorDB.FDenabled = false;
        Refs.LP_CharMotorDB.movement.maxSprintSpeed = MaxSprintSpeed;
        Refs.LP_CharMotorDB.movement.sprintSidewaysSpeed = SprintSidewaysSpeed;
        Refs.LP_CharMotorDB.movement.defaultAirAcceleration = DefaultAirAcceleration;
        Refs.LP_CharMotorDB.movement.sprintAirAcceleration = SprintAirAcceleration;
        Refs.LP_CharMotorDB.movement.maxAirAcceleration = MaxAirAcceleration;
        Refs.LP_CharMotorDB.movement.sprintGroundAcceleration = SprintGroundAcceleration;
      }
      else
      {
        if(!JumpHacks.Enabled)
        {
          Refs.LP_CharMotorDB.FDenabled = Main.NoFall;
          Main.BYPASS_RPC_SETPOS_FROM_SERVER = false;
          Main.BYPASS_PATCH_VELOCITY = false;
        }
        Refs.LP_CharMotorDB.movement.maxSprintSpeed = 5.8f;
        Refs.LP_CharMotorDB.movement.sprintSidewaysSpeed = 3f;
        Refs.LP_CharMotorDB.movement.defaultAirAcceleration = 20f;
        Refs.LP_CharMotorDB.movement.sprintAirAcceleration = 25f;
        Refs.LP_CharMotorDB.movement.maxAirAcceleration = 25f;
        Refs.LP_CharMotorDB.movement.sprintGroundAcceleration = 50f;
      }
    }
  }
}
