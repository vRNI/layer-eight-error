
using UnityEngine;

public static class MathUtil
{
    public static bool RaycastFromMousePointer( out RaycastHit a_hit )
    {
        // cast ray against scene
        var ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            
        return Physics.Raycast( ray, out a_hit );
    }
}
