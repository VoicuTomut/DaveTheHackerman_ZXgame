using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager
{
    public List<UiAction> actions;
    private static readonly ActionManager instance = new ActionManager();
    static ActionManager()
    {
    }

    private ActionManager()
    {
    }

    public static ActionManager Instance
    {
        get
        {
            return instance;
        }
    }
    public void AddAction(UiAction action)
    {
        actions.Add(action);
    }
}
