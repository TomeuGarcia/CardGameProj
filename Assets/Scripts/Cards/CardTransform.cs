using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTransform : MonoBehaviour
{
    [SerializeField] new Collider collider;
    [SerializeField] Transform meshTransform;

    public int id { get; set; }
    public Vector3 Position => transform.position;
    public Vector3 MeshPosition => meshTransform.position;


    public delegate void CardTransformAction(int id);
    public event CardTransformAction OnPlayerMouseEnter;
    public event CardTransformAction OnPlayerMouseExit;
    public event CardTransformAction OnPlayerMouseClickDown;
    public event CardTransformAction OnPlayerMouseClickUp;



    private void OnMouseEnter()
    {
        if (OnPlayerMouseEnter != null) OnPlayerMouseEnter(id);
    }
    
    private void OnMouseExit()
    {
        if (OnPlayerMouseExit != null) OnPlayerMouseExit(id);
    }

    private void OnMouseDown()
    {
        if (OnPlayerMouseClickDown != null) OnPlayerMouseClickDown(id);
    }

    private void OnMouseUp()
    {
        if (OnPlayerMouseClickUp != null) OnPlayerMouseClickUp(id);
    }


    public void MoveFromToPosition(Vector3 from, Vector3 to, float duration = 0.01f, bool disableCollider = false)
    {
        transform.position = from;
        StartCoroutine(DoMoveToPosition(to, duration, this.transform, disableCollider));
    }

    public void MoveToPosition(Vector3 to, float duration = 0.01f, bool disableCollider = false)
    {
        StartCoroutine(DoMoveToPosition(to, duration, this.transform, disableCollider));
    }

    public void MoveMeshToOrigin(float duration = 0.01f, bool disableCollider = false)
    {
        StartCoroutine(DoMoveToPosition(Position, duration, meshTransform, disableCollider));
    }

    public void MoveMeshToPosition(Vector3 to, float duration = 0.01f, bool disableCollider = false)
    {
        StartCoroutine(DoMoveToPosition(to, duration, meshTransform, disableCollider));
    }

    private IEnumerator DoMoveToPosition(Vector3 to, float duration, Transform transform, bool disableCollider)
    {
        if (disableCollider) DisableCollider();

        Vector3 from = transform.position;

        float timeStepScale = 1.0f / duration;
        float t = 0.0f;

        while (t < 1.0f)
        {
            t += Time.deltaTime * timeStepScale;
            transform.position = Vector3.LerpUnclamped(from, to, t);

            yield return null;
        }
        transform.position = to;

        if (disableCollider) EnableCollider();
    }

    public void MoveOriginToMeshPosition()
    {
        transform.position = meshTransform.position;
        meshTransform.localPosition = Vector3.zero;
    }
    

    public void Rotate(Quaternion to, float duration = 0.01f)
    {
        StartCoroutine(DoRotate(to, duration));
    }

    private IEnumerator DoRotate(Quaternion to, float duration)
    {
        Quaternion from = transform.rotation;

        float timeStepScale = 1.0f / duration;
        float t = 0.0f;

        while (t < 1.0f)
        {
            t += Time.deltaTime * timeStepScale;
            transform.rotation = Quaternion.LerpUnclamped(from, to, t);

            yield return null;
        }
        transform.rotation = to;
    }


    public void EnableCollider()
    {
        collider.enabled = true;
    }

    public void DisableCollider()
    {
        collider.enabled = false;
    }

}
