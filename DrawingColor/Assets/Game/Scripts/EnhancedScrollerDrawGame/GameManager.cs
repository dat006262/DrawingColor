using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts._04_Jump_To_Demo_1;
using Game.Scripts.EnhancedScrollerDrawGame;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MMSingleton<GameManager>, MMEventListener<OpenPictureActionEvent>,MMEventListener<PictureCotrollerActionEvent>,MMEventListener<OnColorSelectedEvent>
{
    #region  Public Variables
    public  PictureCotroller   pictureControllerTest;
    public  Camera             HomeUICamera;
    public  Camera             GamePlayCamera;
    public  Canvas             UIGamePlay;
     public CollorIDController collorIDController;
     public int                currentID;
    #endregion
    public void OnEnable()
    {
        this.MMEventStartListening<OpenPictureActionEvent>();
        this.MMEventStartListening<PictureCotrollerActionEvent>();
        this.MMEventStartListening<OnColorSelectedEvent>();
    }

    public void OnDisable()
    {
        this.MMEventStopListening<OpenPictureActionEvent>();
        this.MMEventStopListening<PictureCotrollerActionEvent>();
        this.MMEventStopListening<OnColorSelectedEvent>();
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
    public void OnMMEvent(PictureCotrollerActionEvent eventType)
    {
        if (eventType.pictureControllerAction == PictureControllerAction.OnPictureSetUpComplete)
        {
            UIGamePlay.gameObject.SetActive(true);
            collorIDController.SetUp(pictureControllerTest.colorDatas);
        }
      
    }
    public void OnMMEvent(OnColorSelectedEvent eventType)
    {
        currentID = eventType.idColor;
        pictureControllerTest.ShowAllPart(currentID);
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
        UIGamePlay.gameObject.SetActive(true);
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
