using UnityEngine;

public class ground2 : MonoBehaviour
{

    public static ground2 Instance;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }
    


    private void Start()
    {    
            
        GroundSkinManager.Instance.ApplySkinTo(this);
    }

    public void SetSprite(Sprite newSprite)
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = newSprite;

        

        PolygonCollider2D oldCollider = GetComponent<PolygonCollider2D>();
        if (oldCollider != null)
            DestroyImmediate(oldCollider);

        gameObject.AddComponent<PolygonCollider2D>();
    }
  

}

