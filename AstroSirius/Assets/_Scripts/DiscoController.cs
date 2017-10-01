using UnityEngine;
using System.Collections;

public class DiscoController : MonoBehaviour
{
	public Animator[] AnimationControllers;

	void Awake()
	{
		for (int i = 0; i < AnimationControllers.Length; ++i)
		{
			AnimationControllers[i].SetFloat("Offset", Random.Range(0.1f, 0.9f));
		}
	}
}
