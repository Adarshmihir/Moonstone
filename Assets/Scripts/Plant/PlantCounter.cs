using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCounter : MonoBehaviour
{

    int nbPlant1;
    int nbPlant2;
    int nbPlant3;


    // Start is called before the first frame update
    void Start()
    {
        nbPlant1 = 0;
        nbPlant2 = 0;
        nbPlant3 = 0;
    }

    public void addPlant1()
    {
        nbPlant1++;
    }

    public void addPlant2()
    {
        nbPlant2++;
    }

    public void addPlant3()
    {
        nbPlant3++;
    }

    public void showCount()
    {
        //Debug.Log("nbPlant1 = " + nbPlant1 + " nbPlant2 = " + nbPlant2 + " nbPlant3 = " + nbPlant3);
    }
}
