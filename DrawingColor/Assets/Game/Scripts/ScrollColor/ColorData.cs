
using UnityEngine;

namespace Game.Scripts._04_Jump_To_Demo_1
{
    public delegate void SelectedChangedDelegate(bool val);
    public class ColorData
    {
        public  SelectedChangedDelegate selectedChanged;
        public  Sprite                  colorSprite;
        public  int                     colorID;
        public  int                     totalPart;
        public  int                     countPartFilled;
     
        private bool                    _selected;

        
        public bool Selected
        {
            get { return _selected; }
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    if (selectedChanged != null) selectedChanged(_selected);
                }
            }
        }
    }
}