using TMPro;
using UnityEngine;

public class NicknameFilter : MonoBehaviour
{
    public TMP_InputField nicknameInput;

    void Awake()
    {
        if (nicknameInput == null)
            nicknameInput = GetComponent<TMP_InputField>();
    }

    void Start()
    {
        nicknameInput.onValueChanged.AddListener(FilterNickname);
    }

    private void FilterNickname(string input)
    {
        string filtered = "";
        foreach (char c in input)
        {
            if ((c >= 'a' && c <= 'z') ||
                (c >= 'A' && c <= 'Z') ||
                (c >= '0' && c <= '9'))
            {
                filtered += c;
            }
        }

        if (filtered != input)
        {
            nicknameInput.text = filtered;  // 한글 입력 즉시 제거
        }
    }
}