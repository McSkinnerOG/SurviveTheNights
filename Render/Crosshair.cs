using NDraw;
using UnityEngine;

namespace SurviveTheNights.Render
{
  public class Crosshair
  {
    public static bool Enabled = false;
    public static void RenderCrosshair(float size, bool circle, int interpolations = 40)
    {
      if(Camera.main.GetComponent<Drawer>() == null) { _ = Camera.main.gameObject.AddComponent<Drawer>(); }
      if(circle) { Draw.Screen.Circle(Screen.width / 2, Screen.height / 2, size, interpolations); }
      else
      {
        Vector2[] vector2S =
        [ 
          // LINE #1
          new((Screen.width / 2) - 6, Screen.height / 2), 
          // LINE #2
          new((Screen.width / 2) + 6, Screen.height / 2), 
          // LINE #3
          new(Screen.width / 2, Screen.height / 2), 
          // LINE #4
          new(Screen.width / 2, (Screen.height / 2) + 6),
          new(Screen.width / 2, (Screen.height / 2) -6)
        ];
        Draw.Screen.MultiLine(vector2S);
      }
    }
  }
}
