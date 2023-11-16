using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VarjoGazeDataLSLOutletController : LSLOutletInterface
{
    // Start is called before the first frame update
    void Start()
    {
        initLSLStreamOutlet(
            Presets.GazeDataLSLOutletStreamName,
            Presets.GazeDataLSLOutletStreamType,
            Presets.GazeDataChannelNum,
            Presets.GazeDataNominalSamplingRate,
            LSL.channel_format_t.cf_double64
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pushVarjoGazeData(double[] varjoGazeData)
    {
        // varjo gaze data is a 39-dim vector
        streamOutlet.push_sample(varjoGazeData);
    }



}
