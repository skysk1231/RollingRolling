using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject LeftJoyStick, RightJoyStick;  // ����, ������ ���̽�ƽ ��ü
    public Button restartButton;  // ����� ��ư
    private Camera cam;  // ���� ī�޶�

    public TextMeshProUGUI timerText;  // Ÿ�̸� �ؽ�Ʈ UI
    public float elapsedTime = 0f;  // ��� �ð�
    public bool isGameOver = false;  // ���� ���� ����
    private RectTransform restartButtonRect;  // ����� ��ư�� RectTransform
    private int restartCount;  // ����� Ƚ��

    void Start()
    {
        // PlayerPrefs���� ����� Ƚ���� �ҷ���
        restartCount = PlayerPrefs.GetInt("RestartCount", 0);
    }

    void Awake()
    {
        // ī�޶� ��ü �ʱ�ȭ
        cam = Camera.main;
    }

    void Update()
    {
        float screenHalfWidth = Screen.width / 2;  // ȭ���� ���� �ʺ�

        // ��ġ �Է��� ���� ��� ó��
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                // ����� ��ư�� ��ġ�� ���� ��� ����
                if (IsTouchOverRestartButton(touch.position)) continue;
                // ���̽�ƽ�� ��ġ�� ���� ��� ����
                if (IsTouchOverJoystick(touch.position)) continue;

                // ȭ�� ���� �����̸� ���� ���̽�ƽ �̵�
                if (touch.position.x < screenHalfWidth)
                {
                    LeftJoyStick.transform.position = GetTouchWorldPosition(touch, -12f);
                }
                else
                {
                    // ȭ�� ������ �����̸� ������ ���̽�ƽ �̵�
                    RightJoyStick.transform.position = GetTouchWorldPosition(touch, 12f);
                }
            }
        }

        // ������ ���� ���� ��� Ÿ�̸Ӹ� ����
        if (!isGameOver)
        {
            elapsedTime += Time.deltaTime;  // ��� �ð� ���
            timerText.text = $"Time: {elapsedTime:F2}";  // Ÿ�̸� �ؽ�Ʈ ������Ʈ
        }
        else
        {
            // ���� ������ �Ǹ� PlayerPrefs�� ��� �ð��� �����ϰ�,
            // ����� Ƚ���� ������Ű�� ���� ���� ������ �̵�
            PlayerPrefs.SetFloat("elapsedTime", elapsedTime);
            PlayerPrefs.Save();  // PlayerPrefs ����

            restartCount++;
            PlayerPrefs.SetInt("RestartCount", restartCount);

            // "GameOver" �� �ε� �� ���� ���� ǥ�� ��û
            SceneManager.LoadScene("GameOver");

            // ���� �ε�Ǿ��� �� �ٷ� ���� ���� �� �ֵ��� ȣ��
            AdManager.Instance.ShowInterstitialAd();
        }
    }

    // ��ġ ��ġ�� ���� ��ǥ�� ��ȯ�ϰ� X�� �������� �߰�
    Vector3 GetTouchWorldPosition(Touch touch, float xOffset)
    {
        var touchPos = cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cam.nearClipPlane));
        touchPos.z = 0;  // Z���� 0���� �����Ͽ� 2D �������� ����
        touchPos.x = xOffset;  // X�� ������ ����
        return touchPos;
    }

    // ����� ��ư ���� ��ġ�� �־����� Ȯ���ϴ� �Լ�
    bool IsTouchOverRestartButton(Vector2 touchPosition)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(restartButtonRect, touchPosition, cam);
    }

    // ���̽�ƽ ���� ��ġ�� �־����� Ȯ���ϴ� �Լ�
    bool IsTouchOverJoystick(Vector2 touchPosition)
    {
        Vector3 worldTouch = cam.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, cam.nearClipPlane));
        worldTouch.z = 0;  // Z���� 0���� �����Ͽ� 2D �������� ����

        // ���� ���̽�ƽ�� ������ ���̽�ƽ�� ��� ������ ������
        Bounds leftBounds = LeftJoyStick.GetComponent<SpriteRenderer>().bounds;
        Bounds rightBounds = RightJoyStick.GetComponent<SpriteRenderer>().bounds;

        // ��ġ�� ���̽�ƽ ���� �ȿ� ���� ��� true ��ȯ
        return leftBounds.Contains(worldTouch) || rightBounds.Contains(worldTouch);
    }
}
