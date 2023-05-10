using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornersAndCorridorsManager : MonoBehaviour
{
    [SerializeField] GameObject UP;
    [SerializeField] GameObject DOWN;
    [SerializeField] GameObject LEFT;
    [SerializeField] GameObject RIGHT;
    public static CornersAndCorridorsManager instance;
    public int _nextLevelIndex;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCorrespondingStructure(EndingSide.Side side, Vector3 spawnLocation, int nextLevel)
    {
        _nextLevelIndex = nextLevel;
        GameObject structure = null;
        switch (side)
        {
            case EndingSide.Side.UP:
            structure = DOWN;
            break;

            case EndingSide.Side.DOWN:
            structure = UP;
            break;

            case EndingSide.Side.LEFT:
            structure = RIGHT;
            break;

            case EndingSide.Side.RIGHT:
            structure = LEFT;
            break;
        }
        structure.transform.position = spawnLocation;
        structure.SetActive(true);
    }
}
