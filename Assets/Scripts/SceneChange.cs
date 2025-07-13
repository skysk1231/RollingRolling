using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환을 위해 필요

public class MainMenu : MonoBehaviour
{
    public void StartGame()  // 버튼이 클릭되면 실행됨
    {
        SceneManager.LoadScene("GameScene"); // 씬 이름을 정확히 확인하세요!
    }
}
