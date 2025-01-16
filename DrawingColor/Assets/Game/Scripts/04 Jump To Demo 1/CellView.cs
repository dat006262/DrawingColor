using EnhancedUI.EnhancedScroller;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts._04_Jump_To_Demo_1
{
    public delegate void SelectedDelegate(EnhancedScrollerCellView cellView);
    public class CellView : EnhancedScrollerCellView
    {
        public          int       DataIndex { get; private set; }
        private         ColorData _data;
        public          Image     selectionPanel;
        public          Image     progressImage;
        public          Image     RenderImage;
      [ReadOnly] public int       colorID;
      [ReadOnly] public int       totalPart;
      [ReadOnly] public int       countPartFilled;
      
      public Color selectedColor;
      public Color unSelectedColor;
      
      public SelectedDelegate selected;
        public void SetData(int dataIndex,ColorData colorData)
        {
            if (_data != null)
            {
                _data.selectedChanged -= SelectedChanged;
            }
            DataIndex = dataIndex;
            
            _data = colorData;
            
            
            RenderImage.sprite       = colorData.colorSprite;
            colorID                  = colorData.colorID;
            totalPart                = colorData.totalPart;
            countPartFilled          = colorData.countPartFilled;
            progressImage.fillAmount = countPartFilled / (float)totalPart;
            
            _data.selectedChanged -= SelectedChanged;
            _data.selectedChanged += SelectedChanged;

            // update the selection state UI
            SelectedChanged(colorData.Selected);
        }
        
        void OnDestroy()
        {
            if (_data != null)
            {
                _data.selectedChanged -= SelectedChanged;
            }
        }
        
        private void SelectedChanged(bool selected)
        {
            selectionPanel.color = (selected ? selectedColor : unSelectedColor);
        }
        
        public void OnSelected()
        {
            if (selected != null)
            {
                selected(this);
            }
        }
    }
}