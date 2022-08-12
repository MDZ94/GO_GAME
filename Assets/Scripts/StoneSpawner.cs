using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSpawner : Singleton<StoneSpawner>
{
    [SerializeField]
    private GameObject token1Prefab;

    public GameObject Token1Prefab
    {
        get
        {
            return token1Prefab;
        }
    }

    [SerializeField]
    private GameObject token2Prefab;

    public GameObject Token2Prefab
    {
        get
        {
            return token2Prefab;
        }
    }
}
