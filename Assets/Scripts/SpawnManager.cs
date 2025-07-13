using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    public GameObject Enemy;             // 생성할 적 프리팹
    public LayerMask enemyLayer;         // 적이 속한 레이어
    public float spawnRadius = 0.6f;     // 충돌 검사 반경
    public int maxAttempts = 10;         // 생성 시도 최대 횟수
    public float spawnInterval = 0.3f;   // 생성 시도 간격 (초)

    void Start()
    {
        // 게임 시작 시 스폰 코루틴 시작
        StartCoroutine(SpawnLoop());
    }

    // 주기적으로 스폰을 시도하는 코루틴 함수
    IEnumerator SpawnLoop()
    {
        while (true)
        {
            TrySpawnEnemy();                     // 적 스폰 시도
            yield return new WaitForSeconds(spawnInterval);  // 일정 시간 대기
        }
    }

    // 적 생성 시도 함수
    void TrySpawnEnemy()
    {
        for (int i = 0; i < maxAttempts; i++)     // 최대 시도 횟수만큼 반복
        {
            // 무작위 위치 생성 (X는 -10.5~10.5, Y는 고정 6, Z는 0)
            Vector3 spawnPos = new Vector3(Random.Range(-10.5f, 10.5f), 6, 0);

            // 해당 위치 주변에 다른 오브젝트가 있으면 패스
            Collider2D hit = Physics2D.OverlapCircle(spawnPos, spawnRadius, enemyLayer);
            if (hit != null) continue;

            // 적 프리팹을 해당 위치에 생성
            GameObject newEnemy = Instantiate(Enemy, spawnPos, Quaternion.identity);

            // 무작위 크기 적용 (0.3 ~ 0.6 배)
            float randomSize = Random.Range(0.2f, 0.6f);
            newEnemy.transform.localScale = Vector3.one * randomSize;

            return; // 성공적으로 생성했으면 함수 종료
        }

        // 여기까지 왔다면 생성 실패
        Debug.LogWarning("TrySpawnEnemy: 생성 가능한 위치를 찾지 못함");
    }
}