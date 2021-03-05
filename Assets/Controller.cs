using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    // Track what is last selected
    public Collider prevSelected = null;

    // Track selected items, would only allow one item to be selected at a time
    private Collider selectedCollider = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If colliding with something
        if (selectedCollider)
        {
            // If pressing trigger
            if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0)
            {
                // Mark item as previously selected
                prevSelected = selectedCollider;

                // Ignore gravity and collisions
                if (selectedCollider.attachedRigidbody)
                {
                    selectedCollider.attachedRigidbody.isKinematic = true;
                }
                selectedCollider.transform.SetParent(this.transform);
            }
            // Not pressing trigger
            else
            {
                // Reapply gravity
                if (selectedCollider.attachedRigidbody)
                {
                    selectedCollider.attachedRigidbody.isKinematic = false;
                }
                // Object no longer follows hand
                selectedCollider.transform.SetParent(null);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Indicate that this is the one object being selected
        selectedCollider = other;
        // Highlight
        selectedCollider.gameObject.GetComponent<Outline>().enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // Unhighlight
        selectedCollider.gameObject.GetComponent<Outline>().enabled = false;
        // Nothing is selected
        selectedCollider = null;
    }
}
