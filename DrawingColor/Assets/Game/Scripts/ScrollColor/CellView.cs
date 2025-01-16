using EnhancedUI.EnhancedScroller;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts._04_Jump_To_Demo_1
{
    public struct OnColorSelectedEvent
    {
        public int             idColor;
    
        /// <summary>
        /// Initializes a new instance of the <see cref="MoreMountains.TopDownEngine.TopDownEngineEvent"/> struct.
        /// </summary>
        /// <param name="eventType">Event type.</param>
        public OnColorSelectedEvent( int _idColor)
        {
            idColor         = _idColor;
        }

        static OnColorSelectedEvent e;
        public static void Trigger(int idColor)
        {
            e.idColor         = idColor;
            MMEventManager.TriggerEvent(e);
        }
    }    
    
    
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
                OnColorSelectedEvent.Trigger(colorID);
            }
        }
    }
}