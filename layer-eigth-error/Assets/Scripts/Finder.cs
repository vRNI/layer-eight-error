using UnityEngine;

public static class Finder
{
    public static GameStateManager GetGameStateManager()
    {
        return Object.FindObjectOfType< GameStateManager >();
    }

    public static OrthoPerspectiveSwitcher GetOrthoPerspectiveSwitcher()
    {
        return Camera.main.GetComponent< OrthoPerspectiveSwitcher >();
    }
}
