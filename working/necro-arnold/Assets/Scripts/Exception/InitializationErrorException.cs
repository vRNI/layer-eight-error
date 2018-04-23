using System;

public class InitializationErrorException
    : Exception
{
    public InitializationErrorException( string a_message )
        : base( a_message )
    {
    }
}
