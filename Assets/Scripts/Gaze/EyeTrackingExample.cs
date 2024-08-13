using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using Varjo.XR;
using LSL;
using Keyboard;
using Leap.Unity;
using TMPro;
using Cysharp.Net.Http;
using Grpc.Core;
using Grpc.Net.Client;
using UnityEngine.UI;

public enum GazeDataSource
{
    InputSubsystem,
    GazeAPI
}

public class EyeTrackingExample : MonoBehaviour
{
    [Header("Game Manager")]
    public GameManager gameManager;

    [Header("Gaze data")]
    public GazeDataSource gazeDataSource = GazeDataSource.InputSubsystem;

    [Header("Gaze calibration settings")]
    public VarjoEyeTracking.GazeCalibrationMode gazeCalibrationMode = VarjoEyeTracking.GazeCalibrationMode.Fast;
    public KeyCode calibrationRequestKey = KeyCode.Space;

    [Header("Gaze output filter settings")]
    public VarjoEyeTracking.GazeOutputFilterType gazeOutputFilterType = VarjoEyeTracking.GazeOutputFilterType.Standard;
    public KeyCode setOutputFilterTypeKey = KeyCode.RightShift;

    [Header("Gaze data output frequency")]
    public VarjoEyeTracking.GazeOutputFrequency frequency;

    [Header("Toggle gaze target visibility")]
    public KeyCode toggleGazeTarget = KeyCode.Return;

    [Header("Debug Gaze")]
    public KeyCode checkGazeAllowed = KeyCode.PageUp;
    public KeyCode checkGazeCalibrated = KeyCode.PageDown;

    [Header("Toggle fixation point indicator visibility")]
    public bool showFixationPoint = true;

    [Header("Visualization Transforms")]
    public Transform fixationPointTransform;
    public Transform leftEyeTransform;
    public Transform rightEyeTransform;

    [Header("XR camera")]
    public Camera xrCamera;

    [Header("Gaze point indicator")]
    public GameObject gazeTarget;

    [Header("Gaze trace particle system")]
    public ParticleSystem gazeParticle;

    public ParticleSystem gazeDot;

    [Header("Gaze ray radius")]
    public float gazeRadius = 0.01f;

    [Header("Gaze point distance if not hit anything")]
    public float floatingGazeTargetDistance = 5f;

    [Header("Gaze target offset towards viewer")]
    public float targetOffset = 0.2f;

    [Header("Amout of force give to freerotating objects at point where user is looking")]
    public float hitForce = 5f;

    [Header("Gaze data logging")]
    public KeyCode loggingToggleKey = KeyCode.RightControl;

    [Header("Default path is Logs under application data path.")]
    public bool useCustomLogPath = false;
    public string customLogPath = "";

    [Header("Print gaze data framerate while logging.")]
    public bool printFramerate = false;


    [Header("Lab Streaming Layer (LSL)")]
    public bool streamGazeData = true;
    public VarjoGazeDataLSLOutletController varjoGazeDataLSLOutletController;
    public illumiReadSwypeUserInputLSLOutletController varjoGazeOnKeyboardLSLOutletController;

    [Header("Print gaze data timestamp")]
    public bool printGazeDataTimestamp = false;

    [Header("KeyboardManager")]
    public KeyboardManager keyboardManager;

    [Header("Keyboard")]
    public GameObject keyboard;
    public Key gazeKey = null;
    public bool gazeHitOnKey = false;
    public Vector3 keyHitPointLocal = Vector3.zero;
    public bool gazeHitKeyboardBackground = false;
    public Vector3 keyboardBackgroundHitPointLocal = Vector3.zero;

    public Vector2 keyBoardHitLocal = Vector2.zero;

    



    [Header("Gesture")]
    [SerializeField] PinchDetector pinchDetector;

    [Header("RPC Client Manager")]
    public string host = "http://localhost:13004";

    private GrpcChannel channel;
    private YetAnotherHttpHandler handler;
    private IllumiReadSwypeScript.IllumiReadSwypeScriptClient client;

    private string TapKeyLetter = "";

    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;
    private Eyes eyes;
    private VarjoEyeTracking.GazeData gazeData;
    private List<VarjoEyeTracking.GazeData> dataSinceLastUpdate;
    private List<VarjoEyeTracking.EyeMeasurements> eyeMeasurementsSinceLastUpdate;
    private Vector3 leftEyePosition;
    private Vector3 rightEyePosition;
    private Quaternion leftEyeRotation;
    private Quaternion rightEyeRotation;
    private Vector3 fixationPoint;
    private Vector3 direction;
    private Vector3 rayOrigin;
    //private RaycastHit hit;
    RaycastHit[] hits;
    


    private float distance;
    private StreamWriter writer = null;
    private bool logging = false;

    private static readonly string[] ColumnNames = { "Frame", "CaptureTime", "LogTime", "HMDPosition", "HMDRotation", "GazeStatus", "CombinedGazeForward", "CombinedGazePosition", "InterPupillaryDistanceInMM", "LeftEyeStatus", "LeftEyeForward", "LeftEyePosition", "LeftPupilIrisDiameterRatio", "LeftPupilDiameterInMM", "LeftIrisDiameterInMM", "RightEyeStatus", "RightEyeForward", "RightEyePosition", "RightPupilIrisDiameterRatio", "RightPupilDiameterInMM", "RightIrisDiameterInMM", "FocusDistance", "FocusStability" };
    private const string ValidString = "VALID";
    private const string InvalidString = "INVALID";

    int gazeDataCount = 0;
    float gazeTimer = 0f;

    private StreamWriter GazeWriter = null;
    private string filePath;

//  gaze click and swype detector
    private GazeClickDetector gazeClickDetector;

    private SwypeDetector swypeDetector;

    private List<GameObject> gazeKeyTargets = new List<GameObject>();

    public bool isSwyping = false;

    private bool pinchHandled = false;

    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(XRNode.CenterEye, devices);
        device = devices.FirstOrDefault();
    }

    void OpenCSVFile()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

        GazeWriter = new StreamWriter(filePath, false);

        GazeWriter.WriteLine("Key,KeyID,HitPointLocalX,HitPointLocalY,KeyGroundTruthLocalX,KeyGroundTruthLocalY");
    }

    void WriteToCSV(string key, string keyID, Vector2 hitPointLocal, Vector2 GroudTruthLocal)
    {
        if (GazeWriter != null)
        {
            // string hitPointString = "(" + hitPointLocal.x.ToString() + " " + hitPointLocal.y.ToString() + ")";
            // string GroudTruthString = "(" + GroudTruthLocal.x.ToString() + " " + GroudTruthLocal.y.ToString() + ")";
            string line = $"{key},{keyID},{hitPointLocal.x.ToString()},{hitPointLocal.y.ToString()},{GroudTruthLocal.x.ToString()},{GroudTruthLocal.y.ToString()}";
            // string line = $"{key},{keyID},{hitPointString},{GroudTruthString}";
            GazeWriter.WriteLine(line);
        }
    }

    void CloseCSVFile()
    {
        if(GazeWriter != null)
        {
            GazeWriter.Close();
            GazeWriter = null;
        }
    }

    // the corountine to receive the response from the server
    private IEnumerator RPCTapToChar(Vector2 localHitPosition)
    {
        float x = localHitPosition.x;
        float y = localHitPosition.y;

        // Create a new request
        var request = new Tap2CharRPCRequest(){Input0= x, Input1= y};
        var call = client.Tap2CharRPCAsync(request);
        yield return new WaitUntil(() => call.ResponseAsync.IsCompleted);

        if(call.ResponseAsync.IsCompletedSuccessfully)
        {
            var response = call.ResponseAsync.Result;
            // Debug.Log("The Tap to char prediction result" + response);
            TapKeyLetter =response.Message;

            // if(keyboardManager.shiftActive || keyboardManager.capsLockActive)
            // {
            //     TapKeyLetter = TapKeyLetter.ToUpper();
            // }
            // else
            // {
            //     TapKeyLetter = TapKeyLetter.ToLower();
            // }

            // invoke the button click event on keyboard
            // Find a GameObject with a specific name in the scene
            GameObject targetObject = GameObject.Find(TapKeyLetter);
            // Debug.Log("The GameObject name is: " + TapKeyLetter);

            if (targetObject != null)
            {
                Keyboard.LetterKey letterKey = targetObject.GetComponent<LetterKey>();
                // set the key value for data collector
                gazeClickDetector.keyValue = letterKey.character;
                swypeDetector.keyValue = letterKey.character;

                letterKey.InvokeButtonOnClick();
                letterKey.PlayKeyEnterAudioClip();

            }
            else
            {
                // do nothing, leave the key to key call back
                
                // Debug.LogError("GameObject with the specified name not found!");
            }
            
        }
        else
        {
            Debug.LogError("gRPC call failed: " + call.ResponseAsync.Exception);
        }
    }


    void OnEnable()
    {
        if (!device.isValid)
        {
            GetDevice();
        }
    }

    private void Start()
    {
        // // define the file path to store the csv file
        // filePath = Path.Combine(Application.dataPath + "/Logs/GazeLog/", "GazeData.csv");
        // OpenCSVFile();


        VarjoEyeTracking.SetGazeOutputFrequency(frequency);
        //Hiding the gazetarget if gaze is not available or if the gaze calibration is not done
        if (VarjoEyeTracking.IsGazeAllowed() && VarjoEyeTracking.IsGazeCalibrated())
        {
            gazeTarget.SetActive(true);
        }
        else
        {
            gazeTarget.SetActive(false);
        }

        if (showFixationPoint)
        {
            fixationPointTransform.gameObject.SetActive(true);
        }
        else
        {
            fixationPointTransform.gameObject.SetActive(false);
        }

        // set up the RPC client
        handler = new YetAnotherHttpHandler() { Http2Only = true };  // GRPC requires HTTP/2
        channel = GrpcChannel.ForAddress(host, new GrpcChannelOptions() { HttpHandler = handler, Credentials = ChannelCredentials.Insecure });
        client = new IllumiReadSwypeScript.IllumiReadSwypeScriptClient(channel);

        gazeParticle.Play();

        gazeClickDetector = GameObject.Find("GlobalSettings").GetComponent<GazeClickDetector>();
        swypeDetector = GameObject.Find("GlobalSettings").GetComponent<SwypeDetector>();
    }

    void Update()
    {
        // set the state of the action detectors
        // gazeClickDetector.keyPressed = false;
        // swypeDetector.keyPressed = false;

        if (logging && printFramerate)
        {
            gazeTimer += Time.deltaTime;
            if (gazeTimer >= 1.0f)
            {
                Debug.Log("Gaze data rows per second: " + gazeDataCount);
                gazeDataCount = 0;
                gazeTimer = 0f;
            }
        }

        // Request gaze calibration
        if (Input.GetKeyDown(calibrationRequestKey))
        {
            VarjoEyeTracking.RequestGazeCalibration(gazeCalibrationMode);
        }

        // Set output filter type
        if (Input.GetKeyDown(setOutputFilterTypeKey))
        {
            VarjoEyeTracking.SetGazeOutputFilterType(gazeOutputFilterType);
            Debug.Log("Gaze output filter type is now: " + VarjoEyeTracking.GetGazeOutputFilterType());
        }

        // Check if gaze is allowed
        if (Input.GetKeyDown(checkGazeAllowed))
        {
            Debug.Log("Gaze allowed: " + VarjoEyeTracking.IsGazeAllowed());
        }

        // Check if gaze is calibrated
        if (Input.GetKeyDown(checkGazeCalibrated))
        {
            Debug.Log("Gaze calibrated: " + VarjoEyeTracking.IsGazeCalibrated());
        }

        // just return if the keyboard is not in states that requires gaze tracking
        if(!gameManager.keyboardIllumiReadSwypeStateController.gameObject.activeSelf && !gameManager.KeyboardClickStateController.gameObject.activeSelf)
        {
            gazeDot.Stop();
            return;
        }

        // Toggle gaze target visibility
        // if (Input.GetKeyDown(toggleGazeTarget))
        // {
        //     gazeTarget.GetComponentInChildren<MeshRenderer>().enabled = !gazeTarget.GetComponentInChildren<MeshRenderer>().enabled;
        // }

        // Get gaze data if gaze is allowed and calibrated
        if (VarjoEyeTracking.IsGazeAllowed() && VarjoEyeTracking.IsGazeCalibrated())
        {
            //Get device if not valid
            if (!device.isValid)
            {
                GetDevice();
            }

            // Show gaze target
            gazeTarget.SetActive(true);

            if (gazeDataSource == GazeDataSource.InputSubsystem)
            {
                // Get data for eye positions, rotations and the fixation point
                if (device.TryGetFeatureValue(CommonUsages.eyesData, out eyes))
                {
                    if (eyes.TryGetLeftEyePosition(out leftEyePosition))
                    {
                        leftEyeTransform.localPosition = leftEyePosition;
                    }

                    if (eyes.TryGetLeftEyeRotation(out leftEyeRotation))
                    {
                        leftEyeTransform.localRotation = leftEyeRotation;
                    }

                    if (eyes.TryGetRightEyePosition(out rightEyePosition))
                    {
                        rightEyeTransform.localPosition = rightEyePosition;
                    }

                    if (eyes.TryGetRightEyeRotation(out rightEyeRotation))
                    {
                        rightEyeTransform.localRotation = rightEyeRotation;
                    }

                    if (eyes.TryGetFixationPoint(out fixationPoint))
                    {
                        fixationPointTransform.localPosition = fixationPoint;
                    }
                }

                // Set raycast origin point to VR camera position
                rayOrigin = xrCamera.transform.position;

                // Direction from VR camera towards fixation point
                direction = (fixationPointTransform.position - xrCamera.transform.position).normalized;

            }
            else
            {
                gazeData = VarjoEyeTracking.GetGaze();

                if (gazeData.status != VarjoEyeTracking.GazeStatus.Invalid)
                {
                    // GazeRay vectors are relative to the HMD pose so they need to be transformed to world space
                    if (gazeData.leftStatus != VarjoEyeTracking.GazeEyeStatus.Invalid)
                    {
                        leftEyeTransform.position = xrCamera.transform.TransformPoint(gazeData.left.origin);
                        leftEyeTransform.rotation = Quaternion.LookRotation(xrCamera.transform.TransformDirection(gazeData.left.forward));
                    }

                    if (gazeData.rightStatus != VarjoEyeTracking.GazeEyeStatus.Invalid)
                    {
                        rightEyeTransform.position = xrCamera.transform.TransformPoint(gazeData.right.origin);
                        rightEyeTransform.rotation = Quaternion.LookRotation(xrCamera.transform.TransformDirection(gazeData.right.forward));
                    }

                    // Set gaze origin as raycast origin
                    rayOrigin = xrCamera.transform.TransformPoint(gazeData.gaze.origin);

                    // Set gaze direction as raycast direction
                    direction = xrCamera.transform.TransformDirection(gazeData.gaze.forward);

                    // Fixation point can be calculated using ray origin, direction and focus distance
                    fixationPointTransform.position = rayOrigin + direction * gazeData.focusDistance;
                }
            }
        }



        hits = Physics.RaycastAll(rayOrigin, direction, 100.0F);
        // Debug.Log("Hits: " + hits.Length);
        // Debug.Log("Ray Origin: " + rayOrigin.ToString());
        // Debug.Log("Direction: " + direction.ToString());
        gazeHitOnKey = false;
        keyHitPointLocal = Vector3.zero;
        gazeHitKeyboardBackground = false;
        keyboardBackgroundHitPointLocal = Vector3.one;

        if (hits.Length > 0)
        {
            
            // gaze target
            RaycastHit first_hit = hits[0];
            gazeTarget.transform.position = first_hit.point - direction * targetOffset;
            gazeTarget.transform.LookAt(rayOrigin, Vector3.up);
            distance = first_hit.distance;
            gazeTarget.transform.localScale = Vector3.one * distance;



            RotateWithGaze rotateWithGaze = first_hit.collider.gameObject.GetComponent<RotateWithGaze>();
            if (rotateWithGaze != null)
            {
                rotateWithGaze.RayHit();
            }



            gazeKey = null;
            
            foreach (RaycastHit hit in hits)
            {
                Vector3 hitPointLocal = hit.collider.gameObject.transform.InverseTransformPoint(hit.point);

                // include scaling
                hitPointLocal.x = hitPointLocal.x * hit.collider.gameObject.transform.localScale.x;
                hitPointLocal.y = hitPointLocal.y * hit.collider.gameObject.transform.localScale.y;
                hitPointLocal.z = hitPointLocal.z * hit.collider.gameObject.transform.localScale.z;


                // three types of keys: Key, Letter Key, Suggestion Key
                if (hit.collider.gameObject.tag == KeyParams.KeyTag)
                {
                    // hit a regular function key
                    gazeKey = hit.collider.gameObject.GetComponent<Key>();
                    gazeKey.HasGaze();
                    gazeHitOnKey = true;
                    keyHitPointLocal = hitPointLocal;
                }
                else if(hit.collider.gameObject.tag == KeyParams.LetterKeyTag)
                {
                    gazeKey = hit.collider.gameObject.GetComponent<LetterKey>();
                    gazeKey.HasGaze();
                    gazeHitOnKey = true;
                    keyHitPointLocal = hitPointLocal;
                }
                else if (hit.collider.gameObject.tag == KeyParams.SuggestionKeyTag)
                {
                    gazeKey = hit.collider.gameObject.GetComponent<SuggestionKey>();
                    gazeKey.HasGaze();
                    gazeHitOnKey = true;
                    keyHitPointLocal = hitPointLocal;
                }
                else
                {
                    // do nothing 
                }

                Vector3 hitPointKeyboardLocal = keyboard.transform.InverseTransformPoint(hit.point);

                Vector2 hitPointKeyboardLocal2D = new Vector2(hitPointKeyboardLocal.x, hitPointKeyboardLocal.y);

                Vector3 KeyLocalOffset = keyboard.transform.InverseTransformPoint(hit.collider.gameObject.transform.position);

                Vector2 KeyLocalOffset2D = new Vector2(KeyLocalOffset.x, KeyLocalOffset.y);

                // only the keyPress state is important
                gazeClickDetector.tapPosition = hitPointKeyboardLocal2D;
                swypeDetector.tapPosition = hitPointKeyboardLocal2D;

                // call the fat finger algorithm here
                if(hit.collider.gameObject.layer == LayerMask.NameToLayer("TapToChar") && gameManager.KeyboardClickStateController.gameObject.activeSelf && pinchDetector.DidStartPinch && gazeClickDetector.keyPressed == false)
                {
                    gazeClickDetector.keyPressed = true;
                    StartCoroutine(RPCTapToChar(hitPointKeyboardLocal2D));
                }

                if(pinchDetector.DidEndPinch && gameManager.KeyboardClickStateController.gameObject.activeSelf)
                {
                    gazeClickDetector.keyPressed = false;
                }

                bool swypeState = gameManager.keyboardIllumiReadSwypeStateController.gameObject.activeSelf;

                if(pinchDetector.DidStartPinch == true && swypeState)
                {   
                    // PaintCursor1.GetComponent<MeshRenderer>().material.color = Color.cyan;
                    // PaintCursor2.GetComponent<MeshRenderer>().material.color = Color.cyan;
                    // swypeDetector.cyanKeyColor = true;
                    // the bool controls the swype state and LSL receiver
                    isSwyping = false;
                    // if(hit.collider.gameObject.layer == LayerMask.NameToLayer("TapToChar"))
                    // {
                    //     pinchHandled = false;
                    // }
                    // else
                    // {
                    //     pinchHandled = true;
                    // }
                    pinchHandled = false;

                    gazeDot.Stop();
                    // clear the gaze key targets
                    gazeKeyTargets = new List<GameObject>();
                    // gazeDot.Play();
                    gazeParticle.Play();
                    // gazeParticle.Stop();
                    EmitGazeParticle(hit.point);
                    swypeDetector.keyPressed = false;
                    swypeDetector.keyPinched = false;

                    
                }
                else if (pinchDetector.IsPinching == true &&swypeState)
                {
                    // add the hit collider object to the list if not in
                    if(hit.collider.gameObject.tag == KeyParams.LetterKeyTag && gazeKeyTargets.Contains(hit.collider.gameObject) == false)
                    {
                        gazeKeyTargets.Add(hit.collider.gameObject);
                    }

                    if(gazeKeyTargets.Count>1 && isSwyping == false)
                    {
                        isSwyping = true;
                        swypeDetector.keyPressed = true;
                        swypeDetector.keyPinched = false;
                    }
                    else if(gazeKeyTargets.Count>1 && isSwyping == true)
                    {
                        swypeDetector.keyPressed = true;
                        swypeDetector.keyPinched = false;
                    }
                    else if(gazeKeyTargets.Count<=1)
                    {
                        swypeDetector.keyPressed = false;
                        swypeDetector.keyPinched = false;
                    }

                    EmitGazeParticle(hit.point);
                }
                else if(pinchDetector.DidEndPinch == true && swypeState)
                {
                    gazeDot.Play();
                    gazeParticle.Stop();
                    // swypeDetector.keyPressed = false;

                    if(isSwyping == false && pinchHandled == false && hit.collider.gameObject.layer == LayerMask.NameToLayer("TapToChar"))
                    {
                        swypeDetector.keyPressed = false;
                        swypeDetector.keyPinched = true;
                        StartCoroutine(RPCTapToChar(hitPointKeyboardLocal2D));
                        pinchHandled = true;
                    }
                    else if(isSwyping == true)
                    {
                        swypeDetector.keyPressed = false;
                        swypeDetector.keyPinched = false;

                    }

                }
                else
                {
                    gazeDot.Play();
                    gazeDot.transform.position = hit.point;
                    swypeDetector.keyPressed = false;
                    swypeDetector.keyPinched = false;
                }


                // particle system effect for gaze
                // if(pinchDetector.DidStartPinch == true && gameManager.keyboardIllumiReadSwypeStateController.gameObject.activeSelf && gazeKeyTargets.Count <=1)
                // {
                //     gazeDot.Stop();
                //     gazeParticle.Play();
                //     EmitGazeParticle(hit.point);
                //     swypeDetector.keyPressed = true;
                // }
                // else if(pinchDetector.IsPinching == true && gameManager.keyboardIllumiReadSwypeStateController.gameObject.activeSelf )
                // {
                //     EmitGazeParticle(hit.point);
                //     swypeDetector.keyPressed = true;
                // }
                // else if(pinchDetector.DidEndPinch == true && gameManager.keyboardIllumiReadSwypeStateController.gameObject.activeSelf)
                // {
                //     gazeDot.Play();
                //     gazeParticle.Stop();
                //     swypeDetector.keyPressed = false;
                // }
                // else
                // {
                //     gazeDot.Play();
                //     gazeDot.transform.position = hit.point;
                //     swypeDetector.keyPressed = false;
                // }


                // only check for the letter keys
                if(hit.collider.gameObject.GetComponent<LetterKey>() != null)
                {
                    if(gazeKey != null)
                    {
                        // string key = hit.collider.gameObject.GetComponent<LetterKey>().character;
                        // string keyID = KeyParams.KeysID[gazeKey.key].ToString();

                        // WriteToCSV(key,keyID, hitPointKeyboardLocal2D, KeyLocalOffset2D);

                        // keyBoardHitLocal = hitPointKeyboardLocal2D;
                        keyboardBackgroundHitPointLocal.x = hitPointKeyboardLocal.x;
                        keyboardBackgroundHitPointLocal.y = hitPointKeyboardLocal.y;
                    }
                }

                

                // Debug.Log("Key Value: "+ gazeKey.key + " Key ID: " + KeyParams.KeysID[gazeKey.key]);
                // Debug.Log("Hit point local: " + hitPointKeyboardLocal.ToString());



                //if (hit.collider.gameObject.tag == KeyParams.KeyTag)
                //{
                //    gazeKey = hit.collider.gameObject.GetComponent<KeyController>();
                //    gazeKey.HasGaze();
                //    gazeHitOnKey = true;
                //    keyHitPointLocal = hitPointLocal;

                //}
                //else if(hit.collider.gameObject.tag == KeyParams.KeyboardSuggestionStrip)
                //{
                //    SuggestionStripController suggestionStripController = hit.collider.gameObject.GetComponent<SuggestionStripController>();
                //    suggestionStripController.HasGaze();

                //}
                //else if(hit.collider.gameObject.tag == KeyParams.KeyboardBackgroundTag)
                //{
                //    // get coordinate of the hit point on the keyboarrd
                //    gazeHitKeyboardBackground = true;
                //    keyboardBackgroundHitPointLocal = hitPointLocal;
                //}
                //else
                //{
                //    // do nothing
                //}
            }

        }
        else
        {
            // If gaze ray didn't hit anything, the gaze target is shown at fixed distance
            gazeTarget.transform.position = rayOrigin + direction * floatingGazeTargetDistance;
            gazeTarget.transform.LookAt(rayOrigin, Vector3.up);
            gazeTarget.transform.localScale = Vector3.one * floatingGazeTargetDistance;

            gazeDot.Stop();
        }
        


        bool UserInputButton1 = false;

        if (Input.GetKey(Presets.UserInputButton1) || pinchDetector.DidStartPinch)
        {
            UserInputButton1 = true;
        }


        bool UserInputButton2 = false;

        if (Input.GetKey(Presets.UserInputButton2) || pinchDetector.IsPinching)
        {
            UserInputButton2 = true;
        }


        varjoGazeOnKeyboardLSLOutletController.PushVarjoGazeOnKeyboardData(
            gazeHitKeyboardBackground,
            keyboardBackgroundHitPointLocal,
            gazeHitOnKey,
            keyHitPointLocal,
            gazeKey == null ? KeyParams.KeysID[KeyParams.Keys.None] : KeyParams.KeysID[gazeKey.key],
            UserInputButton1,
            UserInputButton2
        );

        
        //// Raycast to world from VR Camera position towards fixation point
        //if (Physics.SphereCast(rayOrigin, gazeRadius, direction, out hit))
        //{
        //    // Put target on gaze raycast position with offset towards user
        //    gazeTarget.transform.position = hit.point - direction * targetOffset;

        //    // Make gaze target point towards user
        //    gazeTarget.transform.LookAt(rayOrigin, Vector3.up);

        //    // Scale gazetarget with distance so it apperas to be always same size
        //    distance = hit.distance;
        //    gazeTarget.transform.localScale = Vector3.one * distance;

        //    // Prefer layers or tags to identify looked objects in your application
        //    // This is done here using GetComponent for the sake of clarity as an example
        //    RotateWithGaze rotateWithGaze = hit.collider.gameObject.GetComponent<RotateWithGaze>();
        //    if (rotateWithGaze != null)
        //    {
        //        rotateWithGaze.RayHit();
        //    }


        //    // check the hit object type
        //    KeyController keyController = hit.collider.gameObject.GetComponent<KeyController>();
        //    if (keyController != null)
        //    {
        //        // hit on key
        //        //keyController.setGaze();
        //        //Vector3 pointOnKey = keyController.transform.InverseTransformDirection(hit.point);
        //        keyController.HasGaze();
        //        keyController.keyboardController.SetGazeKey(keyController.key);


        //    }
        //    else
        //    {
        //        keyController.keyboardController.ClearGazeKey(); // there is problem with this line

        //    }

        //    //// Alternative way to check if you hit object with tag
        //    //if (hit.transform.CompareTag("FreeRotating"))
        //    //{
        //    //    AddForceAtHitPosition();
        //    //}
        //}
        //else
        //{
        //    // If gaze ray didn't hit anything, the gaze target is shown at fixed distance
        //    gazeTarget.transform.position = rayOrigin + direction * floatingGazeTargetDistance;
        //    gazeTarget.transform.LookAt(rayOrigin, Vector3.up);
        //    gazeTarget.transform.localScale = Vector3.one * floatingGazeTargetDistance;
        //}

        if (Input.GetKeyDown(loggingToggleKey))
        {
            if (!logging)
            {
                StartLogging();
            }
            else
            {
                StopLogging();
            }
            return;
        }

        if (logging)
        {
            int dataCount = VarjoEyeTracking.GetGazeList(out dataSinceLastUpdate, out eyeMeasurementsSinceLastUpdate);
            if (printFramerate) gazeDataCount += dataCount;
            for (int i = 0; i < dataCount; i++)
            {
                LogGazeData(dataSinceLastUpdate[i], eyeMeasurementsSinceLastUpdate[i]);
            }
        }


        if (streamGazeData)
        {
            int dataCount = VarjoEyeTracking.GetGazeList(out dataSinceLastUpdate, out eyeMeasurementsSinceLastUpdate);

            for (int i = 0; i < dataCount; i++)
            {
                double[] varjoGazeData = new double[39];

                //Gaze data frame number
                varjoGazeData[0] = dataSinceLastUpdate[i].frameNumber;

                //Gaze data capture time (nanoseconds)
                varjoGazeData[1] = dataSinceLastUpdate[i].captureTime;

                //Log time (nanoseconds)
                varjoGazeData[2] = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                

                //HMD
                varjoGazeData[3] = xrCamera.transform.localPosition.x;
                varjoGazeData[4] = xrCamera.transform.localPosition.y;
                varjoGazeData[5] = xrCamera.transform.localPosition.z;

                varjoGazeData[6] = xrCamera.transform.localRotation.x;
                varjoGazeData[7] = xrCamera.transform.localRotation.y;
                varjoGazeData[8] = xrCamera.transform.localRotation.z;

                //Combined gaze
                bool invalid = dataSinceLastUpdate[i].status == VarjoEyeTracking.GazeStatus.Invalid;
                varjoGazeData[9] = invalid ? 0 : 1;
                varjoGazeData[10] = invalid ? 0 : dataSinceLastUpdate[i].gaze.forward.x;
                varjoGazeData[11] = invalid ? 0 : dataSinceLastUpdate[i].gaze.forward.y;
                varjoGazeData[12] = invalid ? 0 : dataSinceLastUpdate[i].gaze.forward.z;

                varjoGazeData[13] = invalid ? 0 : dataSinceLastUpdate[i].gaze.origin.x;
                varjoGazeData[14] = invalid ? 0 : dataSinceLastUpdate[i].gaze.origin.y;
                varjoGazeData[15] = invalid ? 0 : dataSinceLastUpdate[i].gaze.origin.z;

                //IDP
                varjoGazeData[16] = invalid ? 0 : eyeMeasurementsSinceLastUpdate[i].interPupillaryDistanceInMM;

                //Left eye
                bool leftInvalid = dataSinceLastUpdate[i].leftStatus == VarjoEyeTracking.GazeEyeStatus.Invalid;
                varjoGazeData[17] = leftInvalid ? 0 : 1;
                varjoGazeData[18] = leftInvalid ? 0 : dataSinceLastUpdate[i].left.forward.x;
                varjoGazeData[19] = leftInvalid ? 0 : dataSinceLastUpdate[i].left.forward.y;
                varjoGazeData[20] = leftInvalid ? 0 : dataSinceLastUpdate[i].left.forward.z;

                varjoGazeData[21] = leftInvalid ? 0 : dataSinceLastUpdate[i].left.origin.x;
                varjoGazeData[22] = leftInvalid ? 0 : dataSinceLastUpdate[i].left.origin.y;
                varjoGazeData[23] = leftInvalid ? 0 : dataSinceLastUpdate[i].left.origin.z;

                varjoGazeData[24] = leftInvalid ? 0 : eyeMeasurementsSinceLastUpdate[i].leftPupilIrisDiameterRatio;
                varjoGazeData[25] = leftInvalid ? 0 : eyeMeasurementsSinceLastUpdate[i].leftPupilDiameterInMM;
                varjoGazeData[26] = leftInvalid ? 0 : eyeMeasurementsSinceLastUpdate[i].leftIrisDiameterInMM;

                //Right eye
                bool rightInvalid = dataSinceLastUpdate[i].rightStatus == VarjoEyeTracking.GazeEyeStatus.Invalid;
                varjoGazeData[27] = rightInvalid ? 0 : 1;
                varjoGazeData[28] = rightInvalid ? 0 : dataSinceLastUpdate[i].right.forward.x;
                varjoGazeData[29] = rightInvalid ? 0 : dataSinceLastUpdate[i].right.forward.y;
                varjoGazeData[30] = rightInvalid ? 0 : dataSinceLastUpdate[i].right.forward.z;

                varjoGazeData[31] = rightInvalid ? 0 : dataSinceLastUpdate[i].right.origin.x;
                varjoGazeData[32] = rightInvalid ? 0 : dataSinceLastUpdate[i].right.origin.y;
                varjoGazeData[33] = rightInvalid ? 0 : dataSinceLastUpdate[i].right.origin.z;

                varjoGazeData[34] = rightInvalid ? 0 : eyeMeasurementsSinceLastUpdate[i].rightPupilIrisDiameterRatio;
                varjoGazeData[35] = rightInvalid ? 0 : eyeMeasurementsSinceLastUpdate[i].rightPupilDiameterInMM;
                varjoGazeData[36] = rightInvalid ? 0 : eyeMeasurementsSinceLastUpdate[i].rightIrisDiameterInMM;


                //Focus
                varjoGazeData[37] = invalid ? 0 : dataSinceLastUpdate[i].focusDistance;
                varjoGazeData[38] = invalid ? 0 : dataSinceLastUpdate[i].focusStability;



                long timeDifference = VarjoTime.GetVarjoTimestamp() - dataSinceLastUpdate[i].captureTime;
                double lsl_local_clock = LSL.LSL.local_clock();
                double timestamp_in_lsl_local_clock = lsl_local_clock - (double)timeDifference / 1000000000.0;

                varjoGazeData[1] = timestamp_in_lsl_local_clock;
                
                if(printGazeDataTimestamp)
                    Debug.Log(timestamp_in_lsl_local_clock);

                varjoGazeDataLSLOutletController.pushVarjoGazeData(varjoGazeData, timestamp_in_lsl_local_clock);
                



              }

        }



    }

    //void AddForceAtHitPosition()
    //{
    //    //Get Rigidbody form hit object and add force on hit position
    //    Rigidbody rb = hit.rigidbody;
    //    if (rb != null)
    //    {
    //        rb.AddForceAtPosition(direction * hitForce, hit.point, ForceMode.Force);
    //    }
    //}

    public void EmitGazeParticle(Vector3 position)
    {
        // ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
        // {
        //     position = position,
        //     applyShapeToPosition = true,
        //     startLifetime = 1.0f
        // };
        gazeParticle.transform.position = position;
        
        
    }

    void LogGazeData(VarjoEyeTracking.GazeData data, VarjoEyeTracking.EyeMeasurements eyeMeasurements)
    {
        string[] logData = new string[23];

        // Gaze data frame number
        logData[0] = data.frameNumber.ToString();

        // Gaze data capture time (nanoseconds)
        logData[1] = data.captureTime.ToString();

        // Log time (milliseconds)
        logData[2] = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString();

        // HMD
        logData[3] = xrCamera.transform.localPosition.ToString("F3");
        logData[4] = xrCamera.transform.localRotation.ToString("F3");

        // Combined gaze
        bool invalid = data.status == VarjoEyeTracking.GazeStatus.Invalid;
        logData[5] = invalid ? InvalidString : ValidString;
        logData[6] = invalid ? "" : data.gaze.forward.ToString("F3");
        logData[7] = invalid ? "" : data.gaze.origin.ToString("F3");

        // IPD
        logData[8] = invalid ? "" : eyeMeasurements.interPupillaryDistanceInMM.ToString("F3");

        // Left eye
        bool leftInvalid = data.leftStatus == VarjoEyeTracking.GazeEyeStatus.Invalid;
        logData[9] = leftInvalid ? InvalidString : ValidString;
        logData[10] = leftInvalid ? "" : data.left.forward.ToString("F3");
        logData[11] = leftInvalid ? "" : data.left.origin.ToString("F3");
        logData[12] = leftInvalid ? "" : eyeMeasurements.leftPupilIrisDiameterRatio.ToString("F3");
        logData[13] = leftInvalid ? "" : eyeMeasurements.leftPupilDiameterInMM.ToString("F3");
        logData[14] = leftInvalid ? "" : eyeMeasurements.leftIrisDiameterInMM.ToString("F3");

        // Right eye
        bool rightInvalid = data.rightStatus == VarjoEyeTracking.GazeEyeStatus.Invalid;
        logData[15] = rightInvalid ? InvalidString : ValidString;
        logData[16] = rightInvalid ? "" : data.right.forward.ToString("F3");
        logData[17] = rightInvalid ? "" : data.right.origin.ToString("F3");
        logData[18] = rightInvalid ? "" : eyeMeasurements.rightPupilIrisDiameterRatio.ToString("F3");
        logData[19] = rightInvalid ? "" : eyeMeasurements.rightPupilDiameterInMM.ToString("F3");
        logData[20] = rightInvalid ? "" : eyeMeasurements.rightIrisDiameterInMM.ToString("F3");

        // Focus
        logData[21] = invalid ? "" : data.focusDistance.ToString();
        logData[22] = invalid ? "" : data.focusStability.ToString();

        Log(logData);
    }

    // Write given values in the log file
    void Log(string[] values)
    {
        if (!logging || writer == null)
            return;

        string line = "";
        for (int i = 0; i < values.Length; ++i)
        {
            values[i] = values[i].Replace("\r", "").Replace("\n", ""); // Remove new lines so they don't break csv
            line += values[i] + (i == (values.Length - 1) ? "" : ";"); // Do not add semicolon to last data string
        }
        writer.WriteLine(line);
    }

    public void StartLogging()
    {
        if (logging)
        {
            Debug.LogWarning("Logging was on when StartLogging was called. No new log was started.");
            return;
        }

        logging = true;

        string logPath = useCustomLogPath ? customLogPath : Application.dataPath + "/Logs/";
        Directory.CreateDirectory(logPath);

        DateTime now = DateTime.Now;
        string fileName = string.Format("{0}-{1:00}-{2:00}-{3:00}-{4:00}", now.Year, now.Month, now.Day, now.Hour, now.Minute);

        string path = logPath + fileName + ".csv";
        writer = new StreamWriter(path);

        Log(ColumnNames);
        Debug.Log("Log file started at: " + path);
    }

    void StopLogging()
    {
        if (!logging)
            return;

        if (writer != null)
        {
            writer.Flush();
            writer.Close();
            writer = null;
        }
        logging = false;
        Debug.Log("Logging ended");
    }

    void OnApplicationQuit()
    {
        StopLogging();
        CloseCSVFile();
    }
}