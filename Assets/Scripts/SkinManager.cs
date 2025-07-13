using UnityEngine;
using UnityEngine.UI;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance;

    private int selectedSkinIndex = 0;
    private string skinPath = "Skins/skin_";
    public int totalSkinCount = 5;
    private const string UnlockKeyPrefix = "SkinUnlocked_";

    // 버튼 참조를 내부 private 변수로 관리합니다.
    private Button startButton;
    private Button watchAdButton;
    
    [ContextMenu("Debug: Reset All Skin Locks")]
    private void ResetAllSkinLocks()
    {
        if (Application.isEditor) // 에디터에서만 실행되도록 안전장치
        {
            Debug.LogWarning("DEBUG: Resetting all skin lock states...");
            // 0번 스킨은 기본 잠금 해제이므로 1번부터 순회합니다.
            for (int i = 1; i < totalSkinCount; i++)
            {
                string key = UnlockKeyPrefix + i;
                if (PlayerPrefs.HasKey(key))
                {
                    PlayerPrefs.DeleteKey(key);
                    Debug.Log($"Deleted PlayerPrefs key: {key}");
                }
            }
            PlayerPrefs.Save();
            Debug.LogWarning("All skin locks have been reset. Please re-apply skin state.");

            // 에디터에서 즉시 UI에 반영하려면 이 함수를 호출합니다.
            ApplySkin();
        }
        else
        {
            Debug.LogWarning("This function can only be run in the Unity Editor.");
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // SkinSceneInitializer가 호출하여 UI 요소들을 안전하게 연결해 줄 함수입니다.
    public void LinkUIElements(Button startBtn, Button watchAdBtn)
    {
        Debug.Log("UI 요소들이 SkinManager에 연결되었습니다.");
        startButton = startBtn;
        watchAdButton = watchAdBtn;

        // 광고 시청 버튼 클릭 이벤트 연결 (null 체크 추가)
        if (watchAdButton != null)
        {
            watchAdButton.onClick.RemoveAllListeners();
            watchAdButton.onClick.AddListener(OnWatchAdButtonClicked);
        }
        
        // UI가 연결된 직후, 현재 선택된 스킨 상태를 바로 적용합니다.
        selectedSkinIndex = PlayerPrefs.GetInt("SelectedSkin", 0);
        ApplySkin();
    }
    public void RefreshSkinUIState()
    {
        // 내부의 private 함수를 안전하게 호출합니다.
        ApplySkin();
    }
    
    public void ApplySkinTo(Player player)
    {
        int index = PlayerPrefs.GetInt("SelectedSkin", 0);
        Sprite skin = Resources.Load<Sprite>(skinPath + index);
        if (player != null && skin != null)
        {
            player.SetSprite(skin);
        }
    }

    public void NextSkin()
    {
        selectedSkinIndex++;
        if (selectedSkinIndex >= totalSkinCount)
            selectedSkinIndex = 0;

        PlayerPrefs.SetInt("SelectedSkin", selectedSkinIndex);
        ApplySkin();
    }

    // 이 함수는 이제 public일 필요가 없습니다. 내부에서만 사용됩니다.
    private void ApplySkin()
{
    // 스킨을 플레이어에 적용하는 부분
    Player playerInScene = FindFirstObjectByType<Player>();
    if(playerInScene != null)
    {
        ApplySkinTo(playerInScene);
    }

    // ========== 디버깅을 위한 로그 추가 ==========
    if (startButton == null)
    {
        Debug.LogError("SkinManager: 'startButton' 참조가 null입니다! SkinSceneInitializer 인스펙터 연결을 확인하세요.");
    }
    if (watchAdButton == null)
    {
        Debug.LogError("SkinManager: 'watchAdButton' 참조가 null입니다! SkinSceneInitializer 인스펙터 연결을 확인하세요.");
    }
    // ==========================================

    // 버튼 참조가 유효할 때만 UI 로직 실행
    if (startButton != null && watchAdButton != null)
    {
        Debug.Log($"ApplySkin UI Update: Skin {selectedSkinIndex} - Unlocked: {IsSkinUnlocked(selectedSkinIndex)}");
        if (IsSkinUnlocked(selectedSkinIndex))
        {
            startButton.gameObject.SetActive(true);
            watchAdButton.gameObject.SetActive(false);
        }
        else
        {
            startButton.gameObject.SetActive(false);
            watchAdButton.gameObject.SetActive(true);
        }
    }
}

    private bool IsSkinUnlocked(int index)
    {
        if (index == 0) return true;
        return PlayerPrefs.GetInt(UnlockKeyPrefix + index, 0) == 1;
    }

    public void OnWatchAdButtonClicked()
    {
        RewardAdController rewardAd = FindFirstObjectByType<RewardAdController>();
        if (rewardAd != null)
        {
            rewardAd.ShowAd(selectedSkinIndex);
        }
    }

    // RefreshUI 함수는 더 이상 필요 없으므로 삭제합니다.
}