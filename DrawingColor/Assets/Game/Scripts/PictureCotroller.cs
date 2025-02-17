using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DG.Tweening;
using Game.Scripts._04_Jump_To_Demo_1;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using TMPro;
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
public class PictureCotroller : MonoBehaviour, MMEventListener<PartClickActionEvent>,MMEventListener<CameraZoomSlideEvent>
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
   public List<TextMeshPro>          lstText = new List<TextMeshPro>();
   public Sprite[]        partSprite;
   #if UNITY_EDITOR
   public List<string>   corFilePaths = new List<string>();
   #endif
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
      this.MMEventStartListening<CameraZoomSlideEvent>();
   }

   private void OnDisable()
   {
      this.MMEventStopListening<PartClickActionEvent>();
      this.MMEventStopListening<CameraZoomSlideEvent>();
   }

   public void OnMMEvent(PartClickActionEvent eventType)
   {
      if (eventType.PartClickAction == PartClickAction.OnPartFillComplete)
      {
         DrawHistory.Add(eventType.id);
         if (DrawHistory.Count == partSprite.Length)
         {
            Line.gameObject.SetActive(false);
            PictureCotrollerActionEvent.Trigger(PictureControllerAction.OnPictureFillComplete);
         }
      }
  
   }
   public void OnMMEvent(CameraZoomSlideEvent eventType)
   {
      for (int i = 0; i < lstText.Count; i++)
      {
       //  lstText[i].gameObject.SetActive(lstText[i].fontSize > ((1-eventType.value) * 10));
       lstText[i].gameObject.SetActive(false);
      }
  
   }
   #endregion

   public void ShowAllPart(int idColor)
   {

      
      for (int i = 0; i < parts.Count; i++)
      {
         parts[i].OffHighLight();
         if (!parts[i].isColored)
         {
            if (parts[i].idColor == idColor)
            {
               parts[i].OnHighLight();
               
            }
         }
      }
   }

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

    
      PictureCotrollerActionEvent.Trigger(PictureControllerAction.OnPictureSetUpComplete);
   }

   [Button]
   public void LoadListSprite(string pathFolder)
   {
#if UNITY_EDITOR
      string[] guids = AssetDatabase.FindAssets("t:sprite", new[] { pathFolder });
      int      i     = 0;
      partSprite    = new Sprite[guids.Length];
      foreach (var guid in guids)
      {
         
         string path   = AssetDatabase.GUIDToAssetPath(guid);
         Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
      
         partSprite[i] = sprite;
         Debug.Log("Loaded sprite: " + sprite.name);
         i++;
      }
      partSprite = partSprite.OrderBy(sprite => sprite.name).ToArray();
#endif
    
   }

   [Button]
   public void SortPart()
   {
      for (int i = 0; i < parts.Count; i++)
      {
         parts[i].text.transform.name ="Text"+ parts[i].transform.name;
      }
      parts.Sort((sprite1,   sprite2) => string.Compare(sprite1.transform.name, sprite2.transform.name));
      lstText.Sort((sprite1, sprite2) => string.Compare(sprite1.transform.name, sprite2.transform.name));
   }

   [Button]
   public void LoadListCor(string pathFolder)
   {
#if UNITY_EDITOR
      if (Directory.Exists(pathFolder))
      {
         Debug.Log("Loaded .cor file");
      }

    
      string[] corFiles = Directory.GetFiles(pathFolder, "*.cor", SearchOption.AllDirectories);
      
      var sortedFiles = corFiles
         .Select(file => new FileInfo(file))
         .OrderBy(fileInfo => fileInfo.FullName)
         .Select(fileInfo => fileInfo.FullName)
         .ToList();
      foreach (var file in sortedFiles)
      {
         
         corFilePaths.Add(file);
         Debug.Log("Loaded .cor file: " + file);
      }
#endif
   
   }
   [Button]
   public void GetRef()
   {
      //LoadListSprite();
      ReadDefaultAsset();
      imageSpriteFullColor.sprite = originSprite;
      parts                       = this.GetComponentsInChildren<PartClick>().ToList();
      lstText                     = this.GetComponentsInChildren<TextMeshPro>().ToList();
      SortText();
      spriteMask                  = this.GetComponentInChildren<SpriteMask>();
      size                        = this.GetComponent<RectTransform>().sizeDelta;
      ScaleTransform.localScale   = Vector3.one * size.y / (originSprite.texture.width / originSprite.pixelsPerUnit);
      SortPart();
   }
   [Button]
   public void SortText()
   {
      lstText.Sort(CompartTextSize);
   }
   public void SetMaskPos(Vector3 position)
   {
      spriteMask.transform.position = position;
   }

   public void TweenSetScaleMask(float _sizeMask,Action onFillComplete = null)
   {
      spriteMask.transform.localScale = Vector3.zero;
      float sizeMask = Mathf.Max(500f, _sizeMask * Screen.height);
      float time = Mathf.Lerp(0.5f,1.5f,(_sizeMask-500)/(float)(Screen.height-sizeMask));
      spriteMask.transform.DOScale(  sizeMask,time).onComplete += () =>
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
         newPart.size               = Mathf.Max( partSprite[i].texture.width ,partSprite[i].texture.height)/(float) originSprite.texture.width;
         newPart.caroMask.sprite    = partSprite[i];
#if UNITY_EDITOR
      //   path                       = AssetDatabase.GetAssetPath(partSpritePos[i]);

         path                       = corFilePaths[i];
         Vector2   content       =   ExtensionClass.ReadVector2FormCORFile(path);

         newPart.transform.localScale = Vector3.one;

         float x = -originSprite.texture.width           / 2f + partSprite[i].texture.width  / 2f + content.x;
         float y = originSprite.texture.height           / 2f - partSprite[i].texture.height / 2f - content.y;
         newPart.transform.localPosition = new Vector3(x /100f,y    /100f, 0);
         
         newPart.AutoGetRef();
         newPart.SetID(i);
         string[] numbers =   newPart.name.Split('_');
         newPart.SetIDColor(int.Parse(numbers[0]) );
         newPart.text.text = numbers[0];
#endif
      }

   }

   [Button]
   public void SetLinePos(SpriteRenderer spriteRenderer,string pathcor)
   {
      Vector2 content =   ExtensionClass.ReadVector2FormCORFile(pathcor);
      Debug.Log(content);
      Debug.Log( spriteRenderer.sprite.texture.width +"_"+ spriteRenderer.sprite.texture.height);
      float   x       = -originSprite.texture.width / 2f + spriteRenderer.sprite.texture.width   / 2f +  content.x;
      float   y       = originSprite.texture.height / 2f -  spriteRenderer.sprite.texture.height / 2f -  content.y;
      Debug.Log(x                                        + " _"                                       + y);
      spriteRenderer.transform.localPosition = new Vector3(x /100f,y                             /100f, 0);
    
   }
   public int CompartTextSize(TextMeshPro v1, TextMeshPro v2)
   {
    
         return v2.fontSize.CompareTo(v1.fontSize); // Ưu tiên theo z thấp
   }

}
