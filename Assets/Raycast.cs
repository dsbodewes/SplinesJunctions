using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Dreamteck.Splines;

public class Raycast : MonoBehaviour
{
    void FixedUpdate()
    {
        RaycastHit hit;
        bool Stop = false;

        if ((Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.TransformDirection(Vector3.forward), out hit, 10)) && hit.transform.tag == "Car")
        {
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            Stop = true;
        }

        else if ((Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.TransformDirection(new Vector3(2, 0, 1)), out hit, 10)) && hit.transform.tag == "Car")
        {
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.TransformDirection(new Vector3(2, 0, 1)) * hit.distance, Color.red);
            Stop = true;
        }

        else
        {
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.TransformDirection(new Vector3(2, 0, 1)) * 8, Color.green);
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.TransformDirection(Vector3.forward) * 8, Color.green);
            Stop = false;
        }


        SplineFollower follower = GetComponent<SplineFollower>();

        if (Stop == true)
        {
            follower.followSpeed = 0f;
        }

        else
        {
            follower.followSpeed = 30f;
        }

    }
}