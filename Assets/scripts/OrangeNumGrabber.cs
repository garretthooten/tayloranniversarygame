using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrangeNumGrabber : MonoBehaviour
{
    private Text myText;

    // Start is called before the first frame update
    void Start()
    {
        myText = GetComponent<Text>();
        myText.text = "Oranges Collected: " + PlayerPrefs.GetInt("score");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
