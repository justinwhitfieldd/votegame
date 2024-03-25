using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyWeaponAttributes : MonoBehaviour
{
    public EnemyAttributesManager atm;
    public EnemyController enemy;
    public AttributesManager player;

    private void OnTriggerEnter(Collider other)
    {
        if(!enemy.contactMade)
        {
            if(other.CompareTag("Player") && enemy.isAttacking)
            {
                enemy.contactMade = true;
                player.TakeDamage(atm.attack);
            }
        }

    }
}
