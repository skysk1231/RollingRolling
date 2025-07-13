using UnityEngine;

public class WSMove : MonoBehaviour
{
    public float moveSpeed = 7f;


    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.zero;

        // 위로 이동
        if (Input.GetKey(KeyCode.W))
        {
            movement += Vector3.up; // (0, 1, 0)
        }

        // 아래로 이동
        if (Input.GetKey(KeyCode.S))
        {
            movement += Vector3.down; // (0, -1, 0)
        }

        // 실제 이동 적용 (프레임 독립)
        transform.position += movement * moveSpeed * Time.deltaTime;
    }
}
