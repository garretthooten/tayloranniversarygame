using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private Button resetButton;

    // Start is called before the first frame update
    void Start()
    {
        resetButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void enableResetButton()
    {
        resetButton.gameObject.SetActive(true);
    }

    public void reloadLevel(string name)
    {
        Debug.Log("Reload Button Clicked");
        //SceneManager.LoadScene(name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadLevel(string name)
    {
        Debug.Log("Loading Level " + name);
        SceneManager.LoadScene(name);
    }
}
