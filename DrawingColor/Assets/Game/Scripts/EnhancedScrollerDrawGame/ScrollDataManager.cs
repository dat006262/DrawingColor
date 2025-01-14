using System.Collections.Generic;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.EnhancedScrollerDrawGame
{
    [System.Serializable]
    public class Data
    {
        public Sprite           SpriteRender;
        public PictureCotroller PictureCotrollerPrefabs;
        public bool             isLoaded;
      
        public string     imageUrl;
        public Vector2    imageDimensions;


    }

    public class ScrollDataManager : MonoBehaviour, IEnhancedScrollerDelegate
    {

        #region Public Variables

        public EnhancedScroller         Scroller1;
        public EnhancedScrollerCellView masterCellViewPrefab;
        public UnityEngine.UI.Scrollbar HScrollbar;

        [SerializeField]
        public Data[] DataTest;

        #endregion

        #region Private Variables

        private SmallList<Data> _data;

        [ShowInInspector]  private int _cellCount
        {
            get
            {
                if (_data == null)
                {
                    return 0;
                }
                else
                {
                    return _data.Count;
                }
            }   
        }

        #endregion

        #region Unity Method

        private void Start()
        {
            Scroller1.Delegate                  = this;
         //   Scroller1.cellViewVisibilityChanged = CellViewVisibilityChanged;
         //   Scroller1.cellViewWillRecycle       = CellViewWillRecycle;
            LoadData();
        }

        #endregion

        #region Public Method

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return( _data.Count)/2+1;
        }
    
        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return CellViewSize();
        }
    
        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            CellView CellView = scroller.GetCellView(masterCellViewPrefab) as CellView;

            // set the name of the game object to the cell's data index.
            // this is optional, but it helps up debug the objects in 
            // the scene hierarchy.
            CellView.name = "Master Cell " + dataIndex.ToString();

            // in this example, we just pass the data to our cell's view which will update its UI
            CellView.SetData(_data[dataIndex],ref _data, dataIndex * 2);

            // return the cell to the scroller
            return CellView;
        }

        public float CellViewSize()
        {
            return (Screen.width - 40f*3)/2f;
        }
        #endregion

        #region Private Method

        private void LoadData()
        {
            _data = new SmallList<Data>();
            for (var i = 0; i < DataTest.Length; i++)
            {
                _data.Add(DataTest[i]);
            }
               

            // tell the scroller to reload now that we have the data
            Scroller1.ReloadData();
        }

        private void DetailScrollerScrolled(EnhancedScroller scroller, Vector2 val, float scrollPosition)
        {
            UpdateDetailScrollers(scroller.NormalizedScrollPosition);
        }
        private void UpdateDetailScrollers(float normalizedScrollPosition)
        {
            if (HScrollbar.value != normalizedScrollPosition)
            {
                HScrollbar.value = normalizedScrollPosition;
            }
        
        
            Scroller1.RefreshActiveCellViews();
        }
        private void CellViewVisibilityChanged(EnhancedScrollerCellView cellView)
        {
            // cast the cell view to our custom view
            CellView view = cellView as CellView;

            // if the cell is active, we set its data,
            // otherwise we will clear the image back to
            // its default state

            if (cellView.active)
            {
                view.SetData(_data[cellView.dataIndex],ref _data, cellView.dataIndex * 2);
            }
            // else
            //     view.ClearImage();
        }
        private void CellViewWillRecycle(EnhancedScrollerCellView cellView)
        {
            (cellView as CellView).WillRecycle();
        }
        #endregion
   
    }
}