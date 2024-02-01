using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttachDetachOnEnableDisable : MonoBehaviour
{
    [Space, Tooltip("Delay to detach the transform when enabled")]
    public float delay = 0.3f;

    [Space, Tooltip("The game object to attach to and detach from")]
    public Transform attachTo;

    float curDelayTime;

    // Start is called before the first frame update
    void OnEnable()
    {
        curDelayTime = 0f;
    }

    private void OnDisable()
    {
        // This ensures the scale is reset in preparation for the next OnEnable
        transform.parent = attachTo;
    }

    // Update is called once per frame
    private void Update()
    {
        // Scaling only occurs while curScaleTime is actively below the maximum time
        if (curDelayTime < curDelayTime)
        {
            curDelayTime += Time.deltaTime;

            if (curDelayTime < curDelayTime)
            {
                // delaying is progressing
            }
            else
            {
                // detach the transform
                transform.parent = null;
            }
        }
    }
}
