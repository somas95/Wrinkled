using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VestigioManager : MonoBehaviour
{

    private AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.instance;       
        audioManager.Play("sal");
        audioManager.Play("background");
        audioManager.Play("viento");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
