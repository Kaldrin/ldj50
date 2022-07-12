using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSaveAndLoadSystem : MonoBehaviour, IDataPersistence
{
    public int maxLevel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            maxLevel++;
        if(Input.GetKeyDown(KeyCode.Alpha0))
            maxLevel = 0;
        if(Input.GetKeyDown(KeyCode.R))
            DataPersistenceManager.instance.ResetSave();
    }

    public void LoadData(GameData data)
    {
        this.maxLevel = data.maxLevel;
    }

    public void SaveData(ref GameData data)
    {
        data.maxLevel = this.maxLevel;
    }
}
