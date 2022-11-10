using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tapTest : MonoBehaviour
{

	RectTransform m_Rect;
	// Start is called before the first frame update
	void Start()
	{
		m_Rect = GetComponent<RectTransform>();
	}

	// Update is called once per frame
	void Update()
	{
		Vector2 position;
		if(InputWraper.GetInputLocationOnRect(m_Rect, out position))
		{
			Debug.Log(position);
		}
	}
}
