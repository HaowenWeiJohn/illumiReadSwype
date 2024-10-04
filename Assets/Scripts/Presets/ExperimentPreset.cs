using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ExperimentPreset
{

    public static List<Presets.ExperimentBlock> ConstructExperimentBlocks()
    {
        List<Presets.ExperimentBlock> ExperimentBlock = new List<Presets.ExperimentBlock> {

            // Presets.ExperimentBlock.InitBlock,
            // Presets.ExperimentBlock.StartBlock,
            // Presets.ExperimentBlock.IntroductionBlock,
            // Presets.ExperimentBlock.PracticeBlock,
            // Presets.ExperimentBlock.TrainBlock,
            // Presets.ExperimentBlock.TestBlock,
            // Presets.ExperimentBlock.EndBlock
            Presets.ExperimentBlock.PracticeBlock
        
        };

        return ExperimentBlock;
    }

    public static List<Presets.ExperimentState> ConstructInitBlock()
    {
        
        List<Presets.ExperimentState> ExperimentStates = new List<Presets.ExperimentState> {

            Presets.ExperimentState.InitState
        
        };

        return ExperimentStates;
    }


    public static List<Presets.ExperimentState> ConstructStartBlock()
    {
        List<Presets.ExperimentState> ExperimentStates = new List<Presets.ExperimentState>
        {

            Presets.ExperimentState.StartState
        
        };
        return ExperimentStates;
    }


    public static List<Presets.ExperimentState> ConstructIntroductionBlock()
    {
        List<Presets.ExperimentState> ExperimentStates = new List<Presets.ExperimentState>
        {

            Presets.ExperimentState.IntroductionInstructionState
        
        };
        return ExperimentStates;
    }

    public static List<Presets.ExperimentState> ConstructPracticeBlock()
    {
        List<Presets.ExperimentState> ExperimentStates = new List<Presets.ExperimentState>
        {
            Presets.ExperimentState.InitState,
            Presets.ExperimentState.CalibrationState,
            Presets.ExperimentState.StartState,
            Presets.ExperimentState.IntroductionInstructionState,

            Presets.ExperimentState.KeyboardDewellTimeIntroductionState,
            Presets.ExperimentState.KeyboardDewellTimeState,
            Presets.ExperimentState.KeyboardClickIntroductionState,
            Presets.ExperimentState.KeyboardClickState,
            Presets.ExperimentState.KeyboardIllumiReadSwypeIntroductionState,
            Presets.ExperimentState.KeyboardIllumiReadSwypeState,
            Presets.ExperimentState.KeyboardFreeSwitchInstructionState,
            Presets.ExperimentState.KeyboardFreeSwitchState,
            Presets.ExperimentState.EndState

        };
        return ExperimentStates;
    }

    public static List<Presets.ExperimentState> ConstructTrainBlock()
    {
        List<Presets.ExperimentState> ExperimentStates = new List<Presets.ExperimentState>
        {

            Presets.ExperimentState.KeyboardIllumiReadSwypeIntroductionState,
            Presets.ExperimentState.KeyboardIllumiReadSwypeState,



        };
        return ExperimentStates;
    }

    public static List<Presets.ExperimentState> ConstructTestBlock()
    {
        List<Presets.ExperimentState> ExperimentStates = new List<Presets.ExperimentState>
        {

            Presets.ExperimentState.KeyboardFreeSwitchInstructionState,
            Presets.ExperimentState.KeyboardFreeSwitchState,

        };
        return ExperimentStates;
    }

    public static List<Presets.ExperimentState> ConstructEndBlock()
    {
        List<Presets.ExperimentState> ExperimentStates = new List<Presets.ExperimentState>
        {

            Presets.ExperimentState.EndState
        
        };
        return ExperimentStates;
    }


}
