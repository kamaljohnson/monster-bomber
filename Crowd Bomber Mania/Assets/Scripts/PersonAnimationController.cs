using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationType
{
    Walk,
    Run,
    Attack,
    Die
}

public class PersonAnimationController : MonoBehaviour
{

    public Animator animator;

    public void PlayAnimation(AnimationType type)
    {
        animator.Play(GetAnimationNameForType(type), 0, 0f);
    }

    private string GetAnimationNameForType(AnimationType type)
    {
        switch (type)
        {
            case AnimationType.Walk:
                return "Walk Forward In Place";
            case AnimationType.Run:
                return "Run Forward In Place";
            case AnimationType.Attack:
                return "Attack 01";
            case AnimationType.Die:
                return "Die";
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
