using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Rigidbody2D rb;

    private float maxVelocity;
    private float movementSpeed;
    private float jumpForce;

    private Player playerRef;
    private Vector2 movementVector = Vector2.zero;
    private int index = 0;
    private bool isMoving = false;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        maxVelocity = playerRef.maxVelocity;
        movementSpeed = playerRef.movementSpeed;
        jumpForce = playerRef.jumpForce;

        if (InputRecorder.RecordedInputList.Count > 0)
            StartCoroutine(ReplayPlayerInput());
    }

    private IEnumerator ReplayPlayerInput()
    {
        if (index > InputRecorder.RecordedInputList.Count - 1)
        {
            if (playerRef.canLoopInput == true)
                index = 0;
            else
            {
                StopAllCoroutines();
            }
        }    

        float seconds = InputRecorder.RecordedInputList[index].timeSinceLastAction;
        yield return new WaitForSeconds(seconds);

        string inputName = InputRecorder.RecordedInputList[index].actionName;
        Vector2 inputVector = InputRecorder.RecordedInputList[index].inputVector;

        if (inputName == "Move")
        {
            movementVector = inputVector;
            isMoving = true;
        }
        else if (inputName == "MoveCancel")
        {
            movementVector = inputVector;
            isMoving = false;
        }
        else if (inputName == "Jump")
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        index++;
        StartCoroutine(ReplayPlayerInput());
    }

    private bool AtMaxSpeed()
    {
        float currentXSpeed = rb.velocity.x;
        float currentYSpeed = rb.velocity.y;

        if (Mathf.Abs(currentYSpeed) > maxVelocity)
            rb.velocity = new Vector2(currentXSpeed, maxVelocity * Mathf.Sign(currentYSpeed));

        if (Mathf.Abs(currentXSpeed) > maxVelocity)
        {
            rb.velocity = new Vector2(maxVelocity * Mathf.Sign(currentXSpeed), currentYSpeed);
            return true;
        }

        return false;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void FixedUpdate()
    {
        if (isMoving == true && AtMaxSpeed() == false)
            rb.AddForce(movementVector * movementSpeed);
    }
}
