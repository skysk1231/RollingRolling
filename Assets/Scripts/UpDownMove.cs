using UnityEngine;

public class VerticalKeyMover : MonoBehaviour
{
    // �̵� �ӵ� (�ʴ� �Ÿ�)
    public float moveSpeed = 7f;

    void Update()
    {
        Vector3 movement = Vector3.zero;

        // ���� �Է�: W Ű �Ǵ� �� ����Ű
        if (Input.GetKey(KeyCode.UpArrow))
        {
            movement += Vector3.up; // (0, 1, 0)
        }

        // �Ʒ��� �Է�: S Ű �Ǵ� �� ����Ű
        if (Input.GetKey(KeyCode.DownArrow))
        {
            movement += Vector3.down; // (0, -1, 0)
        }

        // �̵� ����
        transform.position += movement * moveSpeed * Time.deltaTime;
    }
}
