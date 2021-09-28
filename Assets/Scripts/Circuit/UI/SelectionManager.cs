using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZxDungeon.UI;

public  class SelectionManager 
{
    private static readonly SelectionManager instance = new SelectionManager();
    public UiElement selectedElement = null;
    public UiElement hoveredElement = null;

    static SelectionManager()
    {
    }

    private SelectionManager()
    {
    }

    public static SelectionManager Instance
    {
        get
        {
            return instance;
        }
    }
}
