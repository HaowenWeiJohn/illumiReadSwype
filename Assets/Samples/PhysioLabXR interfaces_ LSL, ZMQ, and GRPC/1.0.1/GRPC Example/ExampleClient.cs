using System;
using Cysharp.Net.Http;
using Grpc.Core;
using Grpc.Net.Client;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExampleClient : MonoBehaviour
{
    public Button Button;
    public TMP_Text Text;
    
    private GrpcChannel channel;
    private YetAnotherHttpHandler handler;
    private IllumiReadSwypeScript.IllumiReadSwypeScriptClient client;

    public string host = "http://localhost:8004";

    // Start is called before the first frame update
    void Start()
    {
        Button.onClick.AddListener(OnCallRPC);
        
        // Set up the RPC client
        handler = new YetAnotherHttpHandler(){Http2Only = true};  // GRPC requires HTTP/2
        channel = GrpcChannel.ForAddress(host, new GrpcChannelOptions() { HttpHandler = handler, Credentials = ChannelCredentials.Insecure});
        client = new IllumiReadSwypeScript.IllumiReadSwypeScriptClient(channel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnCallRPC()
    {
        Debug.Log("Calling RPC");
        var reply = client.ContextRPC(new ContextRPCRequest { Input0 = "Unity" });
        
        Debug.Log(reply.ToString());
        Text.text = $"[{DateTime.Now}] {reply}";
    }
}