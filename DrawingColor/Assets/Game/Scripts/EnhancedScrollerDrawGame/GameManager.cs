using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts._04_Jump_To_Demo_1;
using Game.Scripts.EnhancedScrollerDrawGame;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class GameManager : MMSingleton<GameManager>, MMEventListener<OpenPictureActionEvent>,MMEventListener<PictureCotrollerActionEvent>,MMEventListener<OnColorButtonEvent>
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
        this.MMEventStartListening<OnColorButtonEvent>();
    }

    public void OnDisable()
    {
        this.MMEventStopListening<OpenPictureActionEvent>();
        this.MMEventStopListening<PictureCotrollerActionEvent>();
        this.MMEventStopListening<OnColorButtonEvent>();
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
            SetUp();
        }
      
    }
    public void OnMMEvent(OnColorButtonEvent eventType)
    {
        if (eventType.onColorButtonAction == ColorButtonAction.Selected)
        {
            currentID = eventType.idColor;
            pictureControllerTest.ShowAllPart(currentID);
        }
      
    }

    [Button]
    private void SetUp()
    {
        UIGamePlay.gameObject.SetActive(true);
        collorIDController.SetUp(pictureControllerTest.colorDatas);
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
