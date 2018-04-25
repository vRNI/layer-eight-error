using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour {

    private static MaterialManager instance;

    private MaterialManager() { }

    [SerializeField]
    Material[] m_materials;

    public static MaterialManager GetInstance()
    {
        if (instance == null)
            instance = new MaterialManager();

        return instance;
    }

    public Material GetMaterial(int idx)
    {
        return m_materials[idx];
    }
}
