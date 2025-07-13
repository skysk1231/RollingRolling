using UnityEngine;

public class SkinSelectorUI : MonoBehaviour
{
    public void OnClickChangeSkin()
    {
        SkinManager.Instance.NextSkin();
    }
}