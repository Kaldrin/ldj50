using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour, IDataPersistence
{
    public static MainMenu instance;
    [SerializeField] GameObject hiddenWall;
    [SerializeField] GameObject lastLevelText;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        DataPersistenceManager.instance.LoadGame();
    }

    public void LoadData(GameData data)
    {
        Debug.Log(data.lastLevel);
        if (data.lastLevel > 0) SetUpMenuWhenPlayerHasAlreadyPlayed();
    }

    public void SaveData(ref GameData data)
    {

    }

    void SetUpMenuWhenPlayerHasAlreadyPlayed()
    {
        hiddenWall.SetActive(false);
        lastLevelText.SetActive(true);
    }
}
