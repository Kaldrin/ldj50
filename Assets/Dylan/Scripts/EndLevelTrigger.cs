using UnityEngine;
using UnityEditor;

/// <summary>
/// Detects when a player enters it and ends the corresponding level, switching to the next one. Must be a grand child of the level GO.
/// </summary>
public class EndLevelTrigger : MonoBehaviour
{
    [SerializeField] Color color = Color.gray;
    private void OnTriggerEnter2D(Collider2D other) => transform.parent.parent.GetComponent<Level>().ChangeLevel();
    /// <summary>
    /// Display of the element in the scene
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
