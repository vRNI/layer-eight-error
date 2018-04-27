using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag
    : MonoBehaviour
{
    private void Update()
    {
        var flagTransform         = GetComponent< Transform >();
        var cameraLocalRotation   = Finder.GetCameraOrbitScript().LocalRotation;
        flagTransform.eulerAngles = new Vector3( 0.0f, cameraLocalRotation.x, 0.0f );
    }
}
