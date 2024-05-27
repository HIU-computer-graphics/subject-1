using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chipscript : MonoBehaviour
{
    public DataManager dm;
    public GameObject objectToActivate;

    void Start()
    {
        dm = FindObjectOfType<DataManager>();
    }

    public void bluechip()
    {
        dm.Chip = 5;
    }

    public void blackchip()
    {
        dm.Chip = 1;
    }

    public void whitechip()
    {
        dm.Chip = 50;
    }

    public void greenchip()
    {
        dm.Chip = 10;
    }
}