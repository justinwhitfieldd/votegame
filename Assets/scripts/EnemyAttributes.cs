using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttributesManager : MonoBehaviour
{
    public int health=50;
    public int attack = 10;
    public bool PlayerIsPunching = false;
    public int maxHealth = 50;
    public HealthBar healthbar;
    public EnemyController enemy;
    public Animator animator;
    public AttributesManager playerAttrib;
    public AudioClip punchSound;
    public AudioClip blockSound;
    public AudioSource enemyAudioSource;
    void Start()
    {
        health = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }
    public void TakeDamage(int amount)
    {
        if(!enemy.isBlocking && playerAttrib.PlayerIsPunching)
        {
            enemyAudioSource.clip = punchSound;
            enemyAudioSource.PlayOneShot(enemyAudioSource.clip);
            health -= amount;
            healthbar.SetHealth(health);
        } else {
            enemyAudioSource.clip = blockSound;
            enemyAudioSource.PlayOneShot(enemyAudioSource.clip);
        }
    }

    public void DealDamage(GameObject target)
    {
        if(PlayerIsPunching)//not sure what this is doing here
        {
            var atm = target.GetComponent<AttributesManager>();
            if(atm != null)
            {
                atm.TakeDamage(attack);
            }
        }
    }
}