using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 필요
public class Player : MonoBehaviour
{
    public GameObject GameManager;

    public static Player Instance;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        SkinManager.Instance.ApplySkinTo(this);
    }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("enemy"))
        {
            Destroy(gameObject);
            GameManager.GetComponent<GameManager>().isGameOver = true;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("GameOverZone"))
        {
            Destroy(gameObject);
            GameManager.GetComponent<GameManager>().isGameOver = true;
        }
    }
    public void SetSprite(Sprite newSprite)
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = newSprite;
    }

}
