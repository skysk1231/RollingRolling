using UnityEngine;
using UnityEngine.SceneManagement; // �� ��ȯ�� ���� �ʿ�

public class MainMenu : MonoBehaviour
{
    public void StartGame()  // ��ư�� Ŭ���Ǹ� �����
    {
        SceneManager.LoadScene("GameScene"); // �� �̸��� ��Ȯ�� Ȯ���ϼ���!
    }
}
