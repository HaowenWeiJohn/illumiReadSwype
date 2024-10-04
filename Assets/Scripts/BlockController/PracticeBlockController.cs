using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PracticeBlockController : BlockController
{
    // Start is called before the first frame update
    void Start()
    {
        experimentStates = ExperimentPreset.ConstructPracticeBlock();
        DisableSelf();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override void initExperimentBlockStates()
    {
        base.initExperimentBlockStates();
        experimentStates = ExperimentPreset.ConstructPracticeBlock();
    }

}
