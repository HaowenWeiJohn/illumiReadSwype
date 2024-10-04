using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class illumiReadSwypeUserInputLSLOutletController : LSLOutletInterface
{
    // Start is called before the first frame update
    void Start()
    {
        InitLSLStreamOutlet(
            Presets.GazeOnKeyboardLSLOutletStreamName,
            Presets.GazeOnKeyboardLSLOutletStreamType,
            Presets.GazeOnKeyboardChannelNum,
            Presets.GazeOnKeyboardNominalSamplingRate,
            LSL.channel_format_t.cf_float32
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PushVarjoGazeOnKeyboardData(bool gazeHitKeyboardBackground, Vector3 keyboardBackgroundHitPointLocal, bool gazeHitKey, Vector3 keyHitPointLocal, int keyHitIndex, bool UserInputButton1, bool UserInputButton2)
    {
        // varjo gaze data is a 39-dim vector
        float[] varjoGazeOnKeyboardData = CreateEventMarkerArrayFloat();
        varjoGazeOnKeyboardData[0] = gazeHitKeyboardBackground ? 1.0f : 0.0f;
        varjoGazeOnKeyboardData[1] = keyboardBackgroundHitPointLocal.x;
        varjoGazeOnKeyboardData[2] = keyboardBackgroundHitPointLocal.y;
        varjoGazeOnKeyboardData[3] = keyboardBackgroundHitPointLocal.z;
        varjoGazeOnKeyboardData[4] = gazeHitKey ? 1.0f : 0.0f;
        varjoGazeOnKeyboardData[5] = keyHitPointLocal.x;
        varjoGazeOnKeyboardData[6] = keyHitPointLocal.y;
        varjoGazeOnKeyboardData[7] = keyHitPointLocal.z;
        varjoGazeOnKeyboardData[8] = (float)keyHitIndex;

        varjoGazeOnKeyboardData[9] = UserInputButton1 ? 1.0f : 0.0f;
        varjoGazeOnKeyboardData[10] = UserInputButton2 ? 1.0f : 0.0f;

        streamOutlet.push_sample(varjoGazeOnKeyboardData);
    }
    

}
