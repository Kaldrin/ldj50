using UnityEngine;

/// <summary>
/// Controller to receive inputs from a player. Is linked to a character component to control it.
/// </summary>
[RequireComponent(typeof(Character))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Character characterToControl = null;
    [SerializeField] int playerIndex = 0;


    [Header("INPUT AXIS")]
    [SerializeField] private string horizontal = "Horizontal";
    [SerializeField] private string vertical = "Vertical";
    [SerializeField] private string cancel = "Cancel";
    
    
    // INPUTS
    private float xValue = 0f;
    private float yValue = 0f;
    private bool pause = false;









    private void OnEnable() => GetMissingComponents();
    void Update()
    {
        GetInputValues();
        SendCharacterInputs();
    }
    
    
    
    
    // GET INPUT VALUES
    /// <summary>
    /// Get the input values for this player from the input axis
    /// </summary>
    void GetInputValues()
    {
        xValue = Input.GetAxis(horizontal);
        yValue = Input.GetAxis(vertical);
        pause = Input.GetButtonDown(cancel);
        /*
        if (Input.GetButtonDown(cancel))
            characterToControl.PressPause();
        */
    }

    

    /// <summary>
    /// Sends input values to the character
    /// </summary>
    void SendCharacterInputs()
    {
        if (characterToControl)
        {
            characterToControl.ReceiveMovementInputs(xValue, yValue);
            characterToControl.ReceivePauseInput(pause);
            //Vector2 newValue = new Vector2(xValue, yValue);
            //characterToControl.receivedMovementVector = newValue;
        }
    }




    // EDITOR
    /// <summary>
    /// Automatically assigns some components
    /// </summary>
    void GetMissingComponents()
    {
        if (!characterToControl)
            characterToControl = GetComponent<Character>();
    }
    private void OnValidate() => GetMissingComponents();
}
