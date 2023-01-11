using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ModelDragHandler : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 10f;
    private void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * rotateSpeed * Mathf.Deg2Rad;
        float rotY = Input.GetAxis("Mouse Y") * rotateSpeed * Mathf.Deg2Rad;
        #pragma warning disable 0618
        transform.RotateAround(Vector3.up, -rotX);
        transform.RotateAround(Vector3.right, rotY);
    }
}
