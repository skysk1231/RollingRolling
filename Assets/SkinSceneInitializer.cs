using UnityEngine;
using UnityEngine.UI;

// 이 스크립트는 StartScene에만 존재하며, UI 요소들을 SkinManager에 안전하게 연결해주는 역할을 합니다.
public class SkinSceneInitializer : MonoBehaviour
{
    // 인스펙터에서 직접 연결할 수 있도록 Header와 SerializeField를 추가합니다.
    [Header("UI References to Link")]
    [Tooltip("Hierarchy에 있는 StartButton을 여기에 연결하세요.")]
    [SerializeField] private Button startButton;

    [Tooltip("Hierarchy에 있는 WatchAdButton을 여기에 연결하세요.")]
    [SerializeField] private Button watchAdButton;

    // 만약 스킨 변경 버튼(Next, Prev)의 리스너도 안전하게 설정하고 싶다면 아래 주석을 해제하고 연결하세요.
    // [SerializeField] private Button nextSkinButton;

    void Start()
    {
        // SkinManager 인스턴스가 존재하는지 확인
        if (SkinManager.Instance != null)
        {
            // 인스펙터에서 연결한 버튼 참조들을 SkinManager로 직접 전달합니다.
            SkinManager.Instance.LinkUIElements(startButton, watchAdButton);

            // (선택사항) 스킨 변경 버튼 리스너도 여기서 설정할 수 있습니다.
            // if (nextSkinButton != null)
            // {
            //     nextSkinButton.onClick.RemoveAllListeners();
            //     nextSkinButton.onClick.AddListener(SkinManager.Instance.NextSkin);
            // }
        }
        else
        {
            // 이 경고가 뜬다면 SkinManager 오브젝트가 DontDestroyOnLoad 되기 전에 이 스크립트가 실행되었다는 의미일 수 있습니다.
            // Edit > Project Settings > Script Execution Order에서 SkinManager를 먼저 실행하도록 설정할 수 있습니다.
            Debug.LogError("SkinManager.Instance가 존재하지 않습니다! SkinManager가 씬에 있는지, 혹은 실행 순서를 확인하세요.");
        }
    }
}
