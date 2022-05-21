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

    private XRRayInteractor _leftRayInteractor;
    private XRRayInteractor _rightRayInteractor;

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

    private List<RaycastHit> hits = new List<RaycastHit>();
    private ObservationCube lastSelectedCube = null;

    private static bool handsActive = false;

    private void Start()
    {
        _leftHand = leftOVRHandPrefab.GetComponent<OVRHand>();
        _rightHand = rightOVRHandPrefab.GetComponent<OVRHand>();

        _leftHandSkeleton = leftOVRHandPrefab.GetComponent<OVRSkeleton>();
        _rightHandSkeleton = rightOVRHandPrefab.GetComponent<OVRSkeleton>();

        _leftRay = leftHandAnchor.GetComponent<XRInteractorLineVisual>();
        _rightRay = rightHandAnchor.GetComponent<XRInteractorLineVisual>();

        _leftRayInteractor = leftHandAnchor.GetComponent<XRRayInteractor>();
        _rightRayInteractor = rightHandAnchor.GetComponent<XRRayInteractor>();

        InstantiateColliders();
    }

    private void Update()
    {
        updateHandState();
        TrackFingers();
    }

    private void FixedUpdate()
    {
        CastSelectionRays();
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
            _leftRayInteractor.enabled = false;
            _rightRayInteractor.enabled = false;
        }
        else
        {
            _leftRay.enabled = true;
            _rightRay.enabled = true;
            _leftRayInteractor.enabled = true;
            _rightRayInteractor.enabled = true;
        }
    }

    private void TrackFingers()
    {
        if (handsActive)
        {
            //Set fingertip position
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

    private void CastSelectionRays()
    {
        if (handsActive)
        {
            hits.Clear();

            //Cast rays from all fingers and save all valid hits
            RaycastHit hit;
            List<string> validTags = new List<string>() { "W", "H", "M" };

            if (Physics.Raycast(_leftThumb.transform.position, _leftThumb.transform.TransformDirection(Vector3.forward), out hit, 0.2f)) { if (validTags.Contains(hit.collider.gameObject.tag)) hits.Add(hit); }
            if (Physics.Raycast(_leftIndex.transform.position, _leftIndex.transform.TransformDirection(Vector3.forward), out hit, 0.2f)) { if (validTags.Contains(hit.collider.gameObject.tag)) hits.Add(hit); }
            if (Physics.Raycast(_leftMiddle.transform.position, _leftMiddle.transform.TransformDirection(Vector3.forward), out hit, 0.2f)) { if (validTags.Contains(hit.collider.gameObject.tag)) hits.Add(hit); }
            if (Physics.Raycast(_leftRing.transform.position, _leftRing.transform.TransformDirection(Vector3.forward), out hit, 0.2f)) { if (validTags.Contains(hit.collider.gameObject.tag)) hits.Add(hit); }
            if (Physics.Raycast(_leftPinky.transform.position, _leftPinky.transform.TransformDirection(Vector3.forward), out hit, 0.2f)) { if (validTags.Contains(hit.collider.gameObject.tag)) hits.Add(hit); }

            if (Physics.Raycast(_rightThumb.transform.position, _rightThumb.transform.TransformDirection(Vector3.forward), out hit, 0.2f)) { if (validTags.Contains(hit.collider.gameObject.tag)) hits.Add(hit); }
            if (Physics.Raycast(_rightIndex.transform.position, _rightIndex.transform.TransformDirection(Vector3.forward), out hit, 0.2f)) { if (validTags.Contains(hit.collider.gameObject.tag)) hits.Add(hit); }
            if (Physics.Raycast(_rightMiddle.transform.position, _rightMiddle.transform.TransformDirection(Vector3.forward), out hit, 0.2f)) { if (validTags.Contains(hit.collider.gameObject.tag)) hits.Add(hit); }
            if (Physics.Raycast(_rightRing.transform.position, _rightRing.transform.TransformDirection(Vector3.forward), out hit, 0.2f)) { if (validTags.Contains(hit.collider.gameObject.tag)) hits.Add(hit); }
            if (Physics.Raycast(_rightPinky.transform.position, _rightPinky.transform.TransformDirection(Vector3.forward), out hit, 0.2f)) { if (validTags.Contains(hit.collider.gameObject.tag)) hits.Add(hit); }


            if (hits.Count > 0)
            {
                //check which hit is closest, if it's colliding with a cube
                RaycastHit minHit = hits[0];
                for (int i = 0; i < hits.Count; i++)
                {
                    if (hits[i].distance < minHit.distance && validTags.Contains(hits[i].collider.gameObject.tag))
                    {
                        minHit = hits[i];
                    }
                }

                //Select the closest ray, if its not already selected
                ObservationCube selectedCube = minHit.collider.gameObject.GetComponent<ObservationCube>();
                if (selectedCube != lastSelectedCube && lastSelectedCube != null)
                {
                    lastSelectedCube.RemoveOutline();
                    selectedCube.CreateOutline();
                    lastSelectedCube = selectedCube;
                }
                else if(lastSelectedCube == null)
                {
                    selectedCube.CreateOutline();
                    lastSelectedCube = selectedCube;
                }
            }
            else
            {
                if (lastSelectedCube != null)
                {
                    lastSelectedCube.RemoveOutline();
                    lastSelectedCube = null;
                }                
            }
        }            
    }
}
