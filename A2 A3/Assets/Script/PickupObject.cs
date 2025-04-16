using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PickupObject : MonoBehaviour
{
    public Transform holdPos;
    public float moveSmooth = 10f;
    public float throwForce = 8f;
    public float rayDistance = 4f;

    private Rigidbody heldRB;
    private Transform heldTransform;
    private Vector3 floatOffset;
    private bool isHolding = false;
    private float bobTime;

    void Update()
    {
        if (isHolding)
        {
            AnimateHeldObject();
            CheckDrop();
            CheckThrow();
        }
        else
        {
            TryPickup();
        }
    }

    void TryPickup()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
            {
                Pickable pickable = hit.collider.GetComponent<Pickable>();
                if (pickable != null)
                {
                    heldRB = hit.collider.GetComponent<Rigidbody>();
                    heldTransform = hit.collider.transform;

                    if (heldRB)
                    {
                        heldRB.isKinematic = true;
                        isHolding = true;
                        heldRB.GetComponent<BoxCollider>().enabled = false;
                        bobTime = 0;
                        //floatOffset = heldTransform.position - holdPos.position;
                    }
                }
            }
        }
    }
    

    void AnimateHeldObject()
    {
        if (heldTransform == null) return;

        bobTime += Time.deltaTime * 2f;
        float bobY = Mathf.Sin(bobTime) * 0.05f;

        Vector3 targetPos = holdPos.position + new Vector3(0, bobY, 0);
        heldTransform.position = Vector3.Lerp(heldTransform.position, targetPos, Time.deltaTime * moveSmooth);

        heldTransform.Rotate(Vector3.up * Time.deltaTime * 50f);
    }

    void CheckDrop()
    {
        if (Input.GetMouseButtonDown(1))
        {
            heldRB.isKinematic = false;
            heldRB.detectCollisions = true;
            heldRB.GetComponent<BoxCollider>().enabled = true;

            Collider col = heldRB.GetComponent<Collider>();
            if (col != null) col.enabled = true;

            heldRB = null;
            heldTransform = null;
            isHolding = false;
        }
    }

    void CheckThrow()
    {
        if (Input.GetMouseButtonDown(0))
        {
            heldRB.isKinematic = false;
            heldRB.detectCollisions = true;
            heldRB.GetComponent<BoxCollider>().enabled = true;

            Collider col = heldRB.GetComponent<Collider>();
            if (col != null) col.enabled = true;

            heldRB.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);

            heldRB = null;
            heldTransform = null;
            isHolding = false;
        }
    }
}
