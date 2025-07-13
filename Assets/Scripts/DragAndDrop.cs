using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Camera cam;

    void Awake()
    {
        cam = Camera.main; // 이렇게 카메라를 지정해주는 이유는, 함수 실행마다 찾아가는 수고를 덜어 주기 위해!
    }

//# 드래그를 하는 동안
    void OnMouseDrag()
    {
        transform.position = GetMousePos(); // object의 위치를 마우스 위치로 이동
    }

    Vector3 GetMousePos()
    {
//# 아주 중요한 부분
        var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        // 현재 보여지는 화면안에서 마우스의 좌표값을 가져와 mousePos에 할당

        mousePos.z = 0;
        mousePos.x = (float)-9.5;// 2D이기 때문에 z값 0으로
        return mousePos; // 마우스의 현제 좌표값은 반환
    }
}