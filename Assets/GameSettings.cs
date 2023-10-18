using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public Params.InteractionMode interactionMode = Params.InteractionMode.dwellTime;
    public float dwellActivateTime = 1.0f;
    public float distToPlayer = 10f;

    // the audio clip for key press
    public AudioClip keyPressAudioClip;
    public AudioClip keyHoverAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
