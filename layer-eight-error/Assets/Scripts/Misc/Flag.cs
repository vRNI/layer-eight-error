using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Flag
    : MonoBehaviour
{
    [ SerializeField ]
    private bool m_showWalkPath = false;

    private void Update()
    {
        var flagTransform         = GetComponent< Transform >();
        var cameraLocalRotation   = Finder.GetCameraOrbitScript().LocalRotation;
        flagTransform.eulerAngles = new Vector3( 0.0f, cameraLocalRotation.x, 0.0f );

        this.GetComponent< LineRenderer >().enabled = m_showWalkPath;

        VisualizePlayerNavigationPath();

        var navMeshAgent = Finder.GetPlayer().GetComponent< NavMeshAgent >();
        
         // Check if we've reached the destination
         if (navMeshAgent.pathPending == false)
         {
             if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
             {
                 if (!navMeshAgent.hasPath || Mathf.Abs(navMeshAgent.velocity.sqrMagnitude) <= float.Epsilon)
                 {
                     Destroy( gameObject );
                 }
             }
         }
    }

    private void VisualizePlayerNavigationPath()
    {
        var nav = Finder.GetPlayer().GetComponent< NavMeshAgent >();
 
        var line = this.GetComponent< LineRenderer >();
        line.material = new Material( Shader.Find( "Sprites/Default" ) );
        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(new Color( 44.0f / 255.0f, 44.0f / 255.0f, 255.0f / 255.0f ), 0.0f), new GradientColorKey(new Color( 154.0f / 255.0f, 154.0f / 255.0f, 154.0f / 255.0f ), 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 1.0f), new GradientAlphaKey(alpha, 0.0f) }
            );
        line.colorGradient = gradient;

        var path = nav.path;

        line.positionCount = path.corners.Length;

        for ( int i = 0; i < path.corners.Length; i++ )
        {
            line.SetPosition( i, path.corners[ i ] );
        }
    }
}
