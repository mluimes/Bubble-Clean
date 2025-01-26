using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointsManager : MonoBehaviour
{
    public int CurrentPoints { get; private set; } = 500;
    [SerializeField] TextMeshProUGUI pointsTxt;

    public void AddPoints(int amount)
    {
        CurrentPoints += amount;
        UpdateUI();
        Debug.Log($"Points added: {amount}. Total: {CurrentPoints}");
    }

    public void SpendPoints(int amount)
    {
        if (CurrentPoints >= amount)
        {
            CurrentPoints -= amount;
            UpdateUI();
            Debug.Log($"Points spent: {amount}. Remaining: {CurrentPoints}");
        }
        else
        {
            Debug.Log("Not enough Points!");
        }
    }

    private void UpdateUI() {
        pointsTxt.text = CurrentPoints.ToString();
    }
}
