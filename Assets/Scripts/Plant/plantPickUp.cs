using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plantPickUp : Interactable
{
    public Plant plant;

    public override void Interact()
    {
        base.Interact();

        PickUp();
    }

    private void PickUp()
    {
        //Debug.Log("Picking up " + plant.name + " !");
        GameObject player = GameObject.Find("Player");
        switch (plant.name)
        {
            case ("Plant1"):
            {
                player.GetComponent<PlantCounter>().addPlant1();
                break;
            }
            case ("Plant2"):
            {
                    player.GetComponent<PlantCounter>().addPlant2();
                    break;
            }
            case ("Plant3"):
            {
                    player.GetComponent<PlantCounter>().addPlant3();
                    break;
            }
        }
        player.GetComponent<PlantCounter>().showCount();
        Destroy(transform.gameObject);
    }
}
