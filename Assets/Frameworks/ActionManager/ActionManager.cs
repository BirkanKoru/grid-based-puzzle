using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : SingletonComponent<ActionManager>
{
    private Dictionary<string, Action<ActionParam>> actions = new();

    public void StartListening(string actionName, Action<ActionParam> listener)
    {
        Action<ActionParam> thisAction;

        if(actions.TryGetValue(actionName, out thisAction))
        {
            thisAction += listener;
            actions[actionName] = thisAction;

        } else {

            thisAction += listener;
            actions.Add(actionName, thisAction);
        }
    }

    public void StopListening(string actionName, Action<ActionParam> listener)
    {
        if(actions.TryGetValue(actionName, out Action<ActionParam> thisAction))
        {
            thisAction -= listener;
            actions[actionName] = thisAction;
        }
    }

    public void InvokeAction(string actionName, ActionParam actionParam)
    {
        if(actions.TryGetValue(actionName, out Action<ActionParam> thisAction))
        {
            thisAction?.Invoke(actionParam);
        }
    }
}
