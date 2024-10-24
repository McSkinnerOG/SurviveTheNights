using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NDraw
{
  public static partial class Draw
  {
    public static partial class Screen
    {
      /// <summary>
      /// 
      /// </summary>
      /// <param name="rect">Rect that defines where to draw the grid on screen, in pixels</param>
      /// <param name="limits">The area that encompasses the current value</param>
      /// <param name="unit">The offset and separation of single cell</param>
      public static void SlidingGrid(Rect rect, Rect unit)
      {
        if(!Drawer.Exists) return;

        if(unit.height > 1)
        {
          var off = unit.y % unit.height;
          if(unit.y < 0) off += unit.height;

          var start = rect.y + off;
          var add = unit.height;

          for(var y = start; y < rect.yMax; y += add)
          {
            ScreenPoints.Add(new Vector3(rect.x, y));
            ScreenPoints.Add(new Vector3(rect.xMax, y));
          }
        }

        if(unit.width > 1)
        {
          var off = unit.x % unit.width;
          if(unit.x < 0) off += unit.width;

          var start = rect.x + off;
          var add = unit.width;

          for(var x = start; x < rect.xMax; x += add)
          {
            ScreenPoints.Add(new Vector3(x, rect.y));
            ScreenPoints.Add(new Vector3(x, rect.yMax));
          }
        }
      }
    }
  }
}
