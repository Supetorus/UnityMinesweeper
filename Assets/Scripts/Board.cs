using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Board : MonoBehaviour
{
	public struct Tile
	{
		int adjacentMineCount;
		bool isMine;
		bool isCleared;
		bool isFlagged;
	}

	public int m_Width;
	public int m_Height;
	public float m_TilesSize = 45f;
	public Tile[,] m_Tiles{ get; private set; }

	void Start()
	{
		RectTransform t = GetComponent<RectTransform>();
		m_Width = (int)(t.sizeDelta.x / m_TilesSize);
		m_Height = (int)(t.sizeDelta.y / m_TilesSize);

		GenerateTiles();

		Debug.Log(string.Format("width: {0}, height: {1}", m_Width, m_Height));
	}

	void Update()
	{

	}

	void GenerateTiles()
	{
		m_Tiles = new Tile[m_Width, m_Height];

		int mineCount = m_Width * m_Height / 5;


	}
}
