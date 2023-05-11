using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public bool canLoopInput = false;
    public TextMeshProUGUI loopText;
    public GameObject ghostPrefab;
    public Transform spawnTransform;
    public float maxVelocity = 10f;
    public float movementSpeed = 12f;
    public float jumpForce = 7f;

    private Rigidbody2D rb;
    private Vector2 movementVector = Vector2.zero;
    private bool isAirborne = false;
    private bool isMoving = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        InputRecorder.Init();
    }

    public void DebugSpawnGhost(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InputRecorder.SavePlayerInput();
            GameObject newGhost = Instantiate(ghostPrefab);
            newGhost.transform.position = spawnTransform.position;
        }
    }

    public void DebugToogleLoopInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            canLoopInput = !canLoopInput;

            if (canLoopInput == true)
                loopText.text = "Y";
            else
                loopText.text = "N";
        }
    }

    public void ReloadScene(InputAction.CallbackContext context)
    {
        if (context.performed)
            SceneManager.LoadScene("SampleScene");
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            movementVector = context.ReadValue<Vector2>();
            isMoving = true;

            InputRecorder.AddPlayerInput("Move", movementVector);
        }
            
        if (context.canceled)
        {
            movementVector = context.ReadValue<Vector2>();
            isMoving = false;

            InputRecorder.AddPlayerInput("MoveCancel", movementVector);
        }   
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isAirborne == false)
        {
            isAirborne = true;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            InputRecorder.AddPlayerInput("Jump", movementVector);
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform" || collision.gameObject.tag == "Ghost")
            isAirborne = false;
    }

    private void FixedUpdate()
    {
        if (isMoving == true && AtMaxSpeed() == false)
            rb.AddForce(movementVector * movementSpeed);
    }
}
