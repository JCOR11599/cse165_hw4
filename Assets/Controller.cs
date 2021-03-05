using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    // Tracking space of character, to help with throws
    public Transform trackingSpace;

    // Track selected objects
    private GameObject collidingObject = null;  // collider that the hand is colliding with
    public GameObject selectedObject = null; // collider that we last grabbed
    public GameObject prevSelectedObject = null; // collider that was previously selected
    
    private Vector3 prevSelectedObjectPosition;

    private bool isHolding = false; // tracks when user is already holding an object

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If pressing trigger and not already holding something
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) && !isHolding)
        {
            // If there is something to grab
            if (collidingObject)
            {
                // Mark that you are now holding something
                isHolding = true;

                // Remember previously selected
                prevSelectedObject = selectedObject;

                // Mark that collidingObject is now selected
                selectedObject = collidingObject;

                // Unhighlight previously selectedObject
                if (prevSelectedObject)
                {
                    prevSelectedObject.GetComponent<Outline>().enabled = false;
                }

                // Highlight selectedObject
                selectedObject.GetComponent<Outline>().enabled = true;

                // Disable gravity and momentum on selected object
                Rigidbody rb = selectedObject.GetComponent<Rigidbody>();
                if (rb)
                {
                    rb.isKinematic = true;
                }

                // Have object follow transformations of hand
                selectedObject.transform.SetParent(this.transform);
            }
        }
        // If not pressing trigger and holding something
        else if (!OVRInput.Get(OVRInput.RawButton.RIndexTrigger) && isHolding)
        {
            // Mark that you are not holding
            isHolding = false;

            // Reenable gravity and momentum
            Rigidbody rb = selectedObject.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.isKinematic = false;
                rb.velocity = trackingSpace.rotation * (2.0f * OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch));
                rb.angularVelocity = 0.5f * OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RTouch);
            }

            // No longer inherit transforms
            selectedObject.transform.SetParent(null);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Indicate that this is the one object being selected
        collidingObject = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        // Nothing is colliding
        collidingObject = null;
    }
}
