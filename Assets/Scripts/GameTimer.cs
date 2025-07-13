using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float elapsedTime;

    void Start()
    {
        // PlayerPrefs���� �ð� ��������
        elapsedTime = PlayerPrefs.GetFloat("elapsedTime", 0f);
        UpdateTimerText();
    }

    void UpdateTimerText()
    {
        timerText.text = $"Time: {elapsedTime:F2}"; // �Ҽ��� 2�ڸ����� ǥ��
    }
}
