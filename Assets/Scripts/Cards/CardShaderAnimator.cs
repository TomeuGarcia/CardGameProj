using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardShaderAnimator : MonoBehaviour
{
    [SerializeField] private new Renderer renderer;
    private Material cardMaterial;

    [SerializeField] private SpriteRenderer cardSpriteRenderer;
    private Material cardSpriteMaterial;

    [SerializeField] private TextMeshPro cardNameText;


    private Vector3 BoostInvert => Vector3.forward + Vector3.up;
    private Vector3 DamageInvert => Vector3.one;
    private Vector3 DestroyInvert => Vector3.right;


    public void Init()
    {
        cardMaterial = renderer.material;

        cardSpriteMaterial = cardSpriteRenderer.material;

        ResetShaderValues();
    }

    public void SetCardSprite(Sprite cardSprite)
    {
        cardSpriteRenderer.sprite = cardSprite;
    }

    public void SetCardName(string cardName)
    {
        cardNameText.text = cardName;
    }


    public void ResetShaderValues()
    {
        float timeOffset = Random.Range(0f, 10f);
        cardMaterial.SetFloat("_GridShineTimeOffset", timeOffset);
        cardMaterial.SetFloat("_DestroyedT", 0f);

        cardSpriteMaterial.SetFloat("_DistortionTimeOffset", timeOffset);
        cardSpriteMaterial.SetFloat("_DestroyedT", 0f);
        cardSpriteMaterial.SetVector("_ColorInversion", Vector3.zero);
    }


    public void Boost()
    {
        float duration = 0.5f;
        StartCoroutine(DoBoost(duration));
        StartCoroutine(DoSpriteInvertColors(duration, BoostInvert));
    }
    private IEnumerator DoBoost(float duration)
    {
        Vector2 originalDisplacementSpeed = cardMaterial.GetVector("_GridDisplacementSpeed");

        cardMaterial.SetFloat("_BoostedT", 1f);
        cardMaterial.SetVector("_GridDisplacementSpeed", originalDisplacementSpeed * 10);

        yield return new WaitForSeconds(duration);

        cardMaterial.SetFloat("_BoostedT", 0f);
        cardMaterial.SetVector("_GridDisplacementSpeed", originalDisplacementSpeed);
    }

    public void TakeDamage()
    {
        float duration = 0.5f;
        StartCoroutine(DoTakeDamage(duration));
        StartCoroutine(DoSpriteInvertColors(duration, DamageInvert));
    }

    private IEnumerator DoTakeDamage(float duration)
    {
        Vector2 originalDisplacementSpeed = cardMaterial.GetVector("_GridDisplacementSpeed");

        cardMaterial.SetFloat("_DamagedT", 1f);
        cardMaterial.SetVector("_GridDisplacementSpeed", originalDisplacementSpeed * 10);

        yield return new WaitForSeconds(duration);

        cardMaterial.SetFloat("_DamagedT", 0f);
        cardMaterial.SetVector("_GridDisplacementSpeed", originalDisplacementSpeed);
    }

    private IEnumerator DoSpriteInvertColors(float duration, Vector3 colorInversion)
    {
        duration /= 2f;
        yield return new WaitForSeconds(duration);
        int times = 8;
        float step = duration / times;
        for (int i = 0; i < times; ++i)
        {
            cardSpriteMaterial.SetVector("_ColorInversion", i % 2 == 0 ? Vector3.zero : colorInversion);
            yield return new WaitForSeconds(step);
        }
        cardSpriteMaterial.SetVector("_ColorInversion", Vector3.zero);
    }



    public void Destroy(float duration = 0.5f)
    {
        StartCoroutine(DoDestroy(duration));
        StartCoroutine(DoSpriteInvertColors(duration, DestroyInvert));
    }

    private IEnumerator DoDestroy(float duration)
    {
        float step = 1f / duration;

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            cardMaterial.SetFloat("_DestroyedT", t * step);
            cardSpriteMaterial.SetFloat("_DestroyedT", t * step);
            yield return null;
        }
        cardMaterial.SetFloat("_DestroyedT", 1f);
        cardSpriteMaterial.SetFloat("_DestroyedT", 1f);
    }

    public void Fade(float duration = 0.5f)
    {
        StartCoroutine(DoFade(duration));
        StartCoroutine(DoSpriteInvertColors(duration, BoostInvert));
    }

    private IEnumerator DoFade(float duration)
    {
        float step = 1f / duration;

        cardMaterial.SetFloat("_BoostedT", 1f);
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            cardMaterial.SetFloat("_DestroyedT", t * step);
            cardSpriteMaterial.SetFloat("_DestroyedT", t * step);
            yield return null;
        }
        cardMaterial.SetFloat("_BoostedT", 0f);
        cardMaterial.SetFloat("_DestroyedT", 1f);
        cardSpriteMaterial.SetFloat("_DestroyedT", 1f);
    }



}
