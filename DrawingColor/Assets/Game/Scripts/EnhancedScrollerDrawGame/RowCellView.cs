using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.EnhancedScrollerDrawGame
{
    public struct OpenPictureActionEvent
    {
        public string pictureName ;
        public RectTransform imageTransform;
        public PictureCotroller pictureCotroller;
        /// <summary>
        /// Initializes a new instance of the <see cref="MoreMountains.TopDownEngine.TopDownEngineEvent"/> struct.
        /// </summary>
        /// <param name="eventType">Event type.</param>
        public OpenPictureActionEvent(string _pictureName,RectTransform _imageTransform,PictureCotroller _pictureCotroller)
        {
            pictureName    = _pictureName;
            imageTransform = _imageTransform;
            pictureCotroller = _pictureCotroller;
        }

        static OpenPictureActionEvent e;
        public static void Trigger(string _pictureName,RectTransform _imageTransform, PictureCotroller _pictureCotroller)
        {
            e.pictureName      = _pictureName;
            e.imageTransform   = _imageTransform;
            e.pictureCotroller = _pictureCotroller;
            MMEventManager.TriggerEvent(e);
        }
    }     
    public class RowCellView : MonoBehaviour
    {
        public  RectTransform                              container;
        public  Image                                      notFillImage;
        public  Sprite                                     defaultSprite;
        private Game.Scripts.EnhancedScrollerDrawGame.Data _data;
        #region Private Variables

        private bool isLoaded;
        

        #endregion
        /// <summary>
        /// This function just takes the Demo data and displays it
        /// </summary>
        /// <param name="data"></param>
        public void SetData(Game.Scripts.EnhancedScrollerDrawGame.Data data)
        {
            _data = data;
            container.gameObject.SetActive(data != null);
            if(data == null) return;
            if (data.SpriteRender != null)
            {
                isLoaded            = true;
                notFillImage.sprite = data.SpriteRender;
            }
            else
            {
                isLoaded            = false;
                notFillImage.sprite = defaultSprite;
            }
            
            // PictureCotroller picture = Instantiate(data.PictureCotrollerPrefabs, container.transform);
            // picture.transform.localPosition = Vector3.zero;
            // picture.transform.localScale    = Vector3.one * container.sizeDelta.x / picture.size.x;
            // if (data != null)
            // {
            // }
        }

        public void OpenGamePlay()
        {
            if(!isLoaded) return;
            //Move the Picture to the Button
            //Zoom to Center , Every thing behind fade
            OpenPictureActionEvent.Trigger("Bear",notFillImage.GetComponent<RectTransform>(),_data.PictureCotrollerPrefabs);
            Debug.Log("Open Picture Bear To TEST");
        }
    }
}
