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
        // Ÿ�̸� ��� �ҷ�����
        elapsedTime = PlayerPrefs.GetFloat("elapsedTime", 0f);

        // ��ư ��Ȱ��ȭ (�ʱ�ȭ ������)
        submitButton.interactable = false;

        // FirebaseInit.Init()�� ���� ȣ��
        FirebaseInit.Init();

        // �ʱ�ȭ ��ٸ��� �ڷ�ƾ ����
        StartCoroutine(WaitForFirebaseInit());
    }

    private IEnumerator WaitForFirebaseInit()
    {
        float timeout = 10f; // 10�� Ÿ�Ӿƿ�
        float timer = 0f;

        while ((!FirebaseInit.IsInitialized || FirebaseInit.DB == null) && timer < timeout)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (!FirebaseInit.IsInitialized || FirebaseInit.DB == null)
        {
            Debug.LogError("Firebase �ʱ�ȭ ���� �Ǵ� �ð� �ʰ�");
            yield break;
        }

        Debug.Log("Firebase �ʱ�ȭ ����. ��ư Ȱ��ȭ");
        submitButton.interactable = true;

        // ������ �߰� (��ư���� OnClick ����ص� ����)
        submitButton.onClick.AddListener(SubmitRank);
    }

    public void SubmitRank()
    {
        Debug.Log("SubmitRank() ȣ���");

        if (FirebaseInit.DB == null)
        {
            Debug.LogError("FirebaseInit.DB�� null�Դϴ�.");
            return;
        }

        string nickname = nicknameInput.text.Trim();
        if (string.IsNullOrEmpty(nickname))
        {
            Debug.LogWarning("�г����� �Է��ϼ���.");
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
                Debug.Log("��ŷ ��� �Ϸ�");
            }
            else
            {
                Debug.LogError("��ŷ ��� ����: " + task.Exception);
            }
        });
    }
}
