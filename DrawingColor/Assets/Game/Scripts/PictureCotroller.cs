using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;
public enum PictureControllerAction
{
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
public class PictureCotroller : MonoBehaviour
{

   #region Public Variables
   public List<PartClick> parts = new List<PartClick>();
   public SpriteMask      spriteMask;
   

   #endregion

   [Button]
   public void ResetPicture()
   {
      foreach (var part in parts)
      {
         part.OnNotColor();
      }
   }

   [Button]
   public void GetRef()
   {
      parts      = this.GetComponentsInChildren<PartClick>().ToList();
      spriteMask = this.GetComponentInChildren<SpriteMask>();
   }

   public void SetMaskPos(Vector3 position)
   {
      spriteMask.transform.position = position;
   }

   public void TweenSetScaleMask(Action onFillComplete = null)
   {
      spriteMask.transform.localScale = Vector3.zero;
      spriteMask.transform.DOScale(500f,0.5f).onComplete += () =>
      {
         spriteMask.transform.localScale = Vector3.zero;
         onFillComplete?.Invoke();
      };
   }

}
