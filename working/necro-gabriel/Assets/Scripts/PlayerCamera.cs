using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    [SerializeField]
    Transform cameraTransform;

    [SerializeField]
    float sensitivityX, sensitivityY;

    //[SerializeField]
    //float minRotY, maxRotY;

    [SerializeField]
    float scrolling;

    [SerializeField]
    float offsetFromPlayerX, offsetFromPlayerY, offsetFromPlayerZ;

    private float rotationX, rotationY;
    private Vector3 lastPlayerPosition;

	// Use this for initialization
	void Start () {
        CorrectCameraPosition();
        rotationY = 0;
    }
	
	// Update is called once per frame
	void Update () {
        CorrectCameraPosition();

        lastPlayerPosition = transform.position;
    }

    private void LateUpdate()
    {
        CorrectCameraRotation();
        cameraTransform.LookAt(transform);
    }

    void CorrectCameraPosition()
    {
        if (transform.position != lastPlayerPosition)
        {
            cameraTransform.position = transform.position + new Vector3(offsetFromPlayerX, offsetFromPlayerY, offsetFromPlayerZ);
        }
    }

    void CorrectCameraRotation()
    {
        if (Input.GetMouseButton(1))
        {
            cameraTransform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Mouse X") * sensitivityX);
            UpdateCameraOffset();
        }
    }

    // scrolling

    void UpdateCameraOffset()
    {
        var PlayerToCamVec = cameraTransform.position - transform.position;
        offsetFromPlayerX = PlayerToCamVec.x;
        offsetFromPlayerY = PlayerToCamVec.y;
        offsetFromPlayerZ = PlayerToCamVec.z;
    }
}
