using System.Numerics;
using UnityEngine;
using UnityEngine.Video;
using Vector2 = UnityEngine.Vector2;

public class MovementController : MonoBehaviour
{
    public new Rigidbody2D rigidbody2D{ get; private set; }
    private Vector2 direction = Vector2.down;
    public float speed = 5f;
    public KeyCode inputUp = KeyCode.W;
    public KeyCode inputDown = KeyCode.S;
    public KeyCode inputLeft = KeyCode.A;
    public KeyCode inputRight = KeyCode.D;

    public AnimatedSpriteRenderer animatedSpriteRendererUp;
    public AnimatedSpriteRenderer animatedSpriteRendererDown;
    public AnimatedSpriteRenderer animatedSpriteRendererLeft;
    public AnimatedSpriteRenderer animatedSpriteRendererRight;
    public AnimatedSpriteRenderer animatedSpriteRendererDeath;
    private AnimatedSpriteRenderer activeSpriteRenderer;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        activeSpriteRenderer = animatedSpriteRendererDown;
    }

    private void Update()
    {
        if (Input.GetKey(inputUp))
        {
            Setdirection(Vector2.up, animatedSpriteRendererUp);
        }
        else if (Input.GetKey(inputDown))
        {
            Setdirection(Vector2.down, animatedSpriteRendererDown);
        }
        else if (Input.GetKey(inputLeft))
        {
            Setdirection(Vector2.left, animatedSpriteRendererLeft);
        }
        else if (Input.GetKey(inputRight))
        {
            Setdirection(Vector2.right, animatedSpriteRendererRight);
        }
        else
        {
            Setdirection(Vector2.zero, activeSpriteRenderer);
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody2D.position;
        Vector2 translation = direction * speed * Time.fixedDeltaTime;

        rigidbody2D.MovePosition(position + translation);
    }
    private void Setdirection(Vector2 newDirection, AnimatedSpriteRenderer spriteRenderer)
    {
        direction = newDirection;

        animatedSpriteRendererUp.enabled = spriteRenderer == animatedSpriteRendererUp;
        animatedSpriteRendererDown.enabled = spriteRenderer == animatedSpriteRendererDown;
        animatedSpriteRendererLeft.enabled = spriteRenderer == animatedSpriteRendererLeft;
        animatedSpriteRendererRight.enabled = spriteRenderer == animatedSpriteRendererRight;
        activeSpriteRenderer = spriteRenderer;
        activeSpriteRenderer.idle = direction == Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            DeathSequence();
        }
    }

    private void DeathSequence()
    {
        enabled = false;
        GetComponent<BombContrller>().enabled = false;
        animatedSpriteRendererUp.enabled = false;
        animatedSpriteRendererDown.enabled = false;
        animatedSpriteRendererLeft.enabled = false;
        animatedSpriteRendererRight.enabled = false;
        animatedSpriteRendererDeath.enabled = true;

        Invoke(nameof(OnDeathSequenceEnded), 1.25f);
    }

    private void OnDeathSequenceEnded()
    {
        gameObject.SetActive(false);
        FindObjectOfType<GameManager>().CheckWinState();
    }
}
