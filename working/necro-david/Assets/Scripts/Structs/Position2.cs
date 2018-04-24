using System;

[ Serializable ]
public struct Position2
{
    public static readonly Position2 Invalid = new Position2( int.MinValue, int.MinValue );

    public int X;
    public int Z;

    public Position2( int a_x, int a_z )
    {
        X = a_x;
        Z = a_z;
    }
}
