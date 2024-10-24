using System.Collections.Generic;
using UnityEngine;

namespace NDraw
{
  public static partial class Draw
  {
    public static partial class Screen
    {
      public static void Line(int p1x, int p1y, int p2x, int p2y)
      {
        if(!Drawer.Exists) return;

        ScreenPoints.Add(new Vector2(p1x, p1y));
        ScreenPoints.Add(new Vector2(p2x, p2y));
      }

      public static void Line(Vector2 p1, Vector2 p2)
      {
        if(!Drawer.Exists) return;

        ScreenPoints.Add(p1);
        ScreenPoints.Add(p2);
      }

      public static void MultiLine(Vector2[] points)
      {
        if(!Drawer.Exists) return;

        if(points.Length < 2) return;

        for(var i = 0; i < points.Length - 1; i++)
        {
          ScreenPoints.Add(points[i]);
          ScreenPoints.Add(points[i + 1]);
        }
      }

      public static void MultiLine(List<Vector2> points)
      {
        if(!Drawer.Exists) return;

        var pct = points.Count;
        if(pct < 2) return;

        for(var i = 0; i < pct - 1; i++)
        {
          ScreenPoints.Add(points[i]);
          ScreenPoints.Add(points[i + 1]);
        }
      }

      public static void MultiLine(Vector2[] points, Vector2 offset, float scale)
      {
        if(!Drawer.Exists) return;

        if(points.Length < 2) return;

        for(var i = 0; i < points.Length - 1; i++)
        {
          ScreenPoints.Add(offset + (points[i] * scale));
          ScreenPoints.Add(offset + (points[i + 1] * scale));
        }
      }
    }

    public static partial class World
    {
      public static void Line(Vector3 p1, Vector3 p2)
      {
        if(!Drawer.Exists) return;

        WorldPoints.Add(p1);
        WorldPoints.Add(p2);
      }

      public static void Ray(Vector3 point, Vector3 dir)
      {
        if(!Drawer.Exists) return;

        WorldPoints.Add(point);
        WorldPoints.Add(point + dir);
      }
    }
  }
}
