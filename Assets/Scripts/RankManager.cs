using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankManager : MonoBehaviour
{
    public TMP_InputField nicknameInput;
    public Button submitButton;

    private float elapsedTime;

    private void Start()
    {
        // 타이머 기록 불러오기
        elapsedTime = PlayerPrefs.GetFloat("elapsedTime", 0f);

        // 버튼 비활성화 (초기화 전까지)
        submitButton.interactable = false;

        // FirebaseInit.Init()을 직접 호출
        FirebaseInit.Init();

        // 초기화 기다리는 코루틴 시작
        StartCoroutine(WaitForFirebaseInit());
    }

    private IEnumerator WaitForFirebaseInit()
    {
        float timeout = 10f; // 10초 타임아웃
        float timer = 0f;

        while ((!FirebaseInit.IsInitialized || FirebaseInit.DB == null) && timer < timeout)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (!FirebaseInit.IsInitialized || FirebaseInit.DB == null)
        {
            Debug.LogError("Firebase 초기화 실패 또는 시간 초과");
            yield break;
        }

        Debug.Log("Firebase 초기화 성공. 버튼 활성화");
        submitButton.interactable = true;

        // 리스너 추가 (버튼에서 OnClick 등록해도 무방)
        submitButton.onClick.AddListener(SubmitRank);
    }

    public void SubmitRank()
    {
        Debug.Log("SubmitRank() 호출됨");

        if (FirebaseInit.DB == null)
        {
            Debug.LogError("FirebaseInit.DB는 null입니다.");
            return;
        }

        string nickname = nicknameInput.text.Trim();
        if (string.IsNullOrEmpty(nickname))
        {
            Debug.LogWarning("닉네임을 입력하세요.");
            return;
        }

        SaveRank(nickname);
    }

    private void SaveRank(string nickname)
    {
        string key = FirebaseInit.DB.Child("rankings").Push().Key;

        Dictionary<string, object> rankData = new Dictionary<string, object>
        {
            { "nickname", nickname },
            { "elapsedTime", elapsedTime },
            { "timestamp", ServerValue.Timestamp }
        };

        FirebaseInit.DB.Child("rankings").Child(key).SetValueAsync(rankData).ContinueWith(task =>
        {
            if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
            {
                Debug.Log("랭킹 등록 완료");
            }
            else
            {
                Debug.LogError("랭킹 등록 실패: " + task.Exception);
            }
        });
    }
}
