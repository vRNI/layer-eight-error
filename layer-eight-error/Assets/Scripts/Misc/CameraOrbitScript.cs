using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbitScript : MonoBehaviour {
    [SerializeField]
    private bool enabled = true;

    public bool Enabled { get { return enabled; } set { enabled = value; } }

    private Transform m_cameraTransform;
    private Transform m_parentTransform;

    private Vector3 m_localRotation;
    private float m_distanceToCamera = 10f;

    [SerializeField]
    float m_sensitivityX, m_sensitivityY, m_sensitivityScroll;

    [SerializeField]
    float m_orbitSpeedDampening, m_scrollSpeedDampening;

    [SerializeField]
    float m_minCameraDistance, m_maxCameraDistance;

    [SerializeField]
    float m_scrollFactor;

	// Use this for initialization
	void Start () {
        m_cameraTransform = transform;
        m_parentTransform = transform.parent;
    }
	
	// Update is called once per frame
    // Late Update is called after everything update related has been finished -> perfect for camera adjustments
	void LateUpdate () {
        if (enabled)
        {
            ComputeMouseInputAxes();
            ComputeMouseInputWheel();
            TransformCamera();
        }
	}

    void ComputeMouseInputAxes()
    {
        if ( (Input.GetAxis("Mouse X") != 0 ||Input.GetAxis("Mouse Y") != 0)
            && Input.GetMouseButton(1))
        {
            m_localRotation.x += Input.GetAxis("Mouse X") * m_sensitivityX;
            m_localRotation.y -= Input.GetAxis("Mouse Y") * m_sensitivityY;

            // avoid flipping
            m_localRotation.y = Mathf.Clamp(m_localRotation.y, 0f, 90f);
        }
    }

    void ComputeMouseInputWheel()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            var scrollWheelInputAmount = Input.GetAxis("Mouse ScrollWheel") * m_sensitivityScroll;
            // adaptive scrolling -> faster scrolling the further away we are
            scrollWheelInputAmount *= m_distanceToCamera * m_scrollFactor;
            m_distanceToCamera += -scrollWheelInputAmount;
            m_distanceToCamera = Mathf.Clamp(m_distanceToCamera, m_minCameraDistance, m_maxCameraDistance);
        }
    }

    void TransformCamera()
    {
        var quat = Quaternion.Euler(m_localRotation.y, m_localRotation.x, 0);
        m_parentTransform.rotation = Quaternion.Lerp(m_parentTransform.rotation, quat, m_orbitSpeedDampening * Time.deltaTime);

        if (m_cameraTransform.localPosition.z != -m_distanceToCamera)
        {
            m_cameraTransform.localPosition = new Vector3(0f, 0f, Mathf.Lerp(m_cameraTransform.localPosition.z, -m_distanceToCamera, m_scrollSpeedDampening * Time.deltaTime));
        }
    }
}
