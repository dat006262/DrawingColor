using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts._04_Jump_To_Demo_1
{
    /// <summary>
    /// This demo shows how to jump to an index in the scroller. You can jump to a position before
    /// or after the cell. You can also include the spacing before or after the cell.
    /// </summary>
    public class CollorIDController : MonoBehaviour, IEnhancedScrollerDelegate
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

        [Button]
        public void SetUp( List<ColorData> inputData)
        {
            Application.targetFrameRate = 60;
            hScroller.Delegate          = this;
            _data                       = inputData;

            hScroller.ReloadData();
            
        }
        #region UI Handlers



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

            cellView.name = "Cell " + dataIndex.ToString();
            cellView.SetData(_data[dataIndex]);
            return cellView;
        }

        #endregion
    }
}
