using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathUI : MonoBehaviour
{
    public Button MenuButton;
    public Button RespawnButton;

    private void Start()
    {
        RespawnButton.onClick.AddListener(Respawn);
    }

    private static void Respawn()
    {
        GameManager.Instance.RespawnPlayer();
    }
}
