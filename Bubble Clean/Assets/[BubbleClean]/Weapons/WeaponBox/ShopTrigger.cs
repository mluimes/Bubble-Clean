using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public GameObject weaponSlot;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(WeaponDown());
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                enemy.StopMovement();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            weaponSlot.SetActive(true);
            weaponSlot.GetComponent<Animator>().SetTrigger("weaponUp");
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                enemy.ResumeMovement();
            }
        }
    }

    IEnumerator WeaponDown() {
        weaponSlot.GetComponent<Animator>().SetTrigger("weaponDown");
        yield return new WaitForSeconds(1);
        weaponSlot.SetActive(false);
    }
}
