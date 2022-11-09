using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
	[SerializeField]
	private Image cover;
	[SerializeField]
	private Image bomb;
	[SerializeField]
	private Image flag;
	void Start()
	{
		cover.gameObject.SetActive(true);
		bomb.gameObject.SetActive(false);
		flag.gameObject.SetActive(false);
	}

	void Update()
	{

	}
}
