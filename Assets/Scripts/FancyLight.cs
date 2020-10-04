#pragma warning disable 649
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FancyLight : MonoBehaviour
{
	[SerializeField] float fSizeVar;
	[SerializeField, Range(0, 1)] float fSizeChangeOdds;

	float fOGSize;
	Light2D light2D;

	void Awake()
	{
		light2D = GetComponent<Light2D>();
		fOGSize = light2D.pointLightOuterRadius;
	}

	void Update()
	{
		if (Random.value > fSizeChangeOdds)
		{
			light2D.pointLightOuterRadius = fOGSize + Random.Range(0, fSizeVar);
		}
	}
}