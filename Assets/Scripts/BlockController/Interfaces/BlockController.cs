using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Presets.ExperimentState> experimentStates = new List<Presets.ExperimentState>();
    public Presets.ExperimentBlock experimentBlock;
    public GameManager gameManager;
    public Presets.BlockState blockState;

    int experimentStateIndex = 0;

    private ExperimentManager experimentManager;

    private int blockConfiguration;
    void Awake()
    {
        
        experimentManager = GameObject.Find("ExperimentManager").GetComponent<ExperimentManager>();
        if(experimentManager.isPractice == false)
        {
            // get a random number from 0-5
            blockConfiguration = Random.Range(0, 6);
        }
        else
        {   
            // do not change configuration for practice trials
            blockConfiguration = 0;
        }
        Debug.Log("Block Configuration: " + blockConfiguration);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(gameManager.currentState.getCurrentState() == Presets.State.EndingState)
        {
            gameManager.currentState.setCurrentState(Presets.State.IdleState);
            
            // introduction instruction state ends
            if(experimentStateIndex == 3)
            {
                switch(blockConfiguration)
                {
                    case 0:
                        experimentStateIndex = 4;
                        break;
                    case 1:
                        experimentStateIndex = 4;
                        break;
                    case 2:
                        experimentStateIndex = 6;
                        break;
                    case 3:
                        experimentStateIndex = 6;
                        break;
                    case 4:
                        experimentStateIndex = 8;
                        break;
                    case 5:
                        experimentStateIndex = 8;
                        break;
                }
            }
            // end of hand type state
            else if(experimentStateIndex == 5)
            {
                switch(blockConfiguration)
                {
                    case 0:
                        experimentStateIndex = 6;
                        break;
                    case 1:
                        experimentStateIndex = 8;
                        break;
                    case 2:
                        experimentStateIndex = 8;
                        break;
                    case 3:
                        experimentStateIndex = 12;
                        break;
                    case 4:
                        experimentStateIndex = 6;
                        break;
                    case 5:
                        experimentStateIndex = 12;
                        break;
                }
            }
            else if(experimentStateIndex == 7)
            {
                switch(blockConfiguration)
                {
                    case 0:
                        experimentStateIndex = 8;
                        break;
                    case 1:
                        experimentStateIndex = 12;
                        break;
                    case 2:
                        experimentStateIndex = 4;
                        break;
                    case 3:
                        experimentStateIndex = 8;
                        break;
                    case 4:
                        experimentStateIndex = 12;
                        break;
                    case 5:
                        experimentStateIndex = 4;
                        break;
                }
            }
            else if(experimentStateIndex == 9)
            {
                switch(blockConfiguration)
                {
                    case 0:
                        experimentStateIndex = 12;
                        break;
                    case 1:
                        experimentStateIndex = 6;
                        break;
                    case 2:
                        experimentStateIndex = 12;
                        break;
                    case 3:
                        experimentStateIndex = 4;
                        break;
                    case 4:
                        experimentStateIndex = 4;
                        break;
                    case 5:
                        experimentStateIndex = 6;
                        break;
                }
            }
            else
            {
                experimentStateIndex += 1;
            }

            Debug.Log("Experiment State Index: " + experimentStateIndex);

            if (experimentStateIndex < experimentStates.Count)
            {
                stateSelector(experimentStates[experimentStateIndex]);
                onExperimentStateEntered();
                gameManager.currentState.enterState();
            }
            else
            {
                exitBlock();
            }

        }
    }


    public virtual void onExperimentStateEntered()
    {

    }



    public virtual void initExperimentBlockStates()
    {
        
    }


    public void enterBlock()
    {

        // send event marker
        Debug.Log("enterBlock: " + experimentBlock);
        EnableSelf();
        gameManager.eventMarkerLSLOutletController.sendBlockOnEnterMarker(experimentBlock);
        experimentStateIndex = 0;
        stateSelector(experimentStates[experimentStateIndex]);
        onExperimentStateEntered();
        gameManager.currentState.enterState();

    }

    public void exitBlock()
    {
        // send event marker
        Debug.Log("existBlock: " + experimentBlock);
        experimentStateIndex = 0;
        gameManager.currentGameState = Presets.GameState.IdleState;
        gameManager.eventMarkerLSLOutletController.sendBlockOnExitMarker(experimentBlock);
        DisableSelf();

    }




    public void stateSelector(Presets.ExperimentState nextState)
    {
        switch (nextState)
        {

            case Presets.ExperimentState.InitState:
                gameManager.currentState = gameManager.initStateController;
                break;

            case Presets.ExperimentState.CalibrationState:
                gameManager.currentState = gameManager.calibrationStateController;
                break;

            case Presets.ExperimentState.StartState:
                gameManager.currentState = gameManager.startStateController;
                break;
            
            case Presets.ExperimentState.IntroductionInstructionState:
                gameManager.currentState = gameManager.introductionInstructionStateController;
                break;


            case Presets.ExperimentState.KeyboardDewellTimeIntroductionState:
                gameManager.currentState = gameManager.keyboardDewellTimeIntroductionStateController;
                break;
            case Presets.ExperimentState.KeyboardDewellTimeState:
                gameManager.currentState = gameManager.keyboardDewellTimeStateController;
                break;
            case Presets.ExperimentState.KeyboardClickIntroductionState:
                gameManager.currentState = gameManager.KeyboardClickIntroductionStateController;
                break;
            case Presets.ExperimentState.KeyboardClickState:
                gameManager.currentState = gameManager.KeyboardClickStateController;
                break;
            case Presets.ExperimentState.KeyboardIllumiReadSwypeIntroductionState:
                gameManager.currentState = gameManager.keyboardIllumiReadSwypeIntroductionStateController;
                break;
            case Presets.ExperimentState.KeyboardIllumiReadSwypeState:
                gameManager.currentState = gameManager.keyboardIllumiReadSwypeStateController;
                break;
            case Presets.ExperimentState.KeyboardFreeSwitchInstructionState:
                gameManager.currentState = gameManager.keyboardFreeSwitchInstructionStateController;
                break;
            case Presets.ExperimentState.KeyboardFreeSwitchState:
                gameManager.currentState = gameManager.keyboardFreeSwitchStateController;
                break;

            case Presets.ExperimentState.FeedbackState:
                gameManager.currentState = gameManager.feedbackStateController;
                break;

            case Presets.ExperimentState.EndState:
                gameManager.currentState = gameManager.endStateController;
                break;

        }


    }

    public void EnableSelf()
    {
        gameObject.SetActive(true);
    }

    public void DisableSelf()
    {
        gameObject.SetActive(false);
    }

}
