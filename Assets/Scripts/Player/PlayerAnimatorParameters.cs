using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorParameters
{
    Dictionary<string, int> _animParamIDs;
    public Dictionary<string, int> animParamIDs
    {
        get { return _animParamIDs; }
    }

    public PlayerAnimatorParameters()
    {
        _animParamIDs = new Dictionary<string, int>()
        {
            ["moveX"] = Animator.StringToHash("moveX"),
            ["moveY"] = Animator.StringToHash("moveY"),
            ["idle"] = Animator.StringToHash("idle"),
            ["walk"] = Animator.StringToHash("walk"),
            ["sprint"] = Animator.StringToHash("sprint"),
            ["crouch"] = Animator.StringToHash("crouch"),
            ["slide"] = Animator.StringToHash("slide"),
            ["jump"] = Animator.StringToHash("jump")
        };
    }

}
