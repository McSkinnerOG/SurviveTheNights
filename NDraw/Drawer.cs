///
/// You can use 2 defines, and add them in Settings > Player > Other Settings > Scripting Define Symbols:
/// 
/// NDRAW_ORHTO_MULTIPLICATION
///     ^ If screen space lines don't draw in some cases (for example in HDRP), try using this define. 
///     Reason: GL.LoadPixelMatrix() doesn't seem to work in some cases so GL.LoadOrtho() needs to be used instead,
///     but there is a tiny cost of multiplying each vertex with screen resolution.
///     
/// NDRAW_UPDATE_IN_COROUTINE
///     ^ In case you are using SRP (URP or HDRP) and you don't see any lines, try using this define.
///     Reason: NDraw typically uses OnPostRender() for drawing, but in SRP this callback is not functional.
///     Instead a coroutine that waits for end of frame is used. IT doesn't produce any GC allocs because
///     WaitForEndOfFrame is cached.
///     

using System.Collections.Generic;
using UnityEngine;

namespace NDraw
{

  public class Drawer : MonoBehaviour
  {
    private static Drawer E;
    public Material Material;
    private Camera Camera;

    public static bool Exists => E != null && E.enabled;

    private static readonly Vector2 One = Vector2.one;

    private void Awake()
    {
      E = this;
    }

    protected virtual void Start()
    {
      if(Material == null)
        CreateLineMaterial();

      Camera = GetComponent<Camera>();

#if NDRAW_UPDATE_IN_COROUTINE
            StartCoroutine(PostRender());
#endif
    }

    private void OnDestroy()
    {
      Draw.Clear();
    }

    private WaitForEndOfFrame Wof = new WaitForEndOfFrame();

#if !NDRAW_UPDATE_IN_COROUTINE
    private void OnPostRender()
    {
      if(enabled)
        Render();

      Draw.Clear();
    }
#endif

#if NDRAW_UPDATE_IN_COROUTINE
        IEnumerator PostRender()
        {
            while (true)
            {
                yield return wof;

                if (enabled)
                    Render();

                Draw.Clear();
            }
        }
#endif

    private void CreateLineMaterial()
    {
      var shader = Shader.Find("Hidden/Internal-Colored");
      Material = new Material(shader);
      //material.hideFlags = HideFlags.HideAndDontSave;
      // Turn on alpha blending
      Material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
      Material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
      // Turn backface culling off
      Material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
      // Turn off depth writes
      Material.SetInt("_ZWrite", 0);

      // makes the material draw on top of everything
      Material.SetInt("_ZTest", 0);
    }

    protected void Render()
    {
      Material.SetPass(0);

      //-------------
      // WORLD SPACE
      //-------------

      GL.PushMatrix();
      GL.LoadProjectionMatrix(Camera.projectionMatrix);
      GL.modelview = Camera.worldToCameraMatrix;

      GL.Begin(GL.LINES);
      GL.Color(Color.white);
      ProcessPoints(Draw.WorldPoints, Draw.WorldColorIndices, false);
      GL.End();

      GL.PopMatrix();

      //--------------
      // SCREEN SPACE
      //--------------

      GL.PushMatrix();

#if NDRAW_ORHTO_MULTIPLICATION
            GL.LoadOrtho();
#else
      GL.LoadPixelMatrix();
#endif

      GL.Begin(GL.TRIANGLES);
      ProcessPoints(Draw.ScreenTrisPoints, Draw.ScreenTrisColorIndices, true);
      GL.End();

      GL.Begin(GL.LINES);
      ProcessPoints(Draw.ScreenPoints, Draw.ScreenColorIndices, true);
      GL.End();

      GL.PopMatrix();

    }

    private static void ProcessPoints(List<Vector3> points, List<Draw.ColorIndex> colorIndices, bool screen)
    {
      if(points.Count == 0) return;

      //GL.Color(Color.white);
#if NDRAW_ORHTO_MULTIPLICATION
            Vector2 s = screen ? new Vector2(1.0f / Screen.width, 1.0f / Screen.height) : one;
#endif

      var hasColors = colorIndices.Count > 0;

      var ci = 0;
      var ct = points.Count;
      for(var i = 0; i < ct; i++)
      {
        // handle color
        if(hasColors && colorIndices[ci].I == i)
        {
          GL.Color(colorIndices[ci].C);

          ci++;
          if(ci >= colorIndices.Count) ci = 0;
        }

        // push vertex
#if NDRAW_ORHTO_MULTIPLICATION
                if (screen)
                    GL.Vertex(points[i] * s);
                else
                    GL.Vertex(points[i]);
#else
        GL.Vertex(points[i]);
#endif
      }
    }

    protected void ClearLines()
    {
      Draw.Clear();
    }
  }
}