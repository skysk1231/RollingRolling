using UnityEngine;

public class WSMove : MonoBehaviour
{
    public float moveSpeed = 7f;


    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.zero;

        // ���� �̵�
        if (Input.GetKey(KeyCode.W))
        {
            movement += Vector3.up; // (0, 1, 0)
        }

        // �Ʒ��� �̵�
        if (Input.GetKey(KeyCode.S))
        {
            movement += Vector3.down; // (0, -1, 0)
        }

        // ���� �̵� ���� (������ ����)
        transform.position += movement * moveSpeed * Time.deltaTime;
    }
}
