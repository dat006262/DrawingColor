using DG.Tweening;
using EnhancedUI.EnhancedScroller;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts._04_Jump_To_Demo_1
{
    public enum ColorButtonAction
    {
        Selected,
        Finish
    }
    public struct OnColorButtonEvent
    {
        public ColorButtonAction onColorButtonAction;
        public int               idColor;
        public int               dataIndex;
        /// <summary>
        /// Initializes a new instance of the <see cref="MoreMountains.TopDownEngine.TopDownEngineEvent"/> struct.
        /// </summary>
        /// <param name="eventType">Event type.</param>
        public OnColorButtonEvent(ColorButtonAction _onColorButtonAction, int _idColor, int _dataIndex)
        {
            onColorButtonAction = _onColorButtonAction;
            dataIndex           = _dataIndex;
            idColor             = _idColor;
        }

        static OnColorButtonEvent e;
        public static void Trigger(ColorButtonAction onColorButtonAction, int idColor)
        {
            e.onColorButtonAction = onColorButtonAction;
            e.idColor         = idColor;
            MMEventManager.TriggerEvent(e);
        }
        public static void Trigger(ColorButtonAction onColorButtonAction, int idColor, int   DataIndex)
        {
            e.onColorButtonAction = onColorButtonAction;
            e.idColor             = idColor;
            e.dataIndex           = DataIndex;
            MMEventManager.TriggerEvent(e);
        }
    }    
    
    
    public delegate void SelectedDelegate(EnhancedScrollerCellView cellView);
    public class CellView : EnhancedScrollerCellView,MMEventListener<PartClickActionEvent>
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
        private void OnEnable()
        {
            this.MMEventStartListening<PartClickActionEvent>();
        }

        private void OnDisable()
        {
            this.MMEventStopListening<PartClickActionEvent>();
        }

        public void OnMMEvent(PartClickActionEvent eventType)
        {
            if (colorID == eventType.idColor &&eventType.PartClickAction == PartClickAction.OnPartFillStart)
            {
                countPartFilled++;
                progressImage.DOFillAmount( countPartFilled / (float)totalPart, 0.5f);
                if (countPartFilled == totalPart)
                {
                    OnColorButtonEvent.Trigger(ColorButtonAction.Finish,colorID,DataIndex: DataIndex);
                    
                }
            }
        

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
                OnColorButtonEvent.Trigger(ColorButtonAction.Selected,colorID);
            }
        }
    }
}