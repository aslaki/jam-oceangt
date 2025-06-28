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
        rb.AddForce(-Physics2D.gravity * rb.mass * 3);
    }

    private void OnGameStateChanged(GameState newState)
    {
        canMove = newState == GameState.Game;
    }
}
