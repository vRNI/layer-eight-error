﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ DisallowMultipleComponent ]
[ RequireComponent( typeof( FormationConfiguration ) ) ]
public class FormationGridGrower 
    : MonoBehaviour
{
    [ Tooltip( "The threshold in percent, representing how many slots in your formation grid are filled, which makes your formation grid grow by one in each direction." ) ]
    [ SerializeField ]
    [ Range( 0.05f, 1.0f ) ]
    private float m_populationThreshold = 0.2f;
    
    private void Update()
    {
        // check if threshold is reached
        var formationConfiguration = GetComponent< FormationConfiguration >();
        var slotCountTotal         = formationConfiguration.GetSlotCount();
        var slotCountOccupied      = formationConfiguration.GetUnderlingUnits().Count;
        var relativeGridPopulation = slotCountOccupied / ( slotCountTotal * 1.0f );
        if ( relativeGridPopulation < m_populationThreshold ) { return; }

        // grow grid size
        formationConfiguration.GrowGridSize( 2, 2 );
    }
}
