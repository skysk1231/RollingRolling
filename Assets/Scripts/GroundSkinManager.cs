using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GroundSkinManager : MonoBehaviour
{
    public static GroundSkinManager Instance;

    private int selectedGroundSkinIndex = 0;
    private string groundSkinPath = "Grounds/ground_";
    public int totalGroundSkinCount = 5;  // ground ��Ų �� ����

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ�ص� �������
        }
        else
        {
            Destroy(gameObject); // �ߺ� ����
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
            Debug.LogWarning("�׶��� ��Ų�� Resources���� �ε���� �ʾҽ��ϴ�.");
    }
}


