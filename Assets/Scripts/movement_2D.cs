﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class movement_2D : MonoBehaviour
{
    public controller_2D controller;

    public float move_speed = 40f;
    public float run_speed_multiplier = 1.2f;

    float horizontal_move = 0f;

    bool jump = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        horizontal_move = Input.GetAxisRaw("Horizontal") * move_speed;
        if (Input.GetKey(KeyCode.LeftShift)){
            horizontal_move *= run_speed_multiplier;
        }



        if (Input.GetButtonDown("Jump")) {
            jump = true;
        }

        
    }

    void FixedUpdate(){

        controller.Move(horizontal_move * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
}