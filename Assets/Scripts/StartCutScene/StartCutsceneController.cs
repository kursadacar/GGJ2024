using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCutsceneController : MonoBehaviour
{
    private const float AnimationDuration = 3.0f;

    private enum StartCutsceneState
    {
        None,
        MainCharacterWalking,
        EncounterWithWitch,
        CurseInProgress,
        HorseIsWalking,
    }

    private StartCutsceneState _startCutsceneState = StartCutsceneState.None;
    private float _animationTimer = 0.0f;

    public GameObject MainCharacterWithLegs;

    // Start is called before the first frame update
    void Start()
    {
        _animationTimer = 0.0f;
        ChangeStartCutsceneState(StartCutsceneState.MainCharacterWalking);
    }

    // Update is called once per frame
    void Update()
    {
        _animationTimer += Time.deltaTime;

        if (_animationTimer >= AnimationDuration)
        {
            var nextCutsceneState = ((int)_startCutsceneState) + 1;
            ChangeStartCutsceneState((StartCutsceneState)nextCutsceneState);
            _animationTimer = 0.0f;
        }
        else
        {
            UpdateCurrentCutsceneState();
        }
    }

    private void ChangeStartCutsceneState(StartCutsceneState startCutsceneState)
    {
        if (_startCutsceneState != startCutsceneState)
        {
            switch (startCutsceneState)
            {
                case StartCutsceneState.MainCharacterWalking:
                    _startCutsceneState = startCutsceneState;

                    break;

                case StartCutsceneState.EncounterWithWitch:
                    MainCharacterWithLegs.SetActive(false);

                    break;

                default:

                    break;
            }
        }
        else
        {
            Debug.Assert(false, "Duplicate startCutsceneState change call!");
        }
    }

    private void UpdateCurrentCutsceneState()
    {
        switch (_startCutsceneState)
        {
            case StartCutsceneState.MainCharacterWalking:

                MainCharacterWithLegs.transform.position = new Vector3(MainCharacterWithLegs.transform.position.x + 1, MainCharacterWithLegs.transform.position.y, MainCharacterWithLegs.transform.position.z);

                break;

            default:

                break;
        }
    }
}
