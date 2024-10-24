using UnityEngine;

namespace NDraw
{
  public static partial class Draw
  {
    public static partial class World
    {
      /// <summary>
      /// Draw a conic section, oriented with periapsis at forward.
      /// Draws a circle, ellipse, parabola or hyperbola depending on the eccentricity.
      /// It does not draw the negative part of hyperbola.
      /// </summary>
      /// <param name="center">The focus point</param>
      /// <param name="eccentricity">e=0 is circle, e less than 1 is ellipse, e=1 is parabola, e more than 1 is hyperbola </param>
      /// <param name="semiMajorAxis">Semi major axis of the ellipse, in case e is more than 1 it should be negative</param>
      /// <param name="normal">The normal vector of the section</param>
      /// <param name="periapsisDirection">The direction of the periapsis</param>
      public static void ConicSection(Vector3 center, float eccentricity, float semiMajorAxis, Vector3 normal, Vector3 periapsisDirection, int interpolations)
      {
        var semilatus = eccentricity == 1 ? semiMajorAxis :
            semiMajorAxis * (1 - (eccentricity * eccentricity));

        if(semilatus <= 0) return;
        if(interpolations <= 0) interpolations = 10;
        if(eccentricity < 0) eccentricity = 0;

        periapsisDirection = Vector3.ProjectOnPlane(periapsisDirection, normal).normalized;
        var right = Vector3.Cross(periapsisDirection, normal).normalized;

        var prevlp = new Vector3();
        var prevrp = new Vector3();

        var num = interpolations;
        var thetadiff = Mathf.PI / num;
        float theta = 0;

        var breakn = false;

        for(var i = 0; i < num + 1; i++)
        {
          var cosTheta = Mathf.Cos(theta);
          var r = semilatus / (1 + (eccentricity * cosTheta));

          if(r < 0) { r *= -100; breakn = true; }

          var rvec = right * Mathf.Sin(theta) * r;
          var fvec = periapsisDirection * cosTheta * r;

          // Left side
          var lp = center - rvec + fvec;

          if(theta != 0) WorldPoints.Add(prevlp);
          WorldPoints.Add(lp);
          prevlp = lp;

          // Right side
          var rp = center + rvec + fvec;

          if(theta != 0) WorldPoints.Add(prevrp);
          WorldPoints.Add(rp);
          prevrp = rp;

          theta += thetadiff;

          if(breakn) break; // Prevents drawing the negative part of hyperbola
        }
      }

      /// <summary>
      /// Draws an elliptic orbit in world space using periapsis and apoapsis
      /// </summary>
      public static void ConicSectionUsingApses(Vector3 center, float periapsis, float apoapsis, Vector3 normal, Vector3 forward, int interpolations)
      {
        var a = (periapsis + apoapsis) / 2;
        var e = (apoapsis - periapsis) / (apoapsis + periapsis);

        // TODO: Add interpolations
        ConicSection(center, e, a, normal, forward, interpolations);
      }
    }
  }
}
