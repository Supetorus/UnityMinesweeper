using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class Board : MonoBehaviour
{
	public struct Tile
	{
		public int adjacentMineCount;
		public bool isMine;
		public bool isCleared;
		public bool isFlagged;
	}

	public int m_Width;
	public int m_Height;
	public float m_TilesSize = 45f;
	public Tile[,] m_Tiles{ get; private set; }

	private System.Random rng;

	void Awake()
	{
		rng = new System.Random();
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

		while(mineCount > 0)
		{
			int x = rng.Next(0, m_Width);
			int y = rng.Next(0, m_Height);

			if(!m_Tiles[x, y].isMine)
			{
				m_Tiles[x, y].isMine = true;

				if(x > 0)
				{
					++m_Tiles[x - 1, y].adjacentMineCount;
					if (y > 0) 
					{ 
						++m_Tiles[x - 1, y - 1].adjacentMineCount; 
						++m_Tiles[x, y - 1].adjacentMineCount; 
					}
					if (y < m_Height - 1) 
					{ 
						++m_Tiles[x - 1, y + 1].adjacentMineCount;
						++m_Tiles[x, y + 1].adjacentMineCount;
					}
				}

				if(x < m_Width - 1)
				{
					++m_Tiles[x + 1, y].adjacentMineCount;
					if (y > 0) { ++m_Tiles[x + 1, y - 1].adjacentMineCount; }
					if (y < m_Height - 1) { ++m_Tiles[x + 1, y + 1].adjacentMineCount; }
				}

				--mineCount;
			}
		}
	}
}
