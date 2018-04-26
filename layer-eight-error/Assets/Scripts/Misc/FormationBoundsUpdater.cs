using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationBoundsUpdater
    : MonoBehaviour
{
    private bool m_isFirstUpdate = true;

    private void Update()
    {
        if ( m_isFirstUpdate == false ) { return; }

        UpdateFormationBoundingBox();
        m_isFirstUpdate = true;
    }

    /// <summary>
    /// Updates the bounding box of the formation.
    /// </summary>
    public void UpdateFormationBoundingBox()
    {
        // update enemy detector collider size
        var detectorCollider = GetComponent< BoxCollider >();
        if( detectorCollider != null )
        {
            // calculate current formation width and height
            var left   = int.MaxValue;
            var right  = int.MinValue;
            var bottom = int.MaxValue;
            var top    = int.MinValue;

            var formationConfiguration = transform.parent.GetComponent< FormationConfiguration >();
            var slotDistance           = formationConfiguration.GetSlotDistance();

            foreach ( var entity in formationConfiguration.GetUnderlingUnits() )
            {
                // update slot position boundaries
                var slotPosition = entity.GetFormationSlot();
                left   = Mathf.Min( left, slotPosition.X );
                right  = Mathf.Max( right, slotPosition.X );
                bottom = Mathf.Min( bottom, slotPosition.Z );
                top    = Mathf.Max( top, slotPosition.Z );
            }

            var width   = ( right - left + 6 ) * slotDistance.x;
            var height  = ( top - bottom + 6 ) * slotDistance.y;
            // calculate center by dividing by 2
            var centerX = ( right + left ) * slotDistance.x / 2.0f;
            var centerZ = ( top + bottom ) * slotDistance.y / 2.0f;

            detectorCollider.center = new Vector3( centerX, 0.0f, centerZ );
            detectorCollider.size   = new Vector3( width, Mathf.Max( width, height ), height );
        }
    }
}
