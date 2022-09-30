using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int maxLevel;
    public int lastLevel;
    public float volumeLevel;

    public GameData()
    {
        this.maxLevel = 0;
        this.lastLevel = 0;
        this.volumeLevel = .5f;
    }
}
