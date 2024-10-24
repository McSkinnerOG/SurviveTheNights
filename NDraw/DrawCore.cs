using System.Collections.Generic;
using UnityEngine;

namespace NDraw
{
  public static partial class Draw
  {
#if NET46
        internal readonly struct ColorIndex
#else
    internal struct ColorIndex
#endif
    {
      public readonly int I;
      public readonly Color C;

      public ColorIndex(int i, Color c)
      {
        I = i;
        C = c;
      }
    }

    internal static List<Vector3> ScreenPoints = new List<Vector3>();
    internal static List<Vector3> WorldPoints = new List<Vector3>();
    internal static List<Vector3> ScreenTrisPoints = new List<Vector3>();

    internal static List<ColorIndex> ScreenColorIndices = new List<ColorIndex>();
    internal static List<ColorIndex> WorldColorIndices = new List<ColorIndex>();
    internal static List<ColorIndex> ScreenTrisColorIndices = new List<ColorIndex>();

    internal static void Clear()
    {
      ScreenPoints.Clear();
      ScreenColorIndices.Clear();

      WorldPoints.Clear();
      WorldColorIndices.Clear();

      ScreenTrisPoints.Clear();
      ScreenTrisColorIndices.Clear();
    }

    public static partial class Screen
    {
      public static void SetColor(Color color)
      {
        if(!Drawer.Exists) return;

        var pointIndex = ScreenPoints.Count;
        var lastci = ScreenColorIndices.Count - 1;

        var ci = new ColorIndex(pointIndex, color);

        // Overwrite if last index is the same as this one
        if(ScreenColorIndices.Count > 0 &&
            ScreenColorIndices[lastci].I == pointIndex)
        {
          ScreenColorIndices[lastci] = ci;
          return;
        }

        ScreenColorIndices.Add(ci);
      }

      public static void SetFillColor(Color color)
      {
        if(!Drawer.Exists) return;

        var pointIndex = ScreenTrisPoints.Count;
        var lastci = ScreenTrisColorIndices.Count - 1;

        var ci = new ColorIndex(ScreenTrisPoints.Count, color);

        // Overwrite if last index is the same as this one
        if(ScreenTrisColorIndices.Count > 0 &&
            ScreenTrisColorIndices[lastci].I == pointIndex)
        {
          ScreenTrisColorIndices[lastci] = ci;
          return;
        }

        ScreenTrisColorIndices.Add(ci);
      }
    }

    public static partial class World
    {
      public static void SetColor(Color color)
      {
        if(!Drawer.Exists) return;

        var ci = new ColorIndex(WorldPoints.Count, color);
        WorldColorIndices.Add(ci);
      }
    }
  }
}