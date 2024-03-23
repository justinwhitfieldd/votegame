using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttributes : MonoBehaviour
{
    public AttributesManager atm;
    public PlayerController player;
    private void OnTriggerEnter(Collider other)
    {
        if(!player.contactMade)
        {
            if(other.CompareTag("Enemy") && atm.PlayerIsPunching)
            {
                player.contactMade = true;
                Debug.Log("enemy has been hit");
                other.GetComponent<EnemyAttributesManager>().TakeDamage(atm.attack);
            }
        }
    }
}