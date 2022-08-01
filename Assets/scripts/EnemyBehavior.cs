using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    [SerializeField] private int health = 3;
    [SerializeField] private float moveFactor = -10.0f;
    [SerializeField] private BoxCollider2D bounceBox;
    [SerializeField] private bool canMove = true;

    private bool moving = false;

    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health == 0)
            Die();

        //  Check if enemy is on screen, for bird enemy
        if (GetComponent<Renderer>().isVisible)
        {
            Debug.Log("I am on screen!");
            if (!moving && canMove)
            {
                rb.velocity = new Vector2(moveFactor, 0);
                moving = true;
            }
        }
        
    }

    public void DamageDealt()
    {
        health--;
        Debug.Log("Damage dealt! Current health: " + health);
    }
    
    private void Die()
    {
        Debug.Log("Health is 0! Dying!");
        Destroy(this.gameObject);
    }
}
