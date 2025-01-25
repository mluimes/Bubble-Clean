using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandFollowCamera : MonoBehaviour {
    public Transform CameraTransform; // The camera
    [SerializeField] private float handDistance = 0.5f; // Distance in front of the camera (adjusted locally)
    [SerializeField] private float handHeightOffset = -0.5f; // Vertical height adjustment
    [SerializeField] private float handSideOffset = 0.5f; // Lateral (side to side) offset

    private Vector3 initialHandPosition;

    void Start()
    {
        // Store the initial position of the hand relative to the camera
        initialHandPosition = transform.localPosition;
    }

    void Update()
    {
        // Keep the hand's initial relative position to the camera
        Vector3 handPosition = CameraTransform.position + CameraTransform.forward * handDistance + CameraTransform.up * handHeightOffset + CameraTransform.right * handSideOffset;

        // Set the hand position relative to the camera, without affecting the depth with vertical rotation
        transform.position = handPosition;

        // Make sure the hand rotation follows the camera direction horizontally
        transform.rotation = Quaternion.LookRotation(CameraTransform.forward);
    }
}
