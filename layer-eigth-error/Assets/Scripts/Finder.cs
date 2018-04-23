using UnityEngine;

public static class Finder
{
    public static GameStateManager GetGameStateManager()
    {
        return Object.FindObjectOfType< GameStateManager >();
    }

    public static FormationConfiguration GetFormationConfiguration()
    {
        return Object.FindObjectOfType< FormationConfiguration >();
    }

    public static OrthoPerspectiveSwitcher GetOrthoPerspectiveSwitcher()
    {
        return Camera.main.GetComponent< OrthoPerspectiveSwitcher >();
    }

    public static Vector3 GetPlayerPosition()
    {
        return GameObject.FindGameObjectWithTag( TagName.Player ).GetComponent< Transform >().position;
    }
}
