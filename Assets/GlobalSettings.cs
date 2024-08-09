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

    public bool keyPressed=false;


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
        StartCoroutine(CallLongAsyncRPC());
    }

    private IEnumerator CallLongAsyncRPC()
    {
        string KeyboardContext = keyboardOutputText.text+" ";
        var request = new ContextRPCRequest() { Input0 = KeyboardContext };  // The argument cannot be an empty string
        var call = client.ContextRPCAsync(request);  // The method name in the client is "<method name in RenaScript>Async"
        yield return new WaitUntil(() => call.ResponseAsync.IsCompleted);

        if (call.ResponseAsync.IsCompletedSuccessfully)
        {
            var response = call.ResponseAsync.Result;
            // Text.text = $"[{DateTime.Now}] {response}";
        }
        else
        {
            // Text.text = $"[{DateTime.Now}] Error: {call.ResponseAsync.Exception.Message}";
        }


    }
}
