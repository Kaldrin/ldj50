using UnityEngine;
using UnityEditor;
using UnityEngine.Events;




/// <summary>
/// Detects when a player enters it and allows to trigger various events
/// </summary>
public class GeneralTrigger : MonoBehaviour
{
    [SerializeField] public UnityEvent onTriggerEnterEvent;
    [SerializeField] float delay = 0f;
    [SerializeField] Color color = Color.yellow;
    bool triggered = false;




    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered)
        {
            Invoke("TriggerEvent", delay);
            triggered = true;
        }
    }
    void TriggerEvent() => onTriggerEnterEvent.Invoke();
    public void Reset() => triggered = false;



    // EDITOR
    /// <summary>
    /// Display of the element in the scene
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        if (!triggered)
            Gizmos.DrawCube(transform.position, transform.localScale);
        else
            Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
