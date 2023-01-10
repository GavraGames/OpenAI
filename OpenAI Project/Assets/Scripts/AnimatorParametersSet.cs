using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorParametersSet : MonoBehaviour
{
    public Animator targetAnimator;

    public string animatorTrigger;
    private void Start()
    {
        targetAnimator.SetTrigger(animatorTrigger);
    }

    private void OnValidate()
    {
        if (targetAnimator == null)
            targetAnimator = GetComponentInChildren<Animator>();
    }

}
