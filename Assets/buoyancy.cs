using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buoyancy : MonoBehaviour
{
    [SerializeField]
    private float floating = 1.0f, waterDensity = 1.03f, airDrag = 0.0f, airAngularDrag = 0.05f, uwDrag = 3.0f, uwADrag = 1.0f;

    Rigidbody rigidBody;
    MeshRenderer ocean;

    private bool isTriggered = false; 
    // Start is called before the first frame update
    void Start()
    {
        ocean = GameObject.Find("/Mesh").GetComponent<MeshRenderer>();
        rigidBody = GetComponent<Rigidbody>();
    }
    
    void OnTriggerEnter(Collider par) {
        isTriggered = true;
    }


    // Update is called once per frame
    void Update()
    {
        // ocean.bounds.size.y
        if (isTriggered) {
            float difference = rigidBody.position.y - ocean.bounds.size.y;

            if (difference < 0) {
                rigidBody.AddForceAtPosition(Vector3.up * floating * Mathf.Abs(difference), rigidBody.position, ForceMode.Force);
            
                if (!isTriggered) {
                    isTriggered = true;
                    SwitchState(true);
                }
            }  else if (isTriggered) {
                isTriggered = false;
            }
        }
    }

    void SwitchState(bool isUnderwater) {
        if (isUnderwater) {
            rigidBody.drag = uwDrag;
            rigidBody.angularDrag = uwADrag;
        } else {
            rigidBody.drag = airDrag;
            rigidBody.angularDrag = airAngularDrag;
        }
    }
}
