using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GroundSkinManager : MonoBehaviour
{
    public static GroundSkinManager Instance;

    private int selectedGroundSkinIndex = 0;
    private string groundSkinPath = "Grounds/ground_";
    public int totalGroundSkinCount = 5;  // ground 스킨 총 개수

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환해도 살아있음
        }
        else
        {
            Destroy(gameObject); // 중복 방지
        }
    }

    public void ApplySkinTo(ground2 ground)
    {
        int index = PlayerPrefs.GetInt("SelectedGroundSkin", 0);
        Sprite groundSkin = Resources.Load<Sprite>(groundSkinPath + index);
        ground.SetSprite(groundSkin);
    }

    private void Start()
    {
        selectedGroundSkinIndex = PlayerPrefs.GetInt("SelectedGroundSkin", 0);
        ApplySkin();
    }

    public void NextSkin()
    {
        selectedGroundSkinIndex++;
        if (selectedGroundSkinIndex >= totalGroundSkinCount)
            selectedGroundSkinIndex = 0;

        PlayerPrefs.SetInt("SelectedGroundSkin", selectedGroundSkinIndex);
        ApplySkin();
    }

    public void SelectSkin(int index)
    {
        selectedGroundSkinIndex = index;
        PlayerPrefs.SetInt("SelectedGroundSkin", index);
        ApplySkin();
    }

    private void ApplySkin()
    {
        Sprite newGroundSkin = Resources.Load<Sprite>(groundSkinPath + selectedGroundSkinIndex);
        if (newGroundSkin != null)
            ground2.Instance.SetSprite(newGroundSkin);
        else
            Debug.LogWarning("그라운드 스킨이 Resources에서 로드되지 않았습니다.");
    }
}


