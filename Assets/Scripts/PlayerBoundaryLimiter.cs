using UnityEngine;

public class PlayerBoundaryLimiter : MonoBehaviour
{
    public GameObject square; 
    public GameObject gameManager; 

    private Bounds bounds;
    private float minY = -4f;
    private float maxY = 4f;

    private void Start()
    {
        if (square.TryGetComponent<Collider2D>(out Collider2D col))
        {
            bounds = col.bounds;
        }
        else
        {
            Debug.LogError("square 오브젝트에 Collider2D가 없습니다!");
        }
    }

    private void LateUpdate()
    {
        Vector3 pos = transform.position;

        
        if (pos.y < minY || pos.y > maxY)
        {
            Destroy(gameObject); 
            gameManager.GetComponent<GameManager>().isGameOver = true;
            return;
        }

       
        pos.x = Mathf.Clamp(pos.x, bounds.min.x, bounds.max.x);
        pos.y = Mathf.Clamp(pos.y, bounds.min.y, bounds.max.y); 

        transform.position = pos;
    }
}
