using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnhancedScrollerDemos.NestedLinkedScrollers;
using EnhancedScrollerDemos.SuperSimpleDemo;
using EnhancedUI.EnhancedScroller;
using Sirenix.OdinInspector;
using UnityEngine;
using EnhancedUI;
using UnityEngine.Serialization;

public class ScrollDataManager : MonoBehaviour, IEnhancedScrollerDelegate
{

#region Public Variables

    public EnhancedScroller         Scroller1;
    public                                          EnhancedScrollerCellView masterCellViewPrefab;
    public                                          UnityEngine.UI.Scrollbar HScrollbar;

    public float cellViewSize   = 100f;
  //  public float scrollChild    = 2;
  //  public float cellEachScroll = 20f;
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
        Scroller1.Delegate = this;
        LoadData();
    }

#endregion

#region Public Method

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return _data.Count;
    }
    
    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return (dataIndex % 2 == 0 ? 30f : cellViewSize);
    }
    
    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        CellView CellView = scroller.GetCellView(masterCellViewPrefab) as CellView;

        // set the name of the game object to the cell's data index.
        // this is optional, but it helps up debug the objects in 
        // the scene hierarchy.
       CellView.name = "Master Cell " + dataIndex.ToString();

        // in this example, we just pass the data to our cell's view which will update its UI
       CellView.SetData(_data[dataIndex]);

        // return the cell to the scroller
        return CellView;
    }

#endregion

#region Private Method

    private void LoadData()
    {
        _data = new SmallList<Data>();
        for (var i = 0; i < 1000; i++)
            _data.Add(new Data() { someText = "Cell Data Index " + i.ToString() });

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

#endregion
   
}