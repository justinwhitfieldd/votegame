using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributesManager : MonoBehaviour
{
    public int health = 50;
    public int attack = 10;
    public bool PlayerIsPunching = false;
    public int maxHealth = 50;
    public HealthBar healthbar;
    public EnemyAttributesManager enemyAttrib;
    public PlayerController player;
    public AudioClip punchSound;
    public AudioClip blockSound;
    public AudioSource playerAudioSource;
    void Start()
    {
        health = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }
    public void TakeDamage(int amount)
    {
        if(player.isBlocking)
        {
            playerAudioSource.clip = blockSound;
            playerAudioSource.PlayOneShot(playerAudioSource.clip);
        }else{
            playerAudioSource.clip = punchSound;
            playerAudioSource.PlayOneShot(playerAudioSource.clip);
            health -= amount;
            healthbar.SetHealth(health);
        }

    }
    public void DealDamage(GameObject target)
    {
        var atm = target.GetComponent<EnemyAttributesManager>();
        if(atm != null)
        {
            atm.TakeDamage(attack);
        }
    }
}