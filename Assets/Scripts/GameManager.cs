using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEditor;
using static Presets;

public class GameManager : MonoBehaviour
{

    public Presets.GameState currentGameState;

    public StateController currentState;
    public BlockController currentBlock;
    //public List<Presets.ExperimentState> expermentProcedure = new List<Presets.ExperimentState>();
    public List<Presets.ExperimentBlock> experimentBlocks;


    /// <summary>
    ///  avaliable states
    /// </summary>
    /// 

    //[Header("Network Experiment State")]
    //public UserInputController userInputController;

    [Header("Experiment Blocks")]

    public InitBlockController initBlockController;
    public StartBlockController startBlockController;
    public IntroductionBlockController introductionBlockController;
    public PracticeBlockController practiceBlockController;
    public TrainBlockController trainBlockController;
    public TestBlockController testBlockController;
    public EndBlockController endBlockController;


    [Header("Init State")]
    public InitStateController initStateController;

    [Header("Calibration State")]
    public CalibrationStateController calibrationStateController;

    [Header("Start State")]
    public StartStateController startStateController;

    [Header("Introduction Instruction State")]
    public IntroductionInstructionStateController introductionInstructionStateController;

    [Header("KeyboardDewellTime")]
    public KeyboardDewellTimeIntroductionStateController keyboardDewellTimeIntroductionStateController;
    public KeyboardDewellTimeStateController keyboardDewellTimeStateController;


    [Header("Introduction Instruction State")]
    public KeyboardClickIntroductionStateController KeyboardClickIntroductionStateController;
    public KeyboardClickStateController KeyboardClickStateController;

    [Header("Introduction Instruction State")]
    public KeyboardIllumiReadSwypeIntroductionStateController keyboardIllumiReadSwypeIntroductionStateController;
    public KeyboardIllumiReadSwypeStateController keyboardIllumiReadSwypeStateController;

    [Header("Introduction Instruction State")]
    public KeyboardFreeSwitchInstructionStateController keyboardFreeSwitchInstructionStateController;
    public KeyboardFreeSwitchState keyboardFreeSwitchStateController;

    [Header("FeedbackState")]
    public FeedbackStateController feedbackStateController;

    [Header("EndState")]
    public EndStateController endStateController;



    [Header("Network Controllers")]
    public EventMarkerLSLOutletController eventMarkerLSLOutletController;
    public VarjoGazeDataLSLOutletController varjoGazeDataLSLOutletController;
    public illumiReadSwypeUserInputLSLOutletController varjoGazeOnKeyboardLSLOutletController;


    //int experimentStateIndex = 0;
    int experimentBlockIndex = 0;



    // parameters for frame rate
    private float deltaTime = 0.0f;
    private float fps = 0.0f;

    void CalculateFrameRate()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        fps = 1.0f / deltaTime;
        Debug.Log("FPS: " + fps);
    }



    void Start()
    {

        //expermentProcedure = ExperimentPreset.ConstructExperimentStates();


        //stateSelector(expermentProcedure[experimentStateIndex]);
        //currentGameState.enterState();
        currentGameState = Presets.GameState.IdleState;
        experimentBlocks = ExperimentPreset.ConstructExperimentBlocks(); // list of Enums


        // use a key to enter the first game block.


    }

    // Update is called once per frame
    void Update()
    {
        //CalculateFrameRate();
        if (currentGameState == Presets.GameState.IdleState) // the previous block has been finished
        {
            // check if there is more blocks to run

            if (experimentBlockIndex < experimentBlocks.Count)
            {
                blockSelector(experimentBlocks[experimentBlockIndex]);
                currentBlock.enterBlock();
                currentGameState = Presets.GameState.RunningState;
                experimentBlockIndex += 1;
            }
            else
            {
                Debug.Log("Experiment End");
                // end the experiment and exit play mode
                EditorApplication.isPlaying = false;
            }


        }



        //if (currentGameState.getCurrentState()==Presets.State.EndingState && currentGameState!=endStateController)
        //{
        //    // reset previous state
        //    currentGameState.setCurrentState(Presets.State.IdleState);
        //    experimentStateIndex += 1;
        //    // get new current state
        //    stateSelector(expermentProcedure[experimentStateIndex]);
        //    // activate current state
        //    currentGameState.enterState();
        //    Debug.Log("State Updated");
        //}
    }



    public void blockSelector(Presets.ExperimentBlock nextBlock)
    {
        switch (nextBlock)
        {

            case Presets.ExperimentBlock.InitBlock:
                currentBlock = initBlockController;
                break;


            case Presets.ExperimentBlock.StartBlock:
                currentBlock = startBlockController;
                break;

            case Presets.ExperimentBlock.IntroductionBlock:
                currentBlock = introductionBlockController;
                break;

            case Presets.ExperimentBlock.PracticeBlock:
                currentBlock = practiceBlockController;
                break;

            case Presets.ExperimentBlock.TrainBlock:
                currentBlock = trainBlockController;
                break;

            case Presets.ExperimentBlock.TestBlock:
                currentBlock = testBlockController;
                break;
            case Presets.ExperimentBlock.EndBlock:
                currentBlock = endBlockController;
                break;

        }

    }



    //public void stateSelector(Presets.ExperimentState nextState)
    //{
    //    //switch (nextState)
    //    //{
    //    //    case Presets.ExperimentState.StartState:
    //    //        currentGameState = startStateController;
    //    //        break;
    //    //    case Presets.ExperimentState.IntroductionInstructionState:
    //    //        currentGameState = introductionInstructionStateController;
    //    //        break;
    //    //    case Presets.ExperimentState.PracticeInstructionState:
    //    //        currentGameState = practiceInstructionStateController;
    //    //        break;
    //    //    case Presets.ExperimentState.CalibrationState:
    //    //        currentGameState = calibrationStateController;
    //    //        break;

    //    //    /////////////////////////////////////
    //    //    case Presets.ExperimentState.NoAOIAugmentationInstructionState:
    //    //        currentGameState = noAOIAugmentationInstructionStateController;
    //    //        break;

    //    //    case Presets.ExperimentState.StaticAOIAugmentationInstructionState:
    //    //        currentGameState = staticAOIAugmentationInstructionStateController;
    //    //        break;

    //    //    case Presets.ExperimentState.InteractiveAOIAugmentationInstructionState:
    //    //        currentGameState = interactiveAOIAugmentationInstructionStateController;
    //    //        break;
    //    //    /////////////////////////////////////

    //    //    case Presets.ExperimentState.NoAOIAugmentationState:
    //    //        currentGameState = noAOIAugmentationStateController;
    //    //        break;

    //    //    case Presets.ExperimentState.StaticAOIAugmentationState:
    //    //        currentGameState = staticAOIAugmentationStateController;
    //    //        break;

    //    //    case Presets.ExperimentState.InteractiveAOIAugmentationState:
    //    //        currentGameState = interactiveAOIAugmentationStateController;
    //    //        break;

    //    //    case Presets.ExperimentState.FeedbackState:
    //    //        currentGameState = feedbackStateController;
    //    //        break;

    //    //    case Presets.ExperimentState.EndState:
    //    //        currentGameState = endStateController;
    //    //        break;


    //    //}


    //}

    //void WelcomeStateToInstructionState()
    //void initGame()
    //{

    //}



}
