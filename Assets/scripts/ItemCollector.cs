using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] private AudioClip collect;
    [SerializeField] private Text orangeText;
    [SerializeField] private int orangesToHeal = 5;
    private PlayerHealth health;
    private AudioSource audio;
    private int oranges = 0;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        health = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            Debug.Log("Heal Button Pressed!");
            if (oranges >= orangesToHeal && health.Heal())
            {
                oranges -= orangesToHeal;
                orangeText.text = "Oranges: " + oranges;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Orange"))
        {
            Destroy(collision.gameObject);
            audio.PlayOneShot(collect, 0.7f);
            oranges++;
            orangeText.text = "Oranges: " + oranges;
            Debug.Log("Oranges: " + oranges);
        }
    }

    void OnDisable()
    {
        PlayerPrefs.SetInt("score", oranges);

    }

}
