using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.instance;       
        audioManager.Play("background");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
