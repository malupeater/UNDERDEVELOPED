using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightChanger : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            
        }    
    }
}
