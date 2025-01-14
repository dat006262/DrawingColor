using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.EnhancedScrollerDrawGame
{
    public class RowCellView : MonoBehaviour
    {
        public RectTransform container;
        public Image       notFillImage;

        /// <summary>
        /// This function just takes the Demo data and displays it
        /// </summary>
        /// <param name="data"></param>
        public void SetData(Game.Scripts.EnhancedScrollerDrawGame.Data data)
        {
            container.gameObject.SetActive(data != null);
            if(data == null) return;
            if (data.SpriteRender != null)
            {
                notFillImage.sprite = data.SpriteRender;
            }
            // PictureCotroller picture = Instantiate(data.PictureCotrollerPrefabs, container.transform);
            // picture.transform.localPosition = Vector3.zero;
            // picture.transform.localScale    = Vector3.one * container.sizeDelta.x / picture.size.x;
            // if (data != null)
            // {
            // }
        }
    }
}
