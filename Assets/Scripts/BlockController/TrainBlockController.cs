using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrainBlockController : BlockController
{
    // Start is called before the first frame update
    void Start()
    {
        experimentStates = ExperimentPreset.ConstructTrainBlock();
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
        experimentStates = ExperimentPreset.ConstructTrainBlock();
    }

}
