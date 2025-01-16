using System;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts._04_Jump_To_Demo_1
{
    /// <summary>
    /// This demo shows how to jump to an index in the scroller. You can jump to a position before
    /// or after the cell. You can also include the spacing before or after the cell.
    /// </summary>
    public class CollorIDController : MonoBehaviour, IEnhancedScrollerDelegate,MMEventListener<PartClickActionEvent>
    {
  
        private List<ColorData> _data;

        public EnhancedScroller hScroller;

        public InputField jumpIndexInput;

        public EnhancedScrollerCellView cellViewPrefab;

        public EnhancedScroller.TweenType hScrollerTweenType = EnhancedScroller.TweenType.immediate;
        public float                      hScrollerTweenTime = 0f;
        public RectTransform              holder;
        void Start()
        {
           
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
            if (eventType.PartClickAction == PartClickAction.OnPartFillStart)
            {
                ColorData colorData = _data.Find(x => x.colorID == eventType.idColor);
                colorData.countPartFilled++;
            }

            if (eventType.PartClickAction == PartClickAction.OnPartFillComplete)
            {
                ColorData colorData = _data.Find(x => x.colorID == eventType.idColor);

                if (colorData.countPartFilled == colorData.totalPart)
                {
                    _data.Remove(colorData);
                    hScroller.ReloadData();
                    
                }
            }
           
        
        }
        private void Reload()
        {
            if (_data != null)
            {
                for (var i = 0; i < _data.Count; i++)
                {
                    _data[i].selectedChanged = null;
                }
            }
        }
        [Button]
        public void SetUp( List<ColorData> inputData)
        {
            _data = new List<ColorData>();
            Reload();
            Application.targetFrameRate = 60;
            hScroller.Delegate          = this;
            foreach (var item in inputData)
            {
                _data  .Add(new ColorData()
                {
                    colorSprite     =item.colorSprite,
                    colorID         = item.colorID,
                    totalPart       = item.totalPart,
                    countPartFilled = item.countPartFilled,
                });
            }

            Debug.Log(inputData.Count);
            hScroller.ReloadData();
            
        }
        #region UI Handlers

        private void CellViewSelected(EnhancedScrollerCellView cellView)
        {
            if (cellView == null)
            {
            }
            else
            {
                var selectedDataIndex = (cellView as CellView).dataIndex;

                for (var i = 0; i < _data.Count; i++)
                {
                    _data[i].Selected = (selectedDataIndex == i);
                }


            }
        }

        #endregion

        #region EnhancedScroller Handlers

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            // in this example, we just pass the number of our data elements
            return _data.Count;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            
                return (holder.GetSize().y- hScroller.padding.top- hScroller.padding.bottom);
        }

       
        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            CellView cellView = scroller.GetCellView(cellViewPrefab) as CellView;
            cellView.selected = CellViewSelected;
            cellView.name     = "Cell " + dataIndex.ToString();
            cellView.SetData(dataIndex,_data[dataIndex]);
            return cellView;
        }

        #endregion

    
    }
}
