using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    bool moveUp = false;
    bool moveDown = false;
    bool moveLeft = false;
    bool moveRight = false;

    [SerializeField]
    Rigidbody2D rb;

    [SerializeField]
    float speed = 5f;

    bool canMove = false;

    [SerializeField]
    bool isFacingLeft = true;

    [SerializeField]
    Transform playerHead;

    [SerializeField]
    float angle;

    [SerializeField]
    Transform[] relatedTransforms;

    [SerializeField]
    Transform headRotationPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    // Update is called once per frame
    void Update()
    {
        moveDown = Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed;
        moveUp = Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed;
        moveLeft = Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed;
        moveRight = Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed;
    }

    void FixedUpdate()
    {


        rb.AddForce(new Vector2(
            (moveRight ? 1 : 0) - (moveLeft ? 1 : 0),
            (moveUp ? 1 : 0) - (moveDown ? 1 : 0)
        ) * speed);

        //Negate gravity by adding force in the opposite direction of gravity
        rb.AddForce(-Physics2D.gravity * rb.mass * 5f);
        if (moveLeft || moveRight)
        {
            isFacingLeft = moveLeft && !moveRight;
        }

        if(isFacingLeft)
        {
            foreach (var t in relatedTransforms)
            {
                t.localScale= new Vector3(Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
            }
        }
        else
        {
            foreach (var t in relatedTransforms)
            {
                t.localScale = new Vector3(Mathf.Abs(t.localScale.x) * -1, t.localScale.y, t.localScale.z);
            }
        }
        
        RotateHeadTowardsMouse();
    }

    private void OnGameStateChanged(GameState newState)
    {
        canMove = newState == GameState.Game;
    }

    private void RotateHeadTowardsMouse()
    {
        // Get mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0f; // Ensure z is 0 for 2D
        Vector2 direction;
        // Calculate direction from head to mouse
        if (isFacingLeft)
        {
            direction = -((mousePosition - playerHead.position).normalized);
        }
        else
        {
            // If facing right, calculate direction normally
           direction = (mousePosition - playerHead.position).normalized;
        }

        // Calculate angle in degrees
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, -45f, 45f);
        // Add angle restrictions 

       
        // Apply rotation to the head
        playerHead.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
