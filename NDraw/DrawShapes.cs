using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NDraw
{
  public static partial class Draw
  {
    public static partial class Screen
    {
      public static void Rect(Rect rect)
      {
        if(!Drawer.Exists) return;

        Rect(rect.x, rect.y, rect.width, rect.height);
      }

      public static void Rect(float x, float y, float width, float height)
      {
        if(!Drawer.Exists) return;

        ScreenPoints.Add(new Vector2(x, y));
        ScreenPoints.Add(new Vector2(x + width, y));

        ScreenPoints.Add(new Vector2(x + width, y));
        ScreenPoints.Add(new Vector2(x + width, y + height));

        ScreenPoints.Add(new Vector2(x + width, y + height));
        ScreenPoints.Add(new Vector2(x, y + height));

        ScreenPoints.Add(new Vector2(x, y + height));
        ScreenPoints.Add(new Vector2(x, y));
      }

      public static void Circle(Vector2 center, float pixelRadius, int interpolations = 40)
      {
        Circle(center.x, center.y, pixelRadius, interpolations);
      }

      public static void Circle(float centerX, float centerY, float pixelRadius, int interpolations = 40)
      {
        if(!Drawer.Exists) return;

        var size = new Vector2(pixelRadius, pixelRadius);
        var center = new Vector2(centerX, centerY);

        Ellipse(center, size, interpolations);
      }

      public static void Ellipse(Vector2 center, Vector2 size, int interpolations = 40)
      {
        if(!Drawer.Exists) return;

        var radX = size.x;
        var radY = size.y;

        var ci = new Vector2(
            center.x + (1 * radX),
            center.y);

        var ci0 = ci;
        var step = (2 * Mathf.PI) / interpolations;

        for(var i = 0; i < interpolations; i++)
        {
          var theta = i * step;
          ScreenPoints.Add(ci);

          ci = new Vector2(center.x + (Mathf.Cos(theta) * radX), center.y + (Mathf.Sin(theta) * radY));

          ScreenPoints.Add(ci);
        }

        // close
        ScreenPoints.Add(ci);
        ScreenPoints.Add(ci0);
      }

      public static void Grid(int xLineNum, int yLineNum, Rect rect)
      {
        if(!Drawer.Exists) return;

        var add = rect.height / yLineNum;
        for(var i = 0; i <= yLineNum; i++)
        {
          var y = rect.yMax - i * add;
          ScreenPoints.Add(new Vector2(rect.x, y));
          ScreenPoints.Add(new Vector2(rect.xMax, y));
        }

        add = rect.width / yLineNum;
        for(var i = 0; i <= yLineNum; i++)
        {
          var x = rect.x + i * add;
          ScreenPoints.Add(new Vector2(x, rect.y));
          ScreenPoints.Add(new Vector2(x, rect.yMax));
        }
      }

      // FILLED

      public static void FillRect(Rect rect)
      {
        if(!Drawer.Exists) return;

        FillRect(rect.x, rect.y, rect.width, rect.height);
      }

      public static void FillRect(float x, float y, float width, float height)
      {
        if(!Drawer.Exists) return;

        var p0 = new Vector3(x, y);
        var p1 = new Vector3(x + width, y);
        var p2 = new Vector3(x, y + height);
        var p3 = new Vector3(x + width, y + height);

        ScreenTrisPoints.Add(p0);
        ScreenTrisPoints.Add(p1);
        ScreenTrisPoints.Add(p2);

        ScreenTrisPoints.Add(p1);
        ScreenTrisPoints.Add(p3);
        ScreenTrisPoints.Add(p2);
      }

      public static void FillTriangle(Vector2 p1, Vector2 p2, Vector2 p3)
      {
        if(!Drawer.Exists) return;

        ScreenTrisPoints.Add(p1);
        ScreenTrisPoints.Add(p2);
        ScreenTrisPoints.Add(p3);
      }

      public static void FillFanPolygon(Vector2[] points)
      {
        if(!Drawer.Exists) return;
        if(points == null || points.Length < 2) return;

        Vector3 p0 = points[0];

        for(var i = 1; i < points.Length - 1; i++)
        {
          ScreenTrisPoints.Add(p0);
          ScreenTrisPoints.Add(points[i]);
          ScreenTrisPoints.Add(points[i + 1]);
        }
      }

      public static void FillFanPolygon(List<Vector2> points)
      {
        if(!Drawer.Exists) return;
        if(points == null || points.Count < 2) return;

        Vector3 p0 = points[0];

        var pct = points.Count;
        for(var i = 1; i < pct - 1; i++)
        {
          ScreenTrisPoints.Add(p0);
          ScreenTrisPoints.Add(points[i]);
          ScreenTrisPoints.Add(points[i + 1]);
        }
      }
    }

    public static partial class World
    {
      public static void Cube(Vector3 center, Vector3 size, Vector3 forward, Vector3 up)
      {
        if(!Drawer.Exists) return;

        forward = forward.normalized;
        up = Vector3.ProjectOnPlane(up, forward).normalized;
        var right = Vector3.Cross(forward, up);

        var frw = forward * size.z * 0.5f;
        var rgt = right * size.x * 0.5f;
        var upw = up * size.y * 0.5f;

        // vertical lines
        WorldPoints.Add(center - frw - rgt - upw);
        WorldPoints.Add(center - frw - rgt + upw);

        WorldPoints.Add(center - frw + rgt - upw);
        WorldPoints.Add(center - frw + rgt + upw);

        WorldPoints.Add(center + frw - rgt - upw);
        WorldPoints.Add(center + frw - rgt + upw);

        WorldPoints.Add(center + frw + rgt - upw);
        WorldPoints.Add(center + frw + rgt + upw);

        // horizontal lines
        WorldPoints.Add(center - frw - rgt - upw);
        WorldPoints.Add(center - frw + rgt - upw);

        WorldPoints.Add(center - frw - rgt + upw);
        WorldPoints.Add(center - frw + rgt + upw);

        WorldPoints.Add(center + frw - rgt - upw);
        WorldPoints.Add(center + frw + rgt - upw);

        WorldPoints.Add(center + frw - rgt + upw);
        WorldPoints.Add(center + frw + rgt + upw);

        // forward lines
        WorldPoints.Add(center - frw - rgt - upw);
        WorldPoints.Add(center + frw - rgt - upw);

        WorldPoints.Add(center - frw + rgt - upw);
        WorldPoints.Add(center + frw + rgt - upw);

        WorldPoints.Add(center - frw - rgt + upw);
        WorldPoints.Add(center + frw - rgt + upw);

        WorldPoints.Add(center - frw + rgt + upw);
        WorldPoints.Add(center + frw + rgt + upw);
      }

      public static void Circle(Vector3 center, float radius, Vector3 normal, int interpolations = 100)
      {
        if(!Drawer.Exists) return;

        normal = normal.normalized;
        var forward = normal == Vector3.up ?
            Vector3.ProjectOnPlane(Vector3.forward, normal).normalized :
            Vector3.ProjectOnPlane(Vector3.up, normal).normalized;

        var p = center + forward * radius;

        var step = 360.0f / interpolations;

        for(var i = 0; i <= interpolations; i++)
        {
          var theta = i * step;

          WorldPoints.Add(p);

          var angleDir = Quaternion.AngleAxis(theta, normal) * forward;
          p = center + angleDir * radius;

          WorldPoints.Add(p);
        }
      }

      public static void Helix(Vector3 p1, Vector3 p2, Vector3 forward, float radius, float angle)
      {
        if(!Drawer.Exists) return;

        var diff = p2 - p1;
        var normal = diff.normalized;

        forward = Vector3.ProjectOnPlane(forward, normal).normalized;

        //Vector3 right = Vector3.Cross(normal, forward);

        var ci = p1 + forward * radius;

        var lengthFactor = diff.magnitude;

        for(var f = 0.0f; f <= 1.0f; f += 0.02f)
        {
          var theta = f * angle * Mathf.Deg2Rad;

          //Vector3 ci = center + forward * Mathf.Cos(theta) * radius + right * Mathf.Sin(theta) * radius;

          WorldPoints.Add(ci);

          var offset = normal * f * lengthFactor;

          var angleDir = Quaternion.AngleAxis(theta * Mathf.Rad2Deg, normal) * forward;
          ci = p1 + angleDir.normalized * radius + offset;

          WorldPoints.Add(ci);

          //if (theta != 0)
          //GL.Vertex(ci);
        }

        //worldPoints.Add(ci);
        //worldPoints.Add(c0);
      }

      public static void Spiral(Vector3 position, Vector3 normal, Vector3 forward, float radius)
      {
        const int count = 80;
        const float angle = -17.453f;
        var add = radius / count;

        var lastP = Vector3.zero;

        for(var i = 0; i < count; i++)
        {
          var p = forward * add * i;
          p = Quaternion.AngleAxis(angle * i, normal) * p;
          Line(position + lastP, position + p);
          lastP = p;
        }
      }
    }
  }
}