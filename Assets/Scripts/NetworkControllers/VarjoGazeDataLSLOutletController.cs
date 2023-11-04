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
            LSL.channel_format_t.cf_float32
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
