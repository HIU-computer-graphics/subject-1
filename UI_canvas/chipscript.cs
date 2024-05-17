using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class chipscript : MonoBehaviour
{
    public GameObject objectToActivate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void bluechip()
    {
        PlayerPrefs.SetInt("chip", 5);
    }
    public void blackchip()
    {
        PlayerPrefs.SetInt("chip", 1);
    }
    public void whitechip()
    {
        PlayerPrefs.SetInt("chip", 50);
    }
    public void greenchip()
    {
        PlayerPrefs.SetInt("chip", 10);
    }
}
