using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cam;
using Player;

public class CP_DeathCam : CamPoint
{

    public override void StartPoint()
    {
        base.StartPoint();

        transform.position = CamController.Instance.transform.position;
        transform.rotation = CamController.Instance.transform.rotation;
    }

    public override void UpdatePoint()
    {
        base.UpdatePoint();
    }
    public override void EndPoint()
    {
        base.EndPoint();
    }

    protected override void Start()
    {
        base.Start();

        PlayerManager.Instance.Health.OnHasDied += Activate;
    }

    void Activate() 
    {
        CamController.Instance.ChangeCamPoint(this);
        PlayerManager.Instance.Health.OnHasDied -= Activate;
    }
}
