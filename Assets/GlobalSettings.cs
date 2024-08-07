using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Varjo.XR;

public class GlobalSettings : MonoBehaviour
{

    public bool keyNumPressed=false;
    public illumiReadSwypeKeyboardContextLSLOutletController KeyboardContextLSLOutletController;

    public TMPro.TMP_InputField keyboardOutputText;

    // awake function called before start
    void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        VarjoMixedReality.StartRender();
    }

    // Update is called once per frame
    void Update()
    {
        KeyboardContextLSLOutletController.PushKeyboardContextData(
            keyboardOutputText.text
        );
        Debug.Log("Keyboard Output Text: " + keyboardOutputText.text);
    }
}
