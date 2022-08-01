using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGoal : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private LevelManager levelmanager;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D()
    {
        Debug.Log("Object Entered Goal!");
        anim.Play("Trophy", -1, 0f);
        levelmanager.LoadLevel("EndScreen");
    }
}
