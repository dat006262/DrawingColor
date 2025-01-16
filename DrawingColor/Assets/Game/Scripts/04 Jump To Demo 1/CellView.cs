using EnhancedUI.EnhancedScroller;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace Game.Scripts._04_Jump_To_Demo_1
{
    public class CellView : EnhancedScrollerCellView
    {
        public          Image progressImage;
        public          Image RenderImage;
      [ReadOnly] public int   colorID;
      [ReadOnly] public int   totalPart;
      [ReadOnly] public int   countPartFilled;
        public void SetData(ColorData colorData)
        {
            
            RenderImage.sprite       = colorData.colorSprite;
            colorID                  = colorData.colorID;
            totalPart                = colorData.totalPart;
            countPartFilled          = colorData.countPartFilled;
            progressImage.fillAmount = countPartFilled / (float)totalPart;
        }
    }
}