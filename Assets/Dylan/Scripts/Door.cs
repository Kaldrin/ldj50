using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] List<GameObject> listOfSwitches = new List<GameObject>();
    List<GameObject> hiddenSwitchList = new List<GameObject>();
    [SerializeField] float openSpeed;
    [SerializeField] Vector2 directionWhenOpening;

    void OnEnable()
    {
        Reset();
    }

    public void Reset()
    {
        //hiddenSwitchList = listOfSwitches;
        for (int i = 0; i < listOfSwitches.Count; i++)
        {
            hiddenSwitchList.Add(listOfSwitches[i]);
        }
    }

    public void RemoveFromListOfSwitches(GameObject switchToRemove)
    {
        hiddenSwitchList.Remove(switchToRemove);
        if (hiddenSwitchList.Count == 0)
            StartCoroutine(Opening());
    }

    void OpenAction()
    {
        Vector2 goal = Vector2.zero;
        if (directionWhenOpening == Vector2.up)
            goal = new Vector2(transform.position.x, transform.position.y + transform.localScale.y);
        else if (directionWhenOpening == Vector2.down)
            goal = new Vector2(transform.position.x, transform.position.y - transform.localScale.y);
        else if (directionWhenOpening == Vector2.left)
            goal = new Vector2(transform.position.x - transform.localScale.x, transform.position.y);
        else if (directionWhenOpening == Vector2.right)
            goal = new Vector2(transform.position.x + transform.localScale.x, transform.position.y);

        while (Vector2.Distance(transform.position, goal) != 0)
        {
            Vector2 pos = Vector2.MoveTowards(transform.position, goal, openSpeed * Time.deltaTime);
            transform.position = pos;
        }
    }

    IEnumerator Opening()
    {
        Vector2 goal = Vector2.zero;
        if (directionWhenOpening == Vector2.up)
            goal = new Vector2(transform.position.x, transform.position.y + transform.localScale.y);
        else if (directionWhenOpening == Vector2.down)
            goal = new Vector2(transform.position.x, transform.position.y - transform.localScale.y);
        else if (directionWhenOpening == Vector2.left)
            goal = new Vector2(transform.position.x - transform.localScale.x, transform.position.y);
        else if (directionWhenOpening == Vector2.right)
            goal = new Vector2(transform.position.x + transform.localScale.x, transform.position.y);

        while (Vector2.Distance(transform.position, goal) != 0)
        {
            Vector2 pos = Vector2.MoveTowards(transform.position, goal, openSpeed * Time.deltaTime);
            transform.position = pos;
            yield return null;
        }
    }
}
