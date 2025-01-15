using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.EnhancedScrollerDrawGame;
using MoreMountains.Tools;
using UnityEngine;

public class GameManager : MMSingleton<GameManager>, MMEventListener<OpenPictureActionEvent>
{
    #region  Public Variables
    public PictureCotroller pictureControllerTest;
    public Camera           HomeUICamera;
    public Camera           GamePlayCamera;
    #endregion
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
       
       // Phai dung Image de Scroll Hoat dong dung
       pictureControllerTest.gameObject.SetActive(true);
       pictureControllerTest.transform.localPosition = Vector3.zero;
       pictureControllerTest.transform.localScale    = Vector3.one *Screen.width / pictureControllerTest.size.y;
        
       //
       OffUI();
       //
        OnGamePlay();
    }
    private void OnUI()
    {
        HomeUICamera.gameObject.SetActive(true);

    }

    private void OffUI()
    {
        HomeUICamera.gameObject.SetActive(false);

    }

    private void OnGamePlay()
    {
        GamePlayCamera.gameObject.SetActive(true);
        FillShapeManager.Instance.gameObject.SetActive(true);
        CammeraDrag.Instance.enabled = true;
        CammeraZoom.Instance.enabled = true;
    }
    private void OffGamePlay()
    {
        GamePlayCamera.gameObject.SetActive(false);
        FillShapeManager.Instance.gameObject.SetActive(false);
        CammeraDrag.Instance.enabled = false;
        CammeraZoom.Instance.enabled = false;
    }
}
