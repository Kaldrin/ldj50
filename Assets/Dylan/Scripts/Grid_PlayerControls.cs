using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_PlayerControls : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Transform moveTarget;
    [SerializeField] float padding = 1;
    [SerializeField] Vector2 targetPosition;
    [SerializeField] Vector2 nextTargetPosition;
    bool isMoving = false;
    [SerializeField] Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        //targetPosition = new Vector2(moveTarget.position.x + padding, moveTarget.position.y);
        //nextTargetPosition = new Vector2(targetPosition.x + padding, targetPosition.y);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPos = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        transform.position = newPos;

        if (nextTargetPosition.x > targetPosition.x)
            direction = Vector2.right;
        else if (nextTargetPosition.x < targetPosition.x)
            direction = Vector2.left;
        else if(nextTargetPosition.y > targetPosition.y)
            direction = Vector2.up;
        else if(nextTargetPosition.y < targetPosition.y)
            direction = Vector2.down;

        if (Vector2.Distance(transform.position, targetPosition) == 0)
        {
            targetPosition = nextTargetPosition;
            nextTargetPosition = new Vector2(transform.position.x + direction.x, transform.position.y + direction.y);
        }

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            nextTargetPosition = targetPosition + (new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical") * padding));

        moveTarget.position = nextTargetPosition;
    }
}