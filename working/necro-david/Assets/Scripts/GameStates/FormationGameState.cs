using UnityEngine;

public class FormationState
    : GameState
{
    /// <summary>
    /// The formation placement objects to display in the 3D scene ( object-pooling ).
    /// </summary>
    private static GameObject[] s_slotObjects;

    /// <summary>
    /// Drag'n'drop source for free fighter units.
    /// </summary>
    private static GameObject s_freeFighterUnits;
    
    /// <summary>
    /// Drag'n'drop source for free ranger units.
    /// </summary>
    private static GameObject s_freeRangerUnits;
    
    /// <summary>
    /// Drag'n'drop source for free mage units.
    /// </summary>
    private static GameObject s_freeMageUnits;

    public FormationState()
    {
    }

    public override void Entering()
    {
        base.Entering();

        // check hard-coded formation size
        if ( FormationParameters.FormationSlotSizeX % 2 == 0 || FormationParameters.FormationSlotSizeY % 2 == 0 )
        {
            ValidityManager.TerminateUnexpectedly( "Formation size must be odd ( x a and y dimension )." );
        }

        var prefabManager = ValidityManager.FindManager<PrefabManager>();

        // create placement objects if required
        if ( s_slotObjects == null ) { s_slotObjects = new GameObject[ FormationParameters.FormationSlotCount ]; }
        if ( s_freeFighterUnits == null ) { s_freeFighterUnits = prefabManager.GetFormationFreeFighterUnits(); }
        if ( s_freeRangerUnits == null ) { s_freeRangerUnits = prefabManager.GetFormationFreeRangerUnits(); }
        if ( s_freeMageUnits == null ) { s_freeMageUnits = prefabManager.GetFormationFreeMageUnits(); }

        // place slot grid
        int centerX = ( FormationParameters.FormationSlotSizeX - 1 ) / 2;
        int centerY = ( FormationParameters.FormationSlotSizeX - 1 ) / 2;

        for ( int x = 0; x < FormationParameters.FormationSlotSizeX; ++x )
        {
            for ( int y = 0; y < FormationParameters.FormationSlotSizeY; ++y )
            {
                // skip center position, because player is here
                if ( x == centerX && y == centerY ) { continue; }

                int index     = x + y * FormationParameters.FormationSlotSizeX;

                // create placement objects if required
                if ( s_slotObjects[ index ] == null ) { s_slotObjects[index] = prefabManager.GetFormationSlotPlacement(); }
                
                // enable object again before use
                s_slotObjects[ index ].SetActive( true );

                var transform = s_slotObjects[ index ].GetComponent< Transform >();

                int offsetX   = x - centerX;
                int offsetY   = y - centerY;

                // calculate grid position ( here as relative position )
                var position  = new Vector3( FormationParameters.FormationSlotDistance.x * offsetX, 0.0f, FormationParameters.FormationSlotDistance.y * offsetY );
                var rotation  = transform.rotation;

                transform.SetPositionAndRotation( position, rotation );
            }
        }

        // place free units objects
        s_freeFighterUnits.GetComponent< Transform >().position = new Vector3( -centerX * FormationParameters.FormationSlotDistance.x, 0.0f, ( centerY + 2 ) * FormationParameters.FormationSlotDistance.y );
        s_freeRangerUnits.GetComponent< Transform >().position  = new Vector3(        0 * FormationParameters.FormationSlotDistance.x, 0.0f, ( centerY + 2 ) * FormationParameters.FormationSlotDistance.y );
        s_freeMageUnits.GetComponent< Transform >().position    = new Vector3(  centerX * FormationParameters.FormationSlotDistance.x, 0.0f, ( centerY + 2 ) * FormationParameters.FormationSlotDistance.y );
    }

    public override void Update()
    {
        // todo follow player in grid here...
        if ( Input.GetButton( "Submit"  ) == true )
        {
            Camera.main.orthographic = true;
        }
        else if ( Input.GetButton( "Cancel" ) == true )
        {
            Camera.main.orthographic = false;
        }
    }
    
    public override void Exiting()
    {
        // disable placement objects after state exits to remove unnecessary overhead.
        foreach ( var @object in s_slotObjects )
        {
            @object.SetActive( false );
        }

        base.Exiting();
    }
}
