using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAdditiveActions : MonoBehaviour
{
    readonly List<UnityAction> _actions = new();
    public void AddAction(UnityAction action)
    {
        _actions.Add(action);
    }

    public void InvokeAll()
    {
        foreach (UnityAction action in _actions)
        {
            action.Invoke();
        }
        _actions.Clear();
    }

}
