using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursorControl : MonoBehaviour
{
    bool cursorLocked;
    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cursorLocked)
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            transform.position = new Vector3(pos.x, pos.y, transform.position.z);

            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
    }

    private void OnMouseDown()
    {
        cursorLocked = true;
    }

    private void OnMouseUp()
    {
        cursorLocked = false;
    }
}
