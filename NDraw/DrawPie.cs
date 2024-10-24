using UnityEngine;

namespace NDraw
{
  public static partial class Draw
  {
    public static partial class Screen
    {
      public static void Pie(int x, int y, float innerRadius, float outerRadius, float value)
      {
        if(!Drawer.Exists) return;

        var ci_in = new Vector2(x, y + innerRadius);
        var ci_out = new Vector2(x, y - outerRadius);
        var cil_in = ci_in;
        var cil_out = ci_out;

        var s = Mathf.Sign(value);
        var add = 0.3f * s;
        var limit = Mathf.Abs(2 * Mathf.PI * value);

        for(var theta = 0.0f; Mathf.Abs(theta) < limit; theta += add)
        {

          ci_in = new Vector2(
              x + (Mathf.Sin(theta) * innerRadius),
              y - (Mathf.Cos(theta) * innerRadius));

          ci_out = new Vector2(
              x + (Mathf.Sin(theta) * outerRadius),
              y - (Mathf.Cos(theta) * outerRadius));

          ScreenTrisPoints.Add(cil_in);
          ScreenTrisPoints.Add(cil_out);
          ScreenTrisPoints.Add(ci_out);

          ScreenTrisPoints.Add(ci_out);
          ScreenTrisPoints.Add(ci_in);
          ScreenTrisPoints.Add(cil_in);

          // previous points
          cil_in = ci_in;
          cil_out = ci_out;
        }



        // last segment
        ci_in = new Vector2(
            x + (Mathf.Sin(limit * s) * innerRadius),
            y - (Mathf.Cos(limit * s) * innerRadius));

        ci_out = new Vector2(
            x + (Mathf.Sin(limit * s) * outerRadius),
            y - (Mathf.Cos(limit * s) * outerRadius));

        ScreenTrisPoints.Add(cil_in);
        ScreenTrisPoints.Add(cil_out);
        ScreenTrisPoints.Add(ci_out);

        ScreenTrisPoints.Add(ci_out);
        ScreenTrisPoints.Add(ci_in);
        ScreenTrisPoints.Add(cil_in);
      }
    }
  }
}