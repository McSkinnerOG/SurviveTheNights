using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SurviveTheNights
{
  internal class Utils
  {
    public static Color GUIColorByDistance(float distance)
    {
      Color temp = new();
      if(distance > 150) { temp = Color.green; }
      if(distance is < 150 and > 25) { temp = Color.yellow; }
      if(distance < 25) { temp = Color.red; }
      return temp;
    }
    public static IEnumerable<Collider> GetObjectsInRadius(float radius)
    {
      var p1 = Refs.LP_Position + Refs.LP_CharMotorDB.controllerCenter;
      return Physics.OverlapSphere(p1, radius).Where(g => g.gameObject.GetComponent<HarvestMachine>() != null);
    }
  }
}
