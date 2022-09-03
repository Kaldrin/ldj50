using UnityEngine;

// Reusable script

/// <summary>
/// Gives public functions to set objects positions from events & co.
/// </summary>
public class SetPositionFunctions : MonoBehaviour
{
    [SerializeField] Vector3 newPosition = Vector3.zero;
    [SerializeField] float delay = 5f;
    public void SetObjectPosition(Transform transformToMove) => transformToMove.position = newPosition;
    public void SetObjectPositionToSelf(Transform transformToMove) => transformToMove.position = transform.position;

    public void SetSelfToNewPos(Vector3 newPos) => transform.position = newPos;
    public void SetSelfToNewPos(Transform newPos) => transform.position = newPos.position;
}
