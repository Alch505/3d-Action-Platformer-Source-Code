using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cam;

public class Finish : MonoBehaviour
{
    [SerializeField] CamPoint _finalCamPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            { Debug.Log("Finished!"); }

            CamController.Instance.ChangeCamPoint(_finalCamPoint);
            GameManager.Instance.ChangeState(new GS_Finished());
        }
    }
}
