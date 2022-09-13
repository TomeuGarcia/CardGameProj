using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTransform : MonoBehaviour
{
    private Card thisCard;
    [SerializeField] Transform meshTransform;
    [SerializeField] Animator animator;

    private bool eventsDisabled = false;

    public Vector3 Position => transform.position;
    public Vector3 MeshPosition => meshTransform.position;


    public delegate void CardTransformAction(Card thisCard);
    public event CardTransformAction OnPlayerMouseEnter;
    public event CardTransformAction OnPlayerMouseExit;
    public event CardTransformAction OnPlayerMouseClickDown;
    public event CardTransformAction OnPlayerMouseClickUp;

    public event CardTransformAction OnEndMove;
    public event CardTransformAction OnEndRotate;



    private void OnMouseEnter()
    {
        if (eventsDisabled) return;
        if (OnPlayerMouseEnter != null) OnPlayerMouseEnter(thisCard);
    }
    
    private void OnMouseExit()
    {
        if (eventsDisabled) return; 
        if (OnPlayerMouseExit != null) OnPlayerMouseExit(thisCard);
    }

    private void OnMouseDown()
    {
        if (eventsDisabled) return; 
        if (OnPlayerMouseClickDown != null) OnPlayerMouseClickDown(thisCard);
    }

    private void OnMouseUp()
    {
        if (eventsDisabled) return; 
        if (OnPlayerMouseClickUp != null) OnPlayerMouseClickUp(thisCard);
    }


    public void Init(Card thisCard)
    {
        this.thisCard = thisCard;
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void InstantMoveToMeshPosition()
    {
        transform.position = MeshPosition;
        meshTransform.localPosition = Vector3.zero;
    }

    public void MoveFromToPosition(Vector3 from, Vector3 to, float duration = 0.01f, bool eventsDisabledForDuration = false)
    {
        transform.position = from;
        StartCoroutine(DoMoveToPosition(to, duration, this.transform, eventsDisabledForDuration));
    }

    public void MoveToPosition(Vector3 to, float duration = 0.01f, bool eventsDisabledForDuration = false)
    {
        StartCoroutine(DoMoveToPosition(to, duration, this.transform, eventsDisabledForDuration));
    }

    public void MoveMeshToOrigin(float duration = 0.01f, bool eventsDisabledForDuration = false)
    {
        StartCoroutine(DoMoveToPosition(Position, duration, meshTransform, eventsDisabledForDuration));
    }

    public void MoveMeshToPosition(Vector3 to, float duration = 0.01f, bool eventsDisabledForDuration = false)
    {
        StartCoroutine(DoMoveToPosition(to, duration, meshTransform, eventsDisabledForDuration));
    }

    

    private IEnumerator DoMoveToPosition(Vector3 to, float duration, Transform transform, bool eventsDisabledForDuration)
    {
        if (eventsDisabledForDuration) eventsDisabled = true;

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

        if (eventsDisabledForDuration) eventsDisabled = false;

        if (OnEndMove != null) OnEndMove(thisCard);
    }

    public void MoveOriginToMeshPosition()
    {
        transform.position = meshTransform.position;
        meshTransform.localPosition = Vector3.zero;
    }
    

    public void Rotate(Quaternion to, float duration = 0.01f)
    {
        StartCoroutine(DoRotate(to, duration, meshTransform));
    }

    private IEnumerator DoRotate(Quaternion to, float duration, Transform transform)
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

        if (OnEndRotate != null) OnEndRotate(thisCard);
    }



    public void PlayDeckShuffleAnimation(float delayDuration = 0.0f)
    {
        StartCoroutine(DoPlayDeckShuffleAnimation(delayDuration));
    }

    private IEnumerator DoPlayDeckShuffleAnimation(float delayDuration)
    {
        animator.SetTrigger("DeckShuffleReady");
        yield return new WaitForSeconds(delayDuration);
        animator.SetTrigger("DeckShuffle");
    }


    public float PlayAttackAnimation(Vector3 forward)
    {
        float tA = 0.2f;
        float tB = 0.2f;
        float tC = 0.1f;
        StartCoroutine(DoPlayAttackAnimation(tA, tB, tC, forward));
        return tA + tB + tC;
    }

    private IEnumerator DoPlayAttackAnimation(float tA, float tB, float tC, Vector3 forward)
    {
        MoveMeshToPosition(Position + new Vector3(0f, 1f, 0f) + (forward * -0.5f), tA);
        Rotate(Quaternion.Euler(new Vector3(-20f, 0f, -30f)), tA);        
        yield return new WaitForSeconds(tA);

        MoveMeshToPosition(Position + new Vector3(0f, 0.7f, 0f) + (forward * 0.5f), tB);
        yield return new WaitForSeconds(tB);

        MoveMeshToOrigin(tC);
        Rotate(Quaternion.Euler(Vector3.zero), tC);
        yield return new WaitForSeconds(tC);
    }

}
