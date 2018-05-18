using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToggleParticles : MonoBehaviour {

    private ParticleSystem particles;

	private void Start()
	{
        particles = GetComponent<ParticleSystem>();
	}
	private void OnMouseDown()
	{
        if (particles.isPlaying) { particles.Stop(); particles.Clear(); }
        else particles.Play();
	}
}
