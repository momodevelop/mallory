﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    MainGame mainGame;
    public event Action onTriggerEnterEvent;
    
    // Start is called before the first frame update
    void Awake()
    {
        this.transform.position = new Vector3(Camera.main.transform.position.x, this.transform.position.y, this.transform.position.z);
        this.transform.localScale = new Vector3(Utils.GetCameraWidthPixels(), Utils.GetPixelPerUnit());
        EventManager.I.Events.StartListening("pause", Pause);
        EventManager.I.Events.StartListening("unpause", Unpause);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onTriggerEnterEvent();
    }

    public void SetY(float y)
    {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    public void Pause(object o)
    {
        this.gameObject.SetActive(false);
    }

    public void Unpause(object o)
    {
        this.gameObject.SetActive(true);
    }
}
