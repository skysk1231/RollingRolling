using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RankListDisplay : MonoBehaviour
{
    public GameObject rankItemPrefab; // RankItem 프리팹
    public Transform contentTransform; // ScrollView → Content 오브젝트

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
            .OrderByChild("elapsedTime") // 빠른 순서대로 가져오되
            .LimitToFirst(5)
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("랭킹 불러오기 실패: " + task.Exception);
                    return;
                }

                DataSnapshot snapshot = task.Result;

                // 기존 리스트 제거
                foreach (Transform child in contentTransform)
                {
                    Destroy(child.gameObject);
                }

                // 1. 임시 리스트에 담기
                List<(string nickname, float elapsedTime)> rankingList = new List<(string, float)>();

                foreach (var childSnapshot in snapshot.Children)
                {
                    string nickname = childSnapshot.Child("nickname").Value?.ToString() ?? "??";
                    float elapsed = float.Parse(childSnapshot.Child("elapsedTime").Value.ToString());
                    rankingList.Add((nickname, elapsed));
                }

                // 2. 내림차순 정렬 (큰 값 -> 작은 값 반대는 OrderBy)
                rankingList = rankingList.OrderByDescending(data => data.elapsedTime).ToList();

                // 3. 순위대로 생성
                foreach (var rank in rankingList)
                {
                    GameObject newItem = Instantiate(rankItemPrefab, contentTransform);
                    newItem.GetComponent<RankItem>().SetRankData(rank.nickname, rank.elapsedTime);
                }

                Debug.Log("랭킹 불러오기 완료");
            });
    }
}
