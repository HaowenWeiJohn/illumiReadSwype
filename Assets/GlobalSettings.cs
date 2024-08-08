using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Varjo.XR;
using System;
using Cysharp.Net.Http;
using Grpc.Core;
using Grpc.Net.Client;
using TMPro;
using UnityEngine.UI;

public class GlobalSettings : MonoBehaviour
{
    // the unity RPC call from PhysiioLabXR
    private GrpcChannel channel;
    private YetAnotherHttpHandler handler;
    private IllumiReadSwypeScript.IllumiReadSwypeScriptClient client;
    public string host = "http://localhost:8004";

    public bool keyNumPressed=false;
    // public illumiReadSwypeKeyboardContextLSLOutletController KeyboardContextLSLOutletController;

    public TMPro.TMP_InputField keyboardOutputText;

    // awake function called before start
    void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        VarjoMixedReality.StartRender();

        // Set up the RPC client
        handler = new YetAnotherHttpHandler(){Http2Only = true};  // GRPC requires HTTP/2
        channel = GrpcChannel.ForAddress(host, new GrpcChannelOptions() { HttpHandler = handler, Credentials = ChannelCredentials.Insecure});
        client = new IllumiReadSwypeScript.IllumiReadSwypeScriptClient(channel);
    }

    // Update is called once per frame
    void Update()
    {
        // KeyboardContextLSLOutletController.PushKeyboardContextData(
        //     keyboardOutputText.text
        // );
        // Debug.Log("Keyboard Output Text: " + keyboardOutputText.text);

        // Debug.Log("Calling RPC");
        string KeyboardContext = keyboardOutputText.text+" ";
        var reply = client.ContextRPC(new ContextRPCRequest { Input0 = KeyboardContext });

        // Debug.Log(reply.ToString());
        // Text.text = $"[{DateTime.Now}] {reply}";
    }
}
