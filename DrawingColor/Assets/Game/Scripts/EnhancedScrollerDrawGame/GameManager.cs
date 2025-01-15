using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.EnhancedScrollerDrawGame;
using MoreMountains.Tools;
using UnityEngine;

public class GameManager : MonoBehaviour, MMEventListener<OpenPictureActionEvent>
{
    public void OnEnable()
    {
        this.MMEventStartListening<OpenPictureActionEvent>();
    }

    public void OnDisable()
    {
        this.MMEventStopListening<OpenPictureActionEvent>();
    }

    public void OnMMEvent(OpenPictureActionEvent eventType)
    {
       Debug.Log(eventType.pictureName + " Opened");
    }
}
