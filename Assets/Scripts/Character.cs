using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterMouthController MouthController;

    private void Awake()
    {
        MouthController = GetComponentInChildren<CharacterMouthController>();
        Debug.Assert(MouthController != null, "Character mouth controller not found in character hierarchy");
    }
}
