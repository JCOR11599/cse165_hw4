using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GravityController : MonoBehaviour
{
    // Display gravity
    public Text gravityText;

    // Original gravity
    private float gravity = -1.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Constrain local position
        this.transform.localPosition = new Vector3(Mathf.Clamp(this.transform.localPosition.x, -0.25f, 0.25f), 0.0f, 0.0f);

        // Assign gravity
        gravity = -20.0f * (this.transform.localPosition.x + 0.25f);
        Physics.gravity = new Vector3(0.0f, gravity, 0.0f);
        gravityText.text = "Gravity: " + gravity;
    }
}
