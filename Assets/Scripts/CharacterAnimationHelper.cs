using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal static class CharacterAnimationHelper
{
    public const string HorizontalVelocityParameter = "HorizontalVelocity";
    public const string VerticalVelocityParameter = "VerticalVelocity";
    public const string AttackAnimationTrigger = "AttackTrigger";

    public static void SetHorizontalVelocity(this Animator animator, float velocity)
    {
        animator.SetFloat(HorizontalVelocityParameter, velocity);
    }

    public static void SetVerticalVelocity(this Animator animator, float velocity)
    {
        animator.SetFloat(VerticalVelocityParameter, velocity);
    }

    public static void StartAttackAnimation(this Animator animator)
    {
        //animator.Play("Attack");
        //animator.SetTrigger(AttackAnimationTrigger);
    }
}
