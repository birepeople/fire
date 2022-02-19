using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class controll : MonoBehaviour
{
	[SerializeField] private Transform torchPrefab, torchfirePrefab;
	
	Transform torch;
	private Transform torchfire;
	private ParticleSystem ps;
	
	private Light PointLight;
	private ParticleSystem.ForceOverLifetimeModule forceMode;
	private Vector3 screenPosition; //turn the pos from global coordinate to screen.
	private Vector3 mousePositionOnScreen; // get the pos of screen coordinate when click.
	private Vector3 mousePositionInWorld; // transform the point from screen coordinat to global coordinate.
	private bool is_left = false;
	private bool is_right = false;
	private bool is_torch = false;
	private int contact_num = 0;
	void Awake()
	{
		torch = Instantiate(torchPrefab);
		torchfire = Instantiate(torchfirePrefab);
		Vector3 pos = new Vector3(0, -2, 0);
		torch.localPosition = pos;
		torchfire.localPosition = pos;
	}
	void Start()
	{
		ps = GetComponent<ParticleSystem>();
		forceMode = ps.forceOverLifetime;

		PointLight = GetComponent<Light>();
		PointLight.range = 8;
		PointLight.intensity = 2;
		
		ps.Stop();
		var main = ps.main;
		main.gravityModifier = -0.6f;
		main.maxParticles = 10;
		main.simulationSpeed = 0.8f;
		var em = ps.emission;
		em.rateOverTime = 10;
		ps.Play();
		
		Debug.Log("now");

	}

	void OnGUI()
	{
		if (GUI.Button(new Rect(10, 30, 100, 30), "Torch"))
		{
			is_torch = !is_torch;
			if (!is_torch)
			{
				contact_num = 0;
				Vector3 pos = new Vector3(0, -1, 0);
				torch.localPosition = pos;
				torchfire.localPosition = pos;
			}
			//Debug.Log("button torch");
		}
		
		if (GUI.Button(new Rect(10, 70, 100, 30), "rightWinds"))
		{
			is_right = !is_right;
			
			if (!is_right)
			{
				ps.Stop();
				var main = ps.main;
				main.gravityModifier = -0.6f;
				main.maxParticles = 10;
				main.simulationSpeed = 0.8f;
				var em = ps.emission;
				em.rateOverTime = 10;
				var fo = ps.forceOverLifetime;
				fo.x = 0.0f;
				main.startRotation = 0.0f;
				ps.Play();
			}
			
		}
		
		if (GUI.Button(new Rect(10, 110, 100, 30), "leftWinds"))
		{
			is_left = ! is_left;
			
			if (!is_left)
			{
				ps.Stop();
				var main = ps.main;
				main.gravityModifier = -0.6f;
				main.maxParticles = 10;
				main.simulationSpeed = 0.8f;
				var em = ps.emission;
				em.rateOverTime = 10;
				var fo = ps.forceOverLifetime;
				fo.x = 0.0f;
				main.startRotation = 0.0f;
				ps.Play();
			}
			
		}

		if (is_torch)
		{
			MouseFollow();
			if (contact_num >= 600)
			{
				Vector3 tran = new Vector3(0f, 0.25f, -0.433f);
				torchfire.localPosition = torch.localPosition + tran;
				Debug.Log("hoop!");
			}
				
		}
		
		
		if (is_left)
		{
			ps.Stop();
			var fo = ps.forceOverLifetime;
			fo.x = new ParticleSystem.MinMaxCurve(2.5f, 3f);
			var main = ps.main;
			main.startRotation = new ParticleSystem.MinMaxCurve(-Mathf.PI / 180 * 40, -Mathf.PI / 180 * 30);
			ps.Play();
			// Debug.Log("is_left");
		}

		if (is_right)
		{
			ps.Stop();
			var fo = ps.forceOverLifetime;
			fo.x = new ParticleSystem.MinMaxCurve(-3f, -2.5f);
			var main = ps.main;
			main.startRotation = new ParticleSystem.MinMaxCurve(Mathf.PI / 180 *30, Mathf.PI / 180 * 50);
			ps.Play();
			// Debug.Log("is_right");
		}
	}

	bool contact(Vector3 a)
	{
		if (a.x<=0.2 && a.x>=-0.6 && a.z >=0.2 && a.z <=0.7)
			return true;
		return false;
	}
	void MouseFollow()
	{
		Vector3 pos = Camera.main.WorldToScreenPoint(torch.localPosition);
		Vector3 m_MousePos = new Vector3(Input.mousePosition.x, 0.4f, 0.01f*Input.mousePosition.y + pos.z);

		Vector3 now = Camera.main.ScreenToWorldPoint(m_MousePos);
		now.y = 0.30f;
		torch.position = now;
		if (contact(now))
			contact_num++;
	}
}
