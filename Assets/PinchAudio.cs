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

    private ExperimentManager experimentManager;

    public GameObject LeftPaintCursor1;

    public GameObject LeftPaintCursor2;

    public GameObject RightPaintCursor1;

    public GameObject RightPaintCursor2;

    public GameObject LeftPaint;

    public GameObject RightPaint;

    public Leap.Unity.HandsModule.HandBinder LeftHandModel;

    public Leap.Unity.HandsModule.HandBinder RightHandModel;

    public Leap.Unity.PinchDetector pinchDetector;

    

    // Start is called before the first frame update
    void Start()
    {
        swypeDetector = GameObject.Find("GlobalSettings").GetComponent<SwypeDetector>();
        experimentManager = GameObject.Find("ExperimentManager").GetComponent<ExperimentManager>();
        if(experimentManager.chiral.ToString() == "Left")
        {
            LeftPaint.SetActive(true);
            RightPaint.SetActive(false);
            RightPaintCursor1.SetActive(false);
            RightPaintCursor2.SetActive(false);
            pinchDetector.HandModel = LeftHandModel;
        }
        else if(experimentManager.chiral.ToString() == "Right")
        {
            LeftPaint.SetActive(false);
            LeftPaintCursor1.SetActive(false);
            LeftPaintCursor2.SetActive(false);
            RightPaint.SetActive(true);
            pinchDetector.HandModel = RightHandModel;
        }
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
            LeftPaintCursor1.GetComponent<MeshRenderer>().material.color = Color.cyan;
            LeftPaintCursor2.GetComponent<MeshRenderer>().material.color = Color.cyan;
            RightPaintCursor1.GetComponent<MeshRenderer>().material.color = Color.cyan;
            RightPaintCursor2.GetComponent<MeshRenderer>().material.color = Color.cyan;
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
            audioSource.PlayOneShot(pinchExitClip,0.1f);
            LeftPaintCursor1.GetComponent<MeshRenderer>().material.color = Color.white;
            LeftPaintCursor2.GetComponent<MeshRenderer>().material.color = Color.white;
            RightPaintCursor1.GetComponent<MeshRenderer>().material.color = Color.white;
            RightPaintCursor2.GetComponent<MeshRenderer>().material.color = Color.white;
            swypeDetector.cyanKeyColor = false;

        }
        
    }
}
