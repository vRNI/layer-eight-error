
using UnityEngine;

public static class FormationParameters
{
    /// <summary>
    /// The formation slot size in x direction.
    /// </summary>
    public const int FormationSlotSizeX = 9;

    /// <summary>
    /// The formation slot size in y direction.
    /// </summary>
    public const int FormationSlotSizeY = 9;

    /// <summary>
    /// The formation slot count ( X * Y ).
    /// </summary>
    public const int FormationSlotCount = FormationSlotSizeX * FormationSlotSizeY;

    /// <summary>
    /// The distance ( in x and in y direction ) from one slot to another in 2D space.
    /// </summary>
    public static readonly Vector2 FormationSlotDistance = new Vector2( 1.0f, 1.0f );
}
