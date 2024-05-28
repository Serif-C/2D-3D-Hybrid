using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    [SerializeField] bool xyAxisRotationLocked = true;

    void Update()
    {
        if(xyAxisRotationLocked)
        {
            // Match this object's y-rotation to camera's y-rotation
            transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);

        }
        else
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}
