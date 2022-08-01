using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{

    [SerializeField] int timeOn = 2;
    [SerializeField] int timeOff = 5;

    private Animator anim;
    private GameObject hurtBox;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        hurtBox = transform.GetChild(0).gameObject;
        
        StartCoroutine("FireLoop");
    }

    IEnumerator FireLoop()
    {
        while (true)
        {
            Debug.Log("Fire is OFF");
            anim.SetBool("isOn", false);
            hurtBox.SetActive(false);

            yield return new WaitForSeconds(timeOff);

            Debug.Log("Fire is ON");
            anim.SetBool("isOn", true);
            hurtBox.SetActive(true);

            yield return new WaitForSeconds(timeOn);

        }
    }
}
