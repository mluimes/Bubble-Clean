using TMPro;
using UnityEngine;

public class RoundTextUpdater : MonoBehaviour
{
    public TextMeshProUGUI roundText;

    private void OnEnable()
    {
        RoundManager.Instance.OnRoundChanged += UpdateRoundText;
    }

    private void OnDisable()
    {
        RoundManager.Instance.OnRoundChanged -= UpdateRoundText;
    }

    private void UpdateRoundText(int newRound)
    {
        roundText.text = $"Ronda: {newRound}";
    }
}