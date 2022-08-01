using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    
    [SerializeField] private int health = 3;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private float damageBounceTime = 0.5f;
    [SerializeField] private float xBounceFactor = -10f;
    [SerializeField] private float yBounceFactor = 15f;
    [SerializeField] private Text healthText;
    [SerializeField] private LevelManager levelManager;

    private AudioSource audio;
    private Rigidbody2D rb;
    private PlayerMovement movement;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
    }

    public bool Damage()
    {
        StartCoroutine("BounceBack");
        health--;
        audio.PlayOneShot(deathSound, 0.7f);
        rb.velocity = new Vector2(xBounceFactor * rb.velocity.x, yBounceFactor);
        Debug.Log("Damage taken! Health is: " + health);
        UpdateHealthText();
        return true;
    }

    public bool Heal()
    {
        if (health < 3)
        {
            health++;
            Debug.Log("Health restored! Health is: " + health);
            UpdateHealthText();
            return true;
        }
        return false;
    }
    
    public void Die()
    {
        /*
        levelManager.enableResetButton();
        Debug.Log("Player Dying!");
        Destroy(this.gameObject, deathSound.length);
        */
        levelManager.LoadLevel("Level01");
    }

    public void Update()
    {
        if (health <= 0)
            Die();
    }

    IEnumerator BounceBack()
    {
        movement.enabled = false;
        yield return new WaitForSeconds(damageBounceTime);
        movement.enabled = true;
    }

    private void UpdateHealthText()
    {
        healthText.text = "Health: " + health + "/3";
    }

}
