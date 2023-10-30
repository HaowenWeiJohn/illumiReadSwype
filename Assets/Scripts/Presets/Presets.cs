using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class Presets
{




    public static string EventMarkerLSLOutletStreamName = "illumiReadSwypeLSL";
    public static string EventMarkerLSLOutletStreamType = "EventMarker";
    public static string EventMarkerLSLOutletStreamID = "1";
    public static int EventMarkerChannelNum = 4; // block marker index 0
    public static float EventMarkerNominalSamplingRate = 1;


    public enum EventMarkerChannelInfo
    {
        BlockChannelIndex = 0,
        ExperimentStateChannelIndex = 1,
        UserInputsChannelIndex = 2, // the 0-5 is row, 7-11 is column
    }




    /// <summary>
    /// Varjo gaze data lsl outlet stream
    /// </summary>
    public static string GazeDataLSLOutletStreamName = "Varjo";
    public static string GazeDataLSLOutletStreamType = "GazeData";
    public static string GazeDataLSLOutletStreamID = "2";
    public static int GazeDataChannelNum = 21;
    public static float GazeDataNominalSamplingRate = 200;






    public static string GameManagerName = "GameManager";


    public static KeyCode NextStateKey = KeyCode.KeypadEnter;
    public static KeyCode InterruptKey = KeyCode.Escape;


    public enum UserInputTypes
    {
        Select = 1
    }



    public enum State
    {
        IdleState = 0,
        RunningState = 1,
        EndingState = 2,
        InterruptState = 3
    }


    public enum GameState
    {
        InitState = 0,
        IdleState = 1,
        RunningState = 2,
        EndingState = 3,
        InterruptState = 4
    }


    public enum BlockState
    {
        IdleState = 0,
        RunningState = 1,
        EndingState = 2,
        InterruptState = 3
    }



    public enum ExperimentState
    {
        InitState = 0,

        CalibrationState = 1,
        StartState = 2,
        IntroductionInstructionState = 3,
        
        KeyboardDewellTimeIntroductionState = 4,
        KeyboardDewellTimeState = 5,
        
        KeyboardClickIntroductionState = 6,
        KeyboardClickState = 7,

        KeyboardIllumiReadSwypeIntroductionState = 8,
        KeyboardIllumiReadSwypeState = 9,

        KeyboardFreeSwitchInstructionState = 10,
        KeyboardFreeSwitchState = 11,

        FeedbackState = 12,

        EndState = 13,
    }
    
    public enum ExperimentBlock
    {
        InitBlock = 0,
        StartBlock = 1,
        IntroductionBlock = 2,
        PracticeBlock = 3,
        TestBlock = 4,
        EndBlock = 5,
    }

    public static List<ExperimentState> StartBlock = new List<ExperimentState> {
        ExperimentState.StartState,
        //ExperimentState.IntroductionInstructionState
    };


    public static List<ExperimentState> IntroductionBlock = new List<ExperimentState> {
        //ExperimentState.StartState,
        ExperimentState.IntroductionInstructionState
    };



    public static List<ExperimentState> EndBlock = new List<ExperimentState> {
        ExperimentState.EndState
    };




    //public static List<ExperimentState> NoAOIAugmentationBlock = new List<ExperimentState> {
    //    ExperimentState.CalibrationState,
    //    ExperimentState.NoAOIAugmentationState,
    //    ExperimentState.FeedbackState
    //};


    //public static List<ExperimentState> StaticAOIAugmentationBlock = new List<ExperimentState> {
    //    ExperimentState.CalibrationState,
    //    ExperimentState.StaticAOIAugmentationState,
    //    ExperimentState.FeedbackState
    //};

    //public static List<ExperimentState> InteractiveAOIAugmentationBlock = new List<ExperimentState> {
    //    ExperimentState.CalibrationState,
    //    ExperimentState.InteractiveAOIAugmentationState,
    //    ExperimentState.FeedbackState
    //};

    //public static List<ExperimentState> NoAOIAugmentationBlockWithInstructionBlock = new List<ExperimentState> {
    //    ExperimentState.NoAOIAugmentationInstructionState,
    //    ExperimentState.CalibrationState,
    //    ExperimentState.NoAOIAugmentationState,
    //    ExperimentState.FeedbackState
    //};

    //public static List<ExperimentState> StaticAOIAugmentationBlockWithInstructionBlock = new List<ExperimentState> {
    //    ExperimentState.StaticAOIAugmentationInstructionState,
    //    ExperimentState.CalibrationState,
    //    ExperimentState.StaticAOIAugmentationState,
    //    ExperimentState.FeedbackState
    //};

    //public static List<ExperimentState> InteractiveAOIAugmentationBlockWithInstructionBlock = new List<ExperimentState> {
    //    ExperimentState.InteractiveAOIAugmentationInstructionState,
    //    ExperimentState.CalibrationState,
    //    ExperimentState.InteractiveAOIAugmentationState,
    //    ExperimentState.FeedbackState
    //};

    //public static string ProjectDirectoryPath = Directory.GetCurrentDirectory();
    //public static string PracticeBlockImageDirectoryPath = Path.Combine(ProjectDirectoryPath, "Assets", "Prefabs", "ExperimentImages", "Practice"); //"D:\\HaowenWei\\Rena\\illumiRead\\AOIAugmentation\\experiment_report\\practice";
    //public static string TestBlockImageDirectoryPath = Path.Combine(ProjectDirectoryPath, "Assets", "Prefabs", "ExperimentImages", "Test");//"D:\\HaowenWei\\Rena\\illumiRead\\AOIAugmentation\\experiment_report\\test";




}
