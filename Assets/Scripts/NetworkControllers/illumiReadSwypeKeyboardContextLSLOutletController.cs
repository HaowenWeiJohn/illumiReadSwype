using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class illumiReadSwypeKeyboardContextLSLOutletController : LSLOutletInterface
{
    void Start()
    {
        InitLSLStreamOutlet(
            Presets.KeyboardContextLSLOutletStreamName,
            Presets.KeyboardContextLSLOutletStreamType,
            Presets.KeyboardContextChannelNum,
            Presets.KeyboardContextNominalSamplingRate,
            LSL.channel_format_t.cf_string
        );


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PushKeyboardContextData(string context)
    {
        string[] KeyboardContextData = CreateEventMarkerArrayString();
        KeyboardContextData[0] = context;
        Debug.Log("Keyboard Context Data: " + KeyboardContextData[0]);

        streamOutlet.push_sample(KeyboardContextData);
    }

}