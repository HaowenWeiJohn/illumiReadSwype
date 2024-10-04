using LSL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSLOutletInterface : MonoBehaviour
{
    // Start is called before the first frame update
    public StreamOutlet streamOutlet;

    // the string StreamOutlet
    // public StreamOutlet stringStreamOutlet;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void InitLSLStreamOutlet(string streamName, string streamType, int channelNum, float nominalSamplingRate, LSL.channel_format_t channelFormat)
    {
        StreamInfo streamInfo = new StreamInfo(

            streamName,
            streamType,
            channelNum,
            nominalSamplingRate,
            channelFormat

            );

        streamOutlet = new StreamOutlet(streamInfo);
    }

    public float[] CreateEventMarkerArrayFloat()
    {
        int channel_count = streamOutlet.info().channel_count();
        float[] zerosArray = new float[channel_count];
        return zerosArray;
        //return new float[3] { };
    }

    public string[] CreateEventMarkerArrayString()
    {
        int channel_count = streamOutlet.info().channel_count();
        string[] emptyArray = new string[channel_count];
        return emptyArray;
    }

}
