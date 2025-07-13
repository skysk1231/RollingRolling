using TMPro;
using UnityEngine;

public class RankItem : MonoBehaviour
{
    public TMP_Text nicknameText;
    public TMP_Text timeText;

    public void SetRankData(string nickname, float time)
    {
        nicknameText.text = nickname;
        timeText.text = time.ToString("F2") + "s";
    }
}
