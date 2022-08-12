using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ### GAME SHOULD ALWAYS START FROM PRELOAD SCENE ###
/// use only in preload scene. Used to persists all settings which should be accesable in all scenes.
/// chosen settings, score
/// </summary>
public class DDOL : MonoBehaviour
{

    public void Awake() {
        DontDestroyOnLoad(gameObject);
    }

}
