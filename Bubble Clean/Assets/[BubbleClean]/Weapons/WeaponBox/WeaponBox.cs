using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponBox : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private Weapon weaponPrefab;
    public int weaponCost = 100;

    [Header("States")]
    public bool isEquipped = false;
    public bool isPurchased = false;

    [Header("References")]
    private PointsManager pointsManager;
    private WeaponManager weaponManager;

    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject textObject;

    private void Awake()
    {
        textObject.SetActive(false);
        weaponManager = FindAnyObjectByType<WeaponManager>();
        weaponManager.RegisterWeaponBox(this);

        pointsManager = FindObjectOfType<PointsManager>();
        if (pointsManager == null)
        {
            Debug.LogWarning("PointsManager not found");
        }
    }

    public void UpdateBoxState(Weapon currentlyEquippedWeapon)
    {
        if (weaponPrefab == null)
        {
            Debug.LogWarning($"WeaponBox {gameObject.name} does not have a weaponPrefab assigned.");
            return;
        }

        if (currentlyEquippedWeapon == null)
        {
            isEquipped = false;
            Debug.Log($"No weapon currently equipped. Box {weaponPrefab.name} set to unequipped.");
        }
        else
        {
            isEquipped = currentlyEquippedWeapon.name == weaponPrefab.name;
        }

        UpdateWeaponBox();
    }

    public void UpdateWeaponBox()
    {
        if (isEquipped)
        {
            StartCoroutine(ShowText($"Arma {weaponPrefab.name} equipada"));
            Debug.Log($"Weapon {weaponPrefab.name} is now equipped.");
        }
        else if (!isPurchased)
        {
            textObject.SetActive(true);
            text.text = $"Comprar {weaponPrefab.name} por {weaponCost} puntos";
            Debug.Log($"Weapon {weaponPrefab.name} is available for purchase.");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isPurchased)
            {
                textObject.SetActive(true);
                text.text = $"Pulsa E para comprar {weaponPrefab.name} por {weaponCost} puntos";
            }
            else if (isPurchased && !isEquipped)
            {
                textObject.SetActive(true);
                text.text = $"Pulsa E para equipar {weaponPrefab.name}";
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("isPurchased:" + isPurchased + ", isEquipped: " + isEquipped);
                if (!isPurchased)
                {
                    BuyWeapon();
                }
                else if (isPurchased && !isEquipped)
                {
                    EquipWeapon();
                }
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player"))
        {
            textObject.SetActive(false);
        }
    }

    private void BuyWeapon()
    {
        Debug.Log("BuyWeapon() called");
        if (pointsManager.CurrentPoints >= weaponCost)
        {
            pointsManager.SpendPoints(weaponCost);
            isPurchased = true;
            UpdateWeaponBox();
            Debug.Log($"Weapon {weaponPrefab.name} purchased!");
        }
        else
        {
            Debug.Log("Not enough money...");
        }
    }

    private void EquipWeapon()
    {
        WeaponManager.Instance.EquipWeapon(weaponPrefab);
        isEquipped = true;
        UpdateWeaponBox();
    }

    IEnumerator ShowText(string message)
    {
        textObject.SetActive(true);
        text.text = message;
        yield return new WaitForSeconds(4);
        textObject.SetActive(false);
    }
}

