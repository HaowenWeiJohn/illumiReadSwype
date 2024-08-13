using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchAudio : MonoBehaviour
{

    public AudioSource audioSource;

    public AudioClip pinchEnterClip;

    public AudioClip pinchExitClip;

    public GameManager gameManager;

    private SwypeDetector swypeDetector;

    public GameObject PaintCursor1;

    public GameObject PaintCursor2;

    // Start is called before the first frame update
    void Start()
    {
        swypeDetector = GameObject.Find("GlobalSettings").GetComponent<SwypeDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playPinchEnterClip()
    {
        // when in the swipe state:
        if(gameManager.keyboardIllumiReadSwypeStateController.gameObject.activeSelf)
        {
            audioSource.PlayOneShot(pinchEnterClip);
            PaintCursor1.GetComponent<MeshRenderer>().material.color = Color.cyan;
            PaintCursor2.GetComponent<MeshRenderer>().material.color = Color.cyan;
            swypeDetector.cyanKeyColor = true;
        }
        else if(gameManager.KeyboardClickStateController.gameObject.activeSelf)
        {
            audioSource.PlayOneShot(pinchEnterClip);
        }
        
    }

    public void playPinchExitClip()
    {
        if(gameManager.keyboardIllumiReadSwypeStateController.gameObject.activeSelf)
        {
            audioSource.PlayOneShot(pinchExitClip,0.2f);
            PaintCursor1.GetComponent<MeshRenderer>().material.color = Color.white;
            PaintCursor2.GetComponent<MeshRenderer>().material.color = Color.white;
            swypeDetector.cyanKeyColor = false;

        }
        
    }
}
