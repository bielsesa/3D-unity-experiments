using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       if (other.tag == "Player")
        {
            Debug.Log("Player entered water");
            other.SendMessageUpwards("EnterWater");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player exited water");
            other.SendMessageUpwards("ExitWater");
        }
    }
}
