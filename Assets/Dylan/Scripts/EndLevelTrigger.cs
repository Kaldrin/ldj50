using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

/// <summary>
/// Detects when a player enters it and ends the corresponding level, switching to the next one. Must be a grand child of the level GO.
/// </summary>
public class EndLevelTrigger : MonoBehaviour
{
    [SerializeField] Color color = Color.gray;
    [SerializeField] Level nextLevel = null;
    bool triggered = false;
    [SerializeField] int specificPlayerIndexForSpecialActions = 1;
    [SerializeField] public UnityEvent actionsIfSpecificPlayerIndexTriggeredIt;
    [SerializeField] float specialActionsDelay = 0f;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (transform.parent.parent.GetComponent<Level>() != nextLevel)
        {
            transform.parent.parent.GetComponent<Level>().ChangeLevel(other.transform.parent.GetComponent<Character>(), nextLevel);
            triggered = true;

            // Trigger special actions if specific player index triggered it
            if (other.transform.parent.GetComponent<PlayerController>().playerIndex == specificPlayerIndexForSpecialActions)
                Invoke("CallSpecialActions", specialActionsDelay);
        }
    }
    void CallSpecialActions() => actionsIfSpecificPlayerIndexTriggeredIt.Invoke();
    /// <summary>
    /// Display of the element in the scene
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
