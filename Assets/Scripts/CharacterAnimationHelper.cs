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

    public static void SetHorizontalVelocity(this Animator animator, float velocity)
    {
        animator.SetFloat(HorizontalVelocityParameter, velocity);
    }

    public static void SetVerticalVelocity(this Animator animator, float velocity)
    {
        animator.SetFloat(VerticalVelocityParameter, velocity);
    }
}
