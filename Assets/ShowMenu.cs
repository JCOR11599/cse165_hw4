using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMenu : MonoBehaviour
{
    // Radial Menu
    public GameObject radialMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Cast ray
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.right, out hit, 100.0f))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                radialMenu.SetActive(true);
            }
            else
            {
                radialMenu.SetActive(false);
            }
        }
    }
}
