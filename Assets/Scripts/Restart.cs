using UnityEngine;
using UnityEngine.SceneManagement; // �� ��ȯ�� ���� �ʿ�

public class Restart : MonoBehaviour
{
    public void RestartGame()  // ��ư�� Ŭ���Ǹ� �����
    {
       
        SceneManager.LoadScene("StartScene"); // �� �̸��� ��Ȯ�� Ȯ���ϼ���!
    }
}
