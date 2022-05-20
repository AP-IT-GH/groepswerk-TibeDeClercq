using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandManager : MonoBehaviour
{
    public GameObject leftOVRControllerPrefab;
    public GameObject rightOVRControllerPrefab;
    public GameObject leftHandAnchor;
    public GameObject rightHandAnchor;
    public GameObject fingerCollider;

    public GameObject leftOVRHandPrefab;
    public GameObject rightOVRHandPrefab;

    private OVRHand _leftHand;
    private OVRHand _rightHand;

    private XRInteractorLineVisual _leftRay;
    private XRInteractorLineVisual _rightRay;

    private OVRSkeleton _leftHandSkeleton;
    private OVRSkeleton _rightHandSkeleton;

    private GameObject _leftIndex;
    private GameObject _leftThumb;
    private GameObject _leftMiddle;
    private GameObject _leftRing;
    private GameObject _leftPinky;

    private GameObject _rightIndex;
    private GameObject _rightThumb;
    private GameObject _rightMiddle;
    private GameObject _rightRing;
    private GameObject _rightPinky;

    private static bool handsActive = false;

    private void Start()
    {
        _leftHand = leftOVRHandPrefab.GetComponent<OVRHand>();
        _rightHand = rightOVRHandPrefab.GetComponent<OVRHand>();

        _leftHandSkeleton = leftOVRHandPrefab.GetComponent<OVRSkeleton>();
        _rightHandSkeleton = rightOVRHandPrefab.GetComponent<OVRSkeleton>();

        _leftRay = leftHandAnchor.GetComponent<XRInteractorLineVisual>();
        _rightRay = rightHandAnchor.GetComponent<XRInteractorLineVisual>();

        InstantiateColliders();
    }

    private void Update()
    {
        updateHandState();
        TrackFingers();
    }

    private void InstantiateColliders()
    {
        _leftIndex = Instantiate(fingerCollider, transform);
        _leftThumb = Instantiate(fingerCollider, transform);
        _leftMiddle = Instantiate(fingerCollider, transform);
        _leftRing = Instantiate(fingerCollider, transform);
        _leftPinky = Instantiate(fingerCollider, transform);

        _rightIndex = Instantiate(fingerCollider, transform);
        _rightThumb = Instantiate(fingerCollider, transform);
        _rightMiddle = Instantiate(fingerCollider, transform);
        _rightRing = Instantiate(fingerCollider, transform);
        _rightPinky = Instantiate(fingerCollider, transform);
    }

    private void updateHandState()
    {
        handsActive = _leftHand.IsTracked || _rightHand.IsTracked;

        if (handsActive)
        {
            _leftRay.enabled = false;
            _rightRay.enabled = false;
        }
        else
        {
            _leftRay.enabled = true;
            _rightRay.enabled = true;
        }
    }

    private void TrackFingers()
    {
        if (handsActive)
        {
            _leftThumb.transform.position = _leftHandSkeleton.Bones[19].Transform.position;
            _leftIndex.transform.position = _leftHandSkeleton.Bones[20].Transform.position;
            _leftMiddle.transform.position = _leftHandSkeleton.Bones[21].Transform.position;
            _leftRing.transform.position = _leftHandSkeleton.Bones[22].Transform.position;
            _leftPinky.transform.position = _leftHandSkeleton.Bones[23].Transform.position;

            _rightThumb.transform.position = _rightHandSkeleton.Bones[19].Transform.position;
            _rightIndex.transform.position = _rightHandSkeleton.Bones[20].Transform.position;
            _rightMiddle.transform.position = _rightHandSkeleton.Bones[21].Transform.position;
            _rightRing.transform.position = _rightHandSkeleton.Bones[22].Transform.position;
            _rightPinky.transform.position = _rightHandSkeleton.Bones[23].Transform.position;
        }
    }
}
