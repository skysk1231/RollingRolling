using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Camera cam;

    void Awake()
    {
        cam = Camera.main; // �̷��� ī�޶� �������ִ� ������, �Լ� ���ึ�� ã�ư��� ���� ���� �ֱ� ����!
    }

//# �巡�׸� �ϴ� ����
    void OnMouseDrag()
    {
        transform.position = GetMousePos(); // object�� ��ġ�� ���콺 ��ġ�� �̵�
    }

    Vector3 GetMousePos()
    {
//# ���� �߿��� �κ�
        var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        // ���� �������� ȭ��ȿ��� ���콺�� ��ǥ���� ������ mousePos�� �Ҵ�

        mousePos.z = 0;
        mousePos.x = (float)-9.5;// 2D�̱� ������ z�� 0����
        return mousePos; // ���콺�� ���� ��ǥ���� ��ȯ
    }
}