using System;
using UnityEngine;

namespace UI.Windows
{
    public class InfoWindowData : UIData
    {
        public string Title;
        public string Description;
        public Sprite Icon;
        public Action OKAction;
        
    }
}