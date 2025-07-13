using UnityEngine;

public class GroundSkinSelectorUI : MonoBehaviour
{
    public void OnClickChangeGroundSkin()
    {
        GroundSkinManager.Instance.NextSkin();
    }
}
