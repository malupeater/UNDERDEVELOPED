using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public TextMeshProUGUI hp;
    public int health;

    void Start()
    {
        SetPlayerData();
    }

    void Update()
    {
        UpdateHPBar();
    }
    public void SetPlayerData()
    {
        PlayerData playerData = SaveSystemManager.LoadPlayer();
        Vector3 position;
        
        health = playerData.health;
        position.x = playerData.position[0];
        position.y = playerData.position[1];
        position.z = playerData.position[2];
        transform.position = position;
    }

    public void UpdateHPBar()
    {
        hp.text = $"{health}";
    }
}
