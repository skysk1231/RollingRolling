using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject LeftJoyStick, RightJoyStick;  // 왼쪽, 오른쪽 조이스틱 객체
    public Button restartButton;  // 재시작 버튼
    private Camera cam;  // 메인 카메라

    public TextMeshProUGUI timerText;  // 타이머 텍스트 UI
    public float elapsedTime = 0f;  // 경과 시간
    public bool isGameOver = false;  // 게임 오버 여부
    private RectTransform restartButtonRect;  // 재시작 버튼의 RectTransform
    private int restartCount;  // 재시작 횟수

    void Start()
    {
        // PlayerPrefs에서 재시작 횟수를 불러옴
        restartCount = PlayerPrefs.GetInt("RestartCount", 0);
    }

    void Awake()
    {
        // 카메라 객체 초기화
        cam = Camera.main;
    }

    void Update()
    {
        float screenHalfWidth = Screen.width / 2;  // 화면의 절반 너비

        // 터치 입력이 있을 경우 처리
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                // 재시작 버튼에 터치가 있을 경우 무시
                if (IsTouchOverRestartButton(touch.position)) continue;
                // 조이스틱에 터치가 있을 경우 무시
                if (IsTouchOverJoystick(touch.position)) continue;

                // 화면 왼쪽 절반이면 왼쪽 조이스틱 이동
                if (touch.position.x < screenHalfWidth)
                {
                    LeftJoyStick.transform.position = GetTouchWorldPosition(touch, -12f);
                }
                else
                {
                    // 화면 오른쪽 절반이면 오른쪽 조이스틱 이동
                    RightJoyStick.transform.position = GetTouchWorldPosition(touch, 12f);
                }
            }
        }

        // 게임이 진행 중인 경우 타이머를 갱신
        if (!isGameOver)
        {
            elapsedTime += Time.deltaTime;  // 경과 시간 계산
            timerText.text = $"Time: {elapsedTime:F2}";  // 타이머 텍스트 업데이트
        }
        else
        {
            // 게임 오버가 되면 PlayerPrefs에 경과 시간을 저장하고,
            // 재시작 횟수를 증가시키며 게임 오버 씬으로 이동
            PlayerPrefs.SetFloat("elapsedTime", elapsedTime);
            PlayerPrefs.Save();  // PlayerPrefs 저장

            restartCount++;
            PlayerPrefs.SetInt("RestartCount", restartCount);

            // "GameOver" 씬 로드 후 전면 광고 표시 요청
            SceneManager.LoadScene("GameOver");

            // 광고가 로드되었을 때 바로 광고가 나올 수 있도록 호출
            AdManager.Instance.ShowInterstitialAd();
        }
    }

    // 터치 위치를 월드 좌표로 변환하고 X축 오프셋을 추가
    Vector3 GetTouchWorldPosition(Touch touch, float xOffset)
    {
        var touchPos = cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cam.nearClipPlane));
        touchPos.z = 0;  // Z값을 0으로 설정하여 2D 공간으로 제한
        touchPos.x = xOffset;  // X축 오프셋 적용
        return touchPos;
    }

    // 재시작 버튼 위에 터치가 있었는지 확인하는 함수
    bool IsTouchOverRestartButton(Vector2 touchPosition)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(restartButtonRect, touchPosition, cam);
    }

    // 조이스틱 위에 터치가 있었는지 확인하는 함수
    bool IsTouchOverJoystick(Vector2 touchPosition)
    {
        Vector3 worldTouch = cam.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, cam.nearClipPlane));
        worldTouch.z = 0;  // Z값을 0으로 설정하여 2D 공간으로 제한

        // 왼쪽 조이스틱과 오른쪽 조이스틱의 경계 영역을 가져옴
        Bounds leftBounds = LeftJoyStick.GetComponent<SpriteRenderer>().bounds;
        Bounds rightBounds = RightJoyStick.GetComponent<SpriteRenderer>().bounds;

        // 터치가 조이스틱 영역 안에 있을 경우 true 반환
        return leftBounds.Contains(worldTouch) || rightBounds.Contains(worldTouch);
    }
}
