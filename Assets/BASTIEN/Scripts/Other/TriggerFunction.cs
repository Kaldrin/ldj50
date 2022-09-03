using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Gives a simple interface to subscribe some functions to simple events
/// </summary>
public class TriggerFunction : MonoBehaviour
{
    public UnityEvent onDisable;
    public UnityEvent onDestroy;
    public UnityEvent onEnable;
    [SerializeField] float delay = 0f;


    private void OnDisable() => Invoke("OnDisableEvent", delay);
    void OnDisableEvent() => onDisable.Invoke();
    private void OnDestroy() => Invoke("OnDestroyEvent", delay);
    void OnDestroyEvent() => onDestroy.Invoke();
    private void OnEnable() => Invoke("OnEnableEvent", delay);
    void OnEnableEvent() => onEnable.Invoke();
}
