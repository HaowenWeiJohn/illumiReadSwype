using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CVEPManager : MonoBehaviour
{
    public EventMarkerLSLOutletController eventMarkerLSLOutletController;
    [Header("Flash Settings")]
    public float flashInterval = 0.033f;

    public float flashDuration = 4f;

    public float waitBetweenSequences = 2f;  // Time to wait between sequences
    public int numberOfTrainingEpochs = 1;   // Number of training epochs

    public int LetterIndex = -1;

    [Header("M-Sequence Settings")]
    // [Tooltip("The exponent value n for the length of the m-sequence (length = 2^n - 1).")]
    public bool LoadFile = false;

    public string FileName = "";
    public int n = 6; // Control the length of the m-sequence via n (default is 6)

    private int[] mSequence; // Original m-sequence
    private int[][] laggedSequences; // Array to store the lagged sequences

    public GameObject KeyBoard;
    public RectTransform targetGUI;
    public RectTransform inputGUI;
    // Start is called before the first frame update
    void Start()
    {
        ResetKeyBoard();
        if(LoadFile)
        {

        }
        else
        {
            GenerateMSequence(n);
            CreateLaggedSequences();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(Presets.NextStateKey))
        {
            LetterIndex++;
            ResetKeyBoard();
            StartCoroutine(FlashTrainingSequences());
            eventMarkerLSLOutletController.sendUserInputsMarker(LetterIndex);
            
        }
        
    }


    // Generates an m-sequence of length 2^n - 1 based on the given n value
    void GenerateMSequence(int n)
    {
        int sequenceLength = (1 << n) - 1; // Length is 2^n - 1
        mSequence = new int[sequenceLength];

        // Initialize the register with a seed (any non-zero state)
        int[] register = new int[n];
        register[0] = 1; // Example seed: make sure at least one bit is 1

        // Get the taps from the primitive polynomial for this n
        int[] taps = GetPrimitivePolynomial(n);

        // Generate the m-sequence
        for (int i = 0; i < sequenceLength; i++)
        {
            // Output the current value (from the last register position)
            mSequence[i] = register[register.Length - 1];

            // Calculate the feedback value using XOR of the tapped positions
            int feedback = 0;
            foreach (int tap in taps)
            {
                feedback ^= register[tap];
            }

            // Shift the register and insert the feedback bit
            for (int j = register.Length - 1; j > 0; j--)
            {
                register[j] = register[j - 1];
            }
            register[0] = feedback;
        }

        Debug.Log("Generated M-Sequence: " + string.Join(", ", mSequence));
    }

    // Creates 26 additional lagged versions of the original m-sequence
    void CreateLaggedSequences()
    {
        int sequenceLength = mSequence.Length;
        laggedSequences = new int[27][]; // Store the original and 26 lagged sequences

        // The first sequence is the original one
        laggedSequences[0] = (int[])mSequence.Clone();

        // Generate the lagged sequences
        for (int lag = 1; lag < 27; lag++)
        {
            laggedSequences[lag] = new int[sequenceLength];
            for (int i = 0; i < sequenceLength; i++)
            {
                // Cyclically shift the sequence by 'lag' positions
                laggedSequences[lag][i] = mSequence[(i + lag) % sequenceLength];
            }
        }

        Debug.Log("Generated 26 lagged sequences.");
    }

    // Returns the tap positions for the feedback function based on the primitive polynomial for n
    int[] GetPrimitivePolynomial(int n)
    {
        // Primitive polynomials for small values of n (these are 0-based taps)
        // Reference for binary m-sequences
        switch (n)
        {
            case 2: return new int[] { 0, 1 }; // x^2 + x + 1
            case 3: return new int[] { 0, 1 }; // x^3 + x + 1
            case 4: return new int[] { 0, 3 }; // x^4 + x^3 + 1
            case 5: return new int[] { 0, 2 }; // x^5 + x^3 + 1
            case 6: return new int[] { 0, 5 }; // x^6 + x^5 + 1 (from your article)
            case 7: return new int[] { 0, 6 }; // x^7 + x^6 + 1
            case 8: return new int[] { 0, 2, 3, 7 }; // x^8 + x^6 + x^5 + x^4 + 1
            default:
                Debug.LogError("Unsupported n value. Choose n between 2 and 8.");
                return new int[] { 0 }; // Invalid case
        }
    }

    public void ResetKeyBoard()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 forwardDirection = Camera.main.transform.forward;

        targetGUI.localPosition = new Vector3(targetGUI.localPosition.x, targetGUI.localPosition.y-0.4f, targetGUI.localPosition.z);
        inputGUI.localPosition = new Vector3(inputGUI.localPosition.x, inputGUI.localPosition.y-0.4f, inputGUI.localPosition.z);
        
        
        // Calculate the desired position with the height offset
        Vector3 desiredPosition = cameraPosition + forwardDirection * 0.7f;
        desiredPosition.y -= 0.03f;

        // Set the KeyBoard's position
        KeyBoard.transform.position = desiredPosition;

        // Make the KeyBoard face the camera with a 180-degree rotation on the Y-axis
        Vector3 lookDirection = cameraPosition - KeyBoard.transform.position;
        lookDirection.y = 0; // Keep the rotation only around the Y-axis

        // Calculate the rotation to face the camera with an upward tilt
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection) * Quaternion.Euler(10f, 180, 0);
        KeyBoard.transform.rotation = targetRotation;

        // Optionally, ensure the KeyBoard is only rotating around the Y-axis (vertical)
        KeyBoard.transform.rotation = Quaternion.Euler(KeyBoard.transform.rotation.eulerAngles.x, KeyBoard.transform.rotation.eulerAngles.y, 0);
    }

    // IEnumerator FlashSequence(int[] sequence)
    // {
    //     float startTime = Time.time;  // Record start time for duration

    //     for (int i = 0; i < sequence.Length; i++)
    //     {
    //         // Set the object to black or white based on the m-sequence value (1 = white, 0 = black)
    //         cubeRenderer.material.color = sequence[i] == 1 ? Color.white : Color.black;

    //         // Wait for the next flash interval
    //         yield return new WaitForSeconds(flashInterval);
    //     }

    // }

    IEnumerator FlashTrainingSequences()
    {
        
        for (int epoch = 0; epoch < numberOfTrainingEpochs; epoch++)
        {
            float startTime = Time.time;  // Timer for the total duration of the training epoch

            for (int sequenceIndex = 0; sequenceIndex < mSequence.Length; sequenceIndex++)
            {
                if(Time.time - startTime > flashDuration)
                {
                    break;
                }

                // character iteration
                for(int charIndex =0; charIndex < 26; charIndex++)
                {
                    char charLetter = (char)('a' + charIndex);
                    Button button = GameObject.Find(charLetter.ToString()).GetComponent<Button>();
                    
                    // Debug.Log(charLetter);

                    if(button!=null)
                    {
                        // Debug.Log("Found");
                        // Debug.Log(GameObject.Find(charLetter.ToString()).GetComponent<Button>().name);
                        ColorBlock cb = button.colors;
                        // Debug.Log("colro"+cb.normalColor);
                        if( laggedSequences[charIndex][sequenceIndex] == 1)
                        {
                            cb.normalColor = Color.green;
                            // Debug.Log("green");
                        }
                        else
                        {
                            cb.normalColor = new Color(0.5f, 0.5f, 0.5f,1f);
                        }

                        button.colors = cb;
                        // cb.normalColor = laggedSequences[charIndex][sequenceIndex] == 1 ? new Color(147f/255f, 147f/255f, 147f/255f) : Color.green;
                        // GameObject.Find(charLetter.ToString()).GetComponent<Button>().colors.normalColor = laggedSequences[charIndex][sequenceIndex] == 1 ? new Color(147f/255f, 147f/255f, 147f/255f) : Color.green;
                    }
                    // Set the object to black or white based on the m-sequence value (1 = white, 0 = black)
                    // cubeRenderer.material.color = laggedSequences[charIndex][sequenceIndex] == 1 ? Color.white : Color.black;

                    // // Wait for the next flash interval
                    // yield return new WaitForSeconds(flashInterval);
                }

                
                yield return new WaitForSeconds(flashInterval);
            }

            for(int charIndex =0; charIndex < 26; charIndex++)
            {
                char charLetter = (char)('a' + charIndex);
                Button button = GameObject.Find(charLetter.ToString()).GetComponent<Button>();
                
                // Debug.Log(charLetter);

                if(button!=null)
                {
                    // Debug.Log("Found");
                    // Debug.Log(GameObject.Find(charLetter.ToString()).GetComponent<Button>().name);
                    ColorBlock cb = button.colors;
                    // Debug.Log("colro"+cb.normalColor);
                    
                    cb.normalColor = new Color(0.5f, 0.5f, 0.5f,1f);
                    

                    button.colors = cb;
                    // cb.normalColor = laggedSequences[charIndex][sequenceIndex] == 1 ? new Color(147f/255f, 147f/255f, 147f/255f) : Color.green;
                    // GameObject.Find(charLetter.ToString()).GetComponent<Button>().colors.normalColor = laggedSequences[charIndex][sequenceIndex] == 1 ? new Color(147f/255f, 147f/255f, 147f/255f) : Color.green;
                }
            }

            yield return new WaitForSeconds(waitBetweenSequences);


        }

    }
}
