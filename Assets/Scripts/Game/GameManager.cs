﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Momo;
using System.IO;

public class GameManager : MonoBehaviour
{
    SimpleBlit blitScript;
    bool restart = false;
    float timer = 0.0f;
    float duration = 0.2f;


    private void Awake()
    {
        Screen.SetResolution(1600, 900, false);
    }
    void Start()
    {
        Cursor.visible = false;
        timer = duration;
        EventManager.I.Events.TriggerEvent("pause", null);
        EventManager.I.Events.StartListening("game_restart", RestartGame);
        EventManager.I.Events.StartListening("game_quit", QuitGame);
        EventManager.I.Events.StartListening("player_died", PlayerDied);

        blitScript = Camera.main.GetComponent<SimpleBlit>();
        blitScript.TransitionMaterial.SetFloat("_Cutoff", timer / duration);

    }

    

    private void QuitGame(object obj)
    {
        Application.Quit();
    }

    private void RestartGame(object obj)
    {
        restart = true;
    }

    private void PlayerDied(object obj)
    {
        restart = true;
    }

    private void Update()
    {
        if (restart == false)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer += Time.deltaTime;
        }

        timer = Mathf.Clamp(timer, 0.0f, duration);
        blitScript.TransitionMaterial.SetFloat("_Cutoff", timer / duration);


        if (restart && timer >= duration)
        {
            SceneManager.LoadScene("JumpMan");
        }
    }
}


