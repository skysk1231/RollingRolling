using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RankListDisplay : MonoBehaviour
{
    public GameObject rankItemPrefab; // RankItem ������
    public Transform contentTransform; // ScrollView �� Content ������Ʈ

    private void Start()
    {
        StartCoroutine(LoadRankings());
    }

    private System.Collections.IEnumerator LoadRankings()
    {
        while (!FirebaseInit.IsInitialized)
        {
            yield return null;
        }

        FirebaseInit.DB.Child("rankings")
            .OrderByChild("elapsedTime") // ���� ������� ��������
            .LimitToFirst(5)
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("��ŷ �ҷ����� ����: " + task.Exception);
                    return;
                }

                DataSnapshot snapshot = task.Result;

                // ���� ����Ʈ ����
                foreach (Transform child in contentTransform)
                {
                    Destroy(child.gameObject);
                }

                // 1. �ӽ� ����Ʈ�� ���
                List<(string nickname, float elapsedTime)> rankingList = new List<(string, float)>();

                foreach (var childSnapshot in snapshot.Children)
                {
                    string nickname = childSnapshot.Child("nickname").Value?.ToString() ?? "??";
                    float elapsed = float.Parse(childSnapshot.Child("elapsedTime").Value.ToString());
                    rankingList.Add((nickname, elapsed));
                }

                // 2. �������� ���� (ū �� -> ���� �� �ݴ�� OrderBy)
                rankingList = rankingList.OrderByDescending(data => data.elapsedTime).ToList();

                // 3. ������� ����
                foreach (var rank in rankingList)
                {
                    GameObject newItem = Instantiate(rankItemPrefab, contentTransform);
                    newItem.GetComponent<RankItem>().SetRankData(rank.nickname, rank.elapsedTime);
                }

                Debug.Log("��ŷ �ҷ����� �Ϸ�");
            });
    }
}
