using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ RequireComponent( typeof( Camera ) ) ]
public class OrthoPerspectiveSwitcher
    : MonoBehaviour
{
    private Matrix4x4 ortho;
    private Matrix4x4 perspective;
           
    public float fov   = 60f;
    public float near  = .3f;
    public float far   = 1000f;
    public float orthographicSize = 10f;
 
    private float aspect;
    private bool orthoOn;
 
    private void Awake ()
    {
        aspect = ( Screen.width + 0.0f ) / ( Screen.height + 0.0f );
 
        perspective = gameObject.GetComponent< Camera >().projectionMatrix;
 
        ortho = Matrix4x4.Ortho( -orthographicSize * aspect, orthographicSize * aspect, -orthographicSize, orthographicSize, near, far );
        orthoOn = false;
    }
 
    private void Update ()
    {
        if ( Input.GetKeyDown( KeyCode.P ) )
        {
            orthoOn = !orthoOn;
            if ( orthoOn )
                BlendToMatrix( ortho, 1f );
            else
                BlendToMatrix( perspective, 1f );
        }
    }
 
    private static Matrix4x4 MatrixLerp ( Matrix4x4 from, Matrix4x4 to, float time )
    {
        var ret = new Matrix4x4();
        int i;
        for (i = 0; i < 16; i++)
            ret[i] = Mathf.Lerp(from[i], to[i], time);
        return ret;
    }
 
    private IEnumerator LerpFromTo ( Matrix4x4 src, Matrix4x4 dest, float duration)
    {
        var startTime = Time.time;
        while ( Time.time - startTime < duration )
        {
            gameObject.GetComponent< Camera >().projectionMatrix = MatrixLerp( src, dest, ( Time.time - startTime ) / duration );
        
            yield return 1;
        }

        gameObject.GetComponent< Camera >().projectionMatrix = dest;
    }
 
    public Coroutine BlendToMatrix ( Matrix4x4 targetMatrix, float duration )
    {
        StopAllCoroutines();

        return StartCoroutine( LerpFromTo( gameObject.GetComponent< Camera >().projectionMatrix, targetMatrix, duration ) );
    }
}
