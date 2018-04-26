using System;

[Serializable]
public struct Position2
{
    public static readonly Position2 invalid = new Position2(int.MinValue,int.MinValue);

    public int X;
    public int Z;

    public Position2(int a_x, int a_z)
    {
        X = a_x;
        Z = a_z;
    }

    public static bool operator ==(Position2 obj1, Position2 obj2)
    {
        if (obj1.X != obj2.X)
        {
            return false;
        }

        if (obj1.Z != obj2.Z)
        {
            return false;
        }

        return true;
    }

    public static bool operator !=(Position2 obj1, Position2 obj2)
    {
        return !(obj1 == obj2);
    }


}
