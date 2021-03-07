using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    // Instantiateable objects
    public GameObject cube;
    public GameObject prism;
    public GameObject sphere;
    public GameObject cylinder;

    // Track where index finger is
    public GameObject finger;

    // Tracking space of character, to help with throws
    public Transform trackingSpace;

    // Track selected objects
    private GameObject collidingObject = null;  // what finger tip is colliding with
    private GameObject menuObject = null; // a menu GameObject
    private GameObject selectedObject = null; // buildingblock just selected
    private GameObject prevSelectedObject = null; // buildingblock that was previously selected

    private bool isHolding = false; // tracks when user is already holding an object

    // Track controller rotations
    private Vector3 prevControllerRotation;
    private Vector3 prevDialRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // If holding a menu
        if (menuObject && OVRInput.Get(OVRInput.RawButton.RIndexTrigger) && isHolding)
        {
            // Holding a gravity slider
            if (menuObject.CompareTag("Gravity"))
            {
                Vector3 oldPosition = menuObject.transform.position;
                menuObject.transform.position = new Vector3(finger.transform.position.x, oldPosition.y, oldPosition.z);
            }
            // Holding a size dial
            else if (menuObject.CompareTag("Size"))
            {
                // Assign rotation to size dial
                float rotateAmount = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch).eulerAngles.y - prevControllerRotation.y;
                menuObject.transform.eulerAngles = new Vector3(-180.0f, prevDialRotation.y + rotateAmount + 180.0f, 0.0f);

                // Change size of object
                if (selectedObject)
                {
                    float scaleAmount = 0.01f * (menuObject.transform.eulerAngles.y);
                    // Object is prism
                    if (selectedObject.CompareTag("Prism"))
                    {
                        selectedObject.transform.localScale = new Vector3(scaleAmount, 3.0f * scaleAmount, scaleAmount);
                    }
                    // Object is not a prism
                    else
                    {
                        selectedObject.transform.localScale = new Vector3(scaleAmount, scaleAmount, scaleAmount);
                    }   
                }
            }
            // Pressing red button
            else if (menuObject.CompareTag("Red"))
            {
                // Change color of selected object
                if (selectedObject)
                {
                    selectedObject.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }
            // Pressing green button
            else if (menuObject.CompareTag("Green"))
            {
                // Change color of selected object
                if (selectedObject)
                {
                    selectedObject.GetComponent<MeshRenderer>().material.color = Color.green;
                }
            }
            // Pressing blue button
            else if (menuObject.CompareTag("Blue"))
            {
                // Change color of selected object
                if (selectedObject)
                {
                    selectedObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                }
            }
            // Pressing yellow button
            else if (menuObject.CompareTag("Yellow"))
            {
                // Change color of selected object
                if (selectedObject)
                {
                    selectedObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
                }
            }
        }
        // If pressing trigger and not already holding something
        else if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) && !isHolding)
        {
            // If there is something to grab
            if (collidingObject)
            {
                // Mark that you are now holding something
                isHolding = true;

                // If colliding with building block
                if (collidingObject.CompareTag("Block") || collidingObject.CompareTag("Prism"))
                {
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
                // Colliding with stationary menu
                else if (collidingObject.CompareTag("CreateSphere") || collidingObject.CompareTag("CreateCylinder") || collidingObject.CompareTag("CreateCube") || collidingObject.CompareTag("CreatePrism"))
                {
                    isHolding = false;
                    // Instantiate an object
                    if (collidingObject.CompareTag("CreateSphere"))
                    {
                        collidingObject = Instantiate(sphere, finger.transform.position, Quaternion.identity);
                    }
                    else if (collidingObject.CompareTag("CreateCylinder"))
                    {
                        collidingObject = Instantiate(cylinder, finger.transform.position, Quaternion.identity);
                    }
                    else if (collidingObject.CompareTag("CreateCube"))
                    {
                        collidingObject = Instantiate(cube, finger.transform.position, Quaternion.identity);
                    }
                    else if (collidingObject.CompareTag("CreatePrism"))
                    {
                        collidingObject = Instantiate(prism, finger.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        isHolding = true;
                    }
                }
                // If colliding with a menuObject
                else
                {
                    // Indicate that a menuObject is being held
                    menuObject = collidingObject;

                    // If it's the size dial remember rotations and scale
                    if (menuObject.CompareTag("Size"))
                    {
                        prevDialRotation = menuObject.transform.eulerAngles;
                        prevControllerRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch).eulerAngles;
                    }

                    // Highlight menuObject
                    menuObject.GetComponent<Outline>().enabled = true;
                }
            }
        }
        // If not pressing trigger and holding something
        else if (!OVRInput.Get(OVRInput.RawButton.RIndexTrigger) && isHolding)
        {
            // Mark that you are not holding
            isHolding = false;

            // Reenable gravity and momentum
            if (selectedObject)
            {
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

            // Releasing a menu object
            if (menuObject)
            {
                menuObject.GetComponent<Outline>().enabled = false;
                menuObject = null;
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Indicate that this is the one object being selected
        collidingObject = other.gameObject;
        // Change menu item colors 
        if (collidingObject.CompareTag("CreateSphere") || collidingObject.CompareTag("CreateCylinder") || collidingObject.CompareTag("CreateCube") || collidingObject.CompareTag("CreatePrism"))
        {
            foreach (Transform child in collidingObject.transform)
            {
                child.GetComponent<LineRenderer>().material.color = Color.red;
            }
        }
        // Highlight object if not building block
        else if (!collidingObject.CompareTag("Block") && !collidingObject.CompareTag("Prism"))
        {
            collidingObject.GetComponent<Outline>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Change menu item colors 
        if (other.CompareTag("CreateSphere") || other.CompareTag("CreateCylinder") || other.CompareTag("CreateCube") || other.CompareTag("CreatePrism"))
        {
            foreach(Transform child in other.transform)
            {
                child.GetComponent<LineRenderer>().material.color = Color.white;
            }
        }
        // Highlight object if not building block
        else if (!other.CompareTag("Block") && !other.CompareTag("Prism"))
        {
            other.GetComponent<Outline>().enabled = false;
        }

        // Nothing is colliding
        collidingObject = null;
    }
}
