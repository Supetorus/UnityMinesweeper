using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Board : MonoBehaviour
{
	struct Tile
	{
		int adjacentMineCount;
		bool isMine;
		bool isCleard;
	}

	int m_Width;
	int m_Height;
	float m_TilesSize = 45f;
	Tile[,] m_Tiles;

	void Start()
	{
		RectTransform t = GetComponent<RectTransform>();
		m_Width = (int)(t.sizeDelta.x / m_TilesSize);
		m_Height = (int)(t.sizeDelta.y / m_TilesSize);

		m_Tiles = new Tile[m_Width,m_Height];

		Debug.Log(string.Format("width: {0}, height: {1}", m_Width, m_Height));
	}

	void Update()
	{

	}
}
