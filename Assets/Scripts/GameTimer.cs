using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float elapsedTime;

    void Start()
    {
        // PlayerPrefs에서 시간 가져오기
        elapsedTime = PlayerPrefs.GetFloat("elapsedTime", 0f);
        UpdateTimerText();
    }

    void UpdateTimerText()
    {
        timerText.text = $"Time: {elapsedTime:F2}"; // 소수점 2자리까지 표시
    }
}
