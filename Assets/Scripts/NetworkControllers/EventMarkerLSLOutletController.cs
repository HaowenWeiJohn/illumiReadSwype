using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMarkerLSLOutletController : LSLOutletInterface
{
    // Start is called before the first frame update
    void Start()
    {
        InitLSLStreamOutlet(
            Presets.EventMarkerLSLOutletStreamName,
            Presets.EventMarkerLSLOutletStreamType,
            Presets.EventMarkerChannelNum,
            Presets.EventMarkerNominalSamplingRate,
            LSL.channel_format_t.cf_float32
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    ////////////////////////////////////////send block event marker////////////////////////////////////////////////////////////////////////

    public void sendBlockOnEnterMarker(Presets.ExperimentBlock currentExperimentBlock)
    {
        float[] eventMarkerArray = CreateEventMarkerArrayFloat();
        eventMarkerArray[(int)Presets.EventMarkerChannelInfo.BlockChannelIndex] = (float)currentExperimentBlock;
        streamOutlet.push_sample(eventMarkerArray);

    }

    public void sendBlockOnExitMarker(Presets.ExperimentBlock currentExperimentBlock)
    {

        float[] eventMarkerArray = CreateEventMarkerArrayFloat();
        eventMarkerArray[(int)Presets.EventMarkerChannelInfo.BlockChannelIndex] = (float)currentExperimentBlock * -1.0f;
        streamOutlet.push_sample(eventMarkerArray);
    }

    ////////////////////////////////////////send state event marker////////////////////////////////////////////////////////////////////////
    public void sendStateOnEnterMarker(Presets.ExperimentState currentExperimentState)
    {

        float[] eventMarkerArray = CreateEventMarkerArrayFloat();
        eventMarkerArray[(int)Presets.EventMarkerChannelInfo.ExperimentStateChannelIndex] = (float)currentExperimentState;
        streamOutlet.push_sample(eventMarkerArray);

    }

    // state event marker
    public void sendStateOnExitMarker(Presets.ExperimentState currentExperimentState)
    {

        float[] eventMarkerArray = CreateEventMarkerArrayFloat();
        eventMarkerArray[(int)Presets.EventMarkerChannelInfo.ExperimentStateChannelIndex] = (float)currentExperimentState * -1.0f;
        streamOutlet.push_sample(eventMarkerArray);

    }



    public void sendUserInputsMarker(int UserInputs)
    {

        float[] eventMarkerArray = CreateEventMarkerArrayFloat();
        eventMarkerArray[(int)Presets.EventMarkerChannelInfo.UserInputsChannelIndex] = (float)UserInputs;
        streamOutlet.push_sample(eventMarkerArray);
    }




}
