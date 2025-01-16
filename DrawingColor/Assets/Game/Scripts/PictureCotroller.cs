using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DG.Tweening;
using Game.Scripts._04_Jump_To_Demo_1;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum PictureControllerAction
{
   OnPictureSetUpComplete,
   OnPictureFillComplete,
 
}
public struct PictureCotrollerActionEvent
{
   public PictureControllerAction pictureControllerAction;


   /// <summary>
   /// Initializes a new instance of the <see cref="MoreMountains.TopDownEngine.TopDownEngineEvent"/> struct.
   /// </summary>
   /// <param name="eventType">Event type.</param>
   public PictureCotrollerActionEvent(PictureControllerAction _pictureControllerAction)
   {
      pictureControllerAction = _pictureControllerAction;
   }

   static PictureCotrollerActionEvent e;
   public static void Trigger(PictureControllerAction pictureControllerAction)
   {
      e.pictureControllerAction = pictureControllerAction;
      MMEventManager.TriggerEvent(e);
   }
}     
public class PictureCotroller : MonoBehaviour, MMEventListener<PartClickActionEvent>
{

   #region Public Variables
   public PartClick       partClickPrefabs;
   public SpriteMask      spriteMask;
   public Vector2         size;
   public Transform       ScaleTransform;
   public Transform       partHolder;
   public Image           imageSpriteFullColor;
   public GameObject      Line;
   public Sprite          originSprite;
   public List<PartClick> parts = new List<PartClick>();
   public Sprite[]        partSprite;
   public DefaultAsset[]  partSpritePos;
   public Sprite[]        IDColorSprite;
   #endregion

   #region Private Variables
   [HideInInspector]public List<ColorData> colorDatas = new List<ColorData>();
   private List<int> DrawHistory = new List<int>();
   

   #endregion

   #region Unity Method

   private void OnEnable()
   {
      DrawHistory = new List<int>();
      SetUpParts();
      this.MMEventStartListening<PartClickActionEvent>();
   }

   private void OnDisable()
   {
      this.MMEventStopListening<PartClickActionEvent>();
   }

   public void OnMMEvent(PartClickActionEvent eventType)
   {
     DrawHistory.Add(eventType.id);
     if (DrawHistory.Count == partSprite.Length)
     {
        Line.gameObject.SetActive(false);
        PictureCotrollerActionEvent.Trigger(PictureControllerAction.OnPictureFillComplete);
     }
   }

   #endregion


   public void SetUpParts()
   {
      //Neu chua to thi ko phai lam j 
      
      // Neu do to thuc hien to
      
      colorDatas = new List<ColorData>();
      for (int i = 0; i < IDColorSprite.Length; i++)
      {
         int countFilled = 0;
         for (int j = 0; j < parts.Count; j++)
         {
            if (parts[j].GetIDColor() == (i + 1))
            {
               Debug.Log("count++");
               countFilled++;
            }
         }

         colorDatas.Add(new ColorData
         {
            colorSprite = IDColorSprite[i],
            colorID         = i + 1,
            totalPart       = countFilled,
            countPartFilled = 0
            
         });
      }
      PictureCotrollerActionEvent.Trigger(PictureControllerAction.OnPictureSetUpComplete);
   }
   [Button]
   public void ResetPicture()
   {
      DrawHistory = new List<int>();
      Line.gameObject.SetActive(true);
      foreach (var part in parts)
      {
  
         part.OnNotColor();
      }
   }

   [Button]
   public void GetRef()
   {
      ReadDefaultAsset();
      imageSpriteFullColor.sprite = originSprite;
      parts                       = this.GetComponentsInChildren<PartClick>().ToList();
      spriteMask                  = this.GetComponentInChildren<SpriteMask>();
      size                        = this.GetComponent<RectTransform>().sizeDelta;
      ScaleTransform.localScale   = Vector3.one * size.y / (originSprite.texture.width / originSprite.pixelsPerUnit);
   }

   public void SetMaskPos(Vector3 position)
   {
      spriteMask.transform.position = position;
   }

   public void TweenSetScaleMask(Action onFillComplete = null)
   {
      spriteMask.transform.localScale = Vector3.zero;
      spriteMask.transform.DOScale(1500f,0.5f).onComplete += () =>
      {
         spriteMask.transform.localScale = Vector3.zero;
         onFillComplete?.Invoke();
      };
   }

   void ReadDefaultAsset()
   {
      partHolder.transform.ClearChildrenImmediate();
      string path;
      for (int i = 0; i < partSprite.Length; i++)
      {
         PartClick newPart = Instantiate(partClickPrefabs, partHolder) as PartClick;
         newPart.name               = partSprite[i].name;
         newPart.whiteSprite.sprite = partSprite[i];
         path                       = AssetDatabase.GetAssetPath(partSpritePos[i]);
         Vector2   content       =   ExtensionClass.ReadVector2FormCORFile(path);
         newPart.transform.localScale = Vector3.one;

         float x = -originSprite.texture.width           / 2f + partSprite[i].texture.width  / 2f + content.x;
         float y = originSprite.texture.height           / 2f - partSprite[i].texture.height / 2f - content.y;
         newPart.transform.localPosition = new Vector3(x /100f,y                             /100f, 0);
         
         newPart.AutoGetRef();
         newPart.SetID(i);
         string[] numbers =   newPart.name.Split('_');
         newPart.SetIDColor(int.Parse(numbers[0]) );
      }

   }

 
}
