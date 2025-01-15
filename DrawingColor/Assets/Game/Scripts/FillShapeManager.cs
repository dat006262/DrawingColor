using System.Collections;
using System.Collections.Generic;
using IndieStudio.DrawingAndColoring.Logic;
using IndieStudio.DrawingAndColoring.Utility;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;

public class FillShapeManager : MMSingleton<FillShapeManager>
{
    #region Public Variables

    public Camera           drawCamera;
    public PictureCotroller currentPictureCotroller => GameManager.Instance.pictureControllerTest;

    #endregion

    #region Private Variables

    [ShowInInspector] private bool            isColoring = false;
    private                   ContactFilter2D ContactFilter;

    #endregion

    #region UnityMethod

    void Start()
    {
        ContactFilter = new ContactFilter2D
        {
            useLayerMask = true,
            layerMask    = LayerMask.NameToLayer("MiddleCamera"),
            useTriggers  = false,
        };
    }

    void Update()
    {
        if (Application.isMobilePlatform)
        {
            //Mobile Platform
            if (Input.touchCount != 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    if (!isColoring)
                    {
                        FillFeatureOnClickBegan();
                    }
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    FillFeatureOnClickReleased();
                }
            }
        }
        else
        {
            //Others
            if (Input.GetMouseButtonDown(0))
            {
                if (!isColoring)
                {
                    FillFeatureOnClickBegan();
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                FillFeatureOnClickReleased();
            }
        }
    }

    
    #endregion

    public void DrawAgain()
    {
        currentPictureCotroller.ResetPicture();
    }
    
    #region Private Methods

    private void FillFeatureOnClickBegan()
    {
        RaycastHit2D hit2d = Physics2D.Raycast(GetCurrentPlatformClickPosition(drawCamera), Vector2.zero);
        if (hit2d.collider != null)
        {
            PartClick partClick = hit2d.transform.gameObject.GetComponent<PartClick>();
            if (partClick != null)
            {
                currentPictureCotroller.SetMaskPos(hit2d.point);
                partClick.OnColoring();
                isColoring = true;
                currentPictureCotroller.TweenSetScaleMask(onFillComplete: () =>
                {
                    partClick.OnColored();
                    isColoring = false;
                });
                Debug.Log(partClick.transform.gameObject.name);
            }
        }
    }

    private Vector3 GetCurrentPlatformClickPosition(Camera camera)
    {
        Vector3 clickPosition = Vector3.zero;

        if (Application.isMobilePlatform)
        {
            //current platform is mobile
            if (Input.touchCount != 0)
            {
                Touch touch = Input.GetTouch(0);
                clickPosition = touch.position;
            }
        }
        else
        {
            //others
            clickPosition = Input.mousePosition;
        }

        clickPosition   = camera.ScreenToWorldPoint(clickPosition); //get click position in the world space
        clickPosition.z = 0;
        return clickPosition;
    }

    private void FillFeatureOnClickReleased()
    {
        Debug.Log("Upmouse");
    }

    #endregion
 
}