using UnityEngine;

public class Bubble : MonoBehaviour
{
    public int colorIndex;
    public int gridX = -1;
    public int gridY = -1;
    public bool isMoving = false;
    public bool isFalling = false;

    private Vector2 velocity;
    private float speed = 16f;
    private float fallSpeed = 0f;
    private SpriteRenderer sr;

    public static readonly Color[] Palette = new Color[]
    {
        new Color(0.95f, 0.25f, 0.30f),
        new Color(0.20f, 0.75f, 0.95f),
        new Color(0.25f, 0.90f, 0.45f),
        new Color(0.98f, 0.80f, 0.15f),
        new Color(0.75f, 0.30f, 0.95f)
    };

    public void Init(int cIndex, Sprite sprite)
    {
        colorIndex = cIndex;
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.color = Palette[colorIndex];
        sr.sortingOrder = 2;

        CircleCollider2D col = gameObject.AddComponent<CircleCollider2D>();
        col.radius = 0.45f;
        col.isTrigger = true;
    }

    public void Launch(Vector2 direction)
    {
        velocity = direction.normalized * speed;
        isMoving = true;
    }

    void Update()
    {
        if (isFalling)
        {
            fallSpeed += 25f * Time.deltaTime;
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            if (transform.position.y < -7f) Destroy(gameObject);
            return;
        }

        if (!isMoving) return;

        transform.position += (Vector3)(velocity * Time.deltaTime);
        float halfWidth = BubbleGameManager.Instance.boardHalfWidth - 0.5f;

        if (transform.position.x <= -halfWidth)
        {
            transform.position = new Vector3(-halfWidth, transform.position.y, 0);
            velocity.x = Mathf.Abs(velocity.x);
        }
        else if (transform.position.x >= halfWidth)
        {
            transform.position = new Vector3(halfWidth, transform.position.y, 0);
            velocity.x = -Mathf.Abs(velocity.x);
        }

        if (transform.position.y >= BubbleGameManager.Instance.topY)
        {
            SnapToGrid();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isMoving) return;
        Bubble otherBubble = other.GetComponent<Bubble>();
        if (otherBubble != null && !otherBubble.isMoving && !otherBubble.isFalling)
        {
            SnapToGrid();
        }
    }

    private void SnapToGrid()
    {
        isMoving = false;
        velocity = Vector2.zero;
        BubbleGameManager.Instance.PlaceBubbleOnGrid(this);
    }
}
