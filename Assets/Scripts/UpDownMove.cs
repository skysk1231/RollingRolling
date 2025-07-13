using UnityEngine;

public class VerticalKeyMover : MonoBehaviour
{
    // 이동 속도 (초당 거리)
    public float moveSpeed = 7f;

    void Update()
    {
        Vector3 movement = Vector3.zero;

        // 위쪽 입력: W 키 또는 ↑ 방향키
        if (Input.GetKey(KeyCode.UpArrow))
        {
            movement += Vector3.up; // (0, 1, 0)
        }

        // 아래쪽 입력: S 키 또는 ↓ 방향키
        if (Input.GetKey(KeyCode.DownArrow))
        {
            movement += Vector3.down; // (0, -1, 0)
        }

        // 이동 적용
        transform.position += movement * moveSpeed * Time.deltaTime;
    }
}
