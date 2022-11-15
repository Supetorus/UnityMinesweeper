using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Board : MonoBehaviour
{
	public struct Tile
	{
		public int adjacentMineCount;
		public bool isMine;
		public bool isCleared;
		public bool isFlagged;
	}

	public int m_Width { get; private set; }
	public int m_Height { get; private set; }
	public Tile[,] m_Tiles { get; private set; }
	public float m_TilesSize { get; private set; }
	public UnityEvent m_OnLose { get; private set; }
	public UnityEvent m_OnWin { get; private set; }

	private System.Random rng;
	private int m_TileCount;
	private int m_ClearedTileCount;
	private int m_MineTileCount;
	private bool m_FirstClick;

	private void Awake()
	{
		m_OnLose = new UnityEvent();
		m_OnWin = new UnityEvent();
		rng = new System.Random();
		RectTransform t = GetComponent<RectTransform>();
		m_TilesSize = t.sizeDelta.x / 8.0f;
		m_Width = (int)(t.sizeDelta.x / m_TilesSize);
		m_Height = (int)(t.sizeDelta.y / m_TilesSize);
		m_TileCount = m_Width * m_Height;
		m_ClearedTileCount = 0;

		GenerateTiles();
	}

	private void Update()
	{
#if UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.W))
		{
			for (int x = 0; x < m_Width; ++x)
			{
				for (int y = 0; y < m_Width; ++y)
				{
					m_Tiles[x, y].isCleared = !m_Tiles[x, y].isMine;
				}
			}

			m_ClearedTileCount = m_TileCount - m_MineTileCount;
			m_OnWin.Invoke();
		}
#endif
	}

	private void GenerateTiles()
	{
		m_Tiles = new Tile[m_Width, m_Height];
		m_FirstClick = true;

		int mineCount = m_Width * m_Height / 5;
		m_MineTileCount = mineCount;

		while (mineCount > 0)
		{
			int x = rng.Next(0, m_Width);
			int y = rng.Next(0, m_Height);

			if (!m_Tiles[x, y].isMine)
			{
				m_Tiles[x, y].isMine = true;
				--mineCount;

				if (x > 0)
				{
					++m_Tiles[x - 1, y].adjacentMineCount;
					if (y > 0) { ++m_Tiles[x - 1, y - 1].adjacentMineCount; }
					if (y < m_Height - 1) { ++m_Tiles[x - 1, y + 1].adjacentMineCount; }
				}

				if (x < m_Width - 1)
				{
					++m_Tiles[x + 1, y].adjacentMineCount;
					if (y > 0) { ++m_Tiles[x + 1, y - 1].adjacentMineCount; }
					if (y < m_Height - 1) { ++m_Tiles[x + 1, y + 1].adjacentMineCount; }
				}

				if (y > 0) { ++m_Tiles[x, y - 1].adjacentMineCount; }
				if (y < m_Height - 1) { ++m_Tiles[x, y + 1].adjacentMineCount; }
			}
		}
	}

	public void ClickTile(int x, int y)
	{
		if(m_FirstClick)
		{
			m_FirstClick = false;

			//Move mines around here elsewhere
			if(m_Tiles[x, y].adjacentMineCount > 0)
			{
				
			}
		}

		if (m_Tiles[x, y].isMine && !m_Tiles[x, y].isFlagged)
		{
			m_Tiles[x, y].isCleared = true;
			m_OnLose.Invoke();
		}
		else if (!m_Tiles[x, y].isCleared && !m_Tiles[x, y].isFlagged)
		{
			if (m_Tiles[x, y].adjacentMineCount == 0)
			{
				Stack<Vector2Int> tiles = new Stack<Vector2Int>();
				tiles.Push(new Vector2Int(x, y));

				while (tiles.Count > 0)
				{
					Vector2Int pos = tiles.Pop();

					if (pos.x >= 0 && pos.y >= 0 && pos.x < m_Width && pos.y < m_Height && !m_Tiles[pos.x, pos.y].isCleared)
					{
						m_Tiles[pos.x, pos.y].isCleared = true;
						m_Tiles[pos.x, pos.y].isFlagged = false;
						++m_ClearedTileCount;

						if (m_Tiles[pos.x, pos.y].adjacentMineCount == 0)
						{
							tiles.Push(new Vector2Int(pos.x - 1, pos.y));
							tiles.Push(new Vector2Int(pos.x + 1, pos.y));
							tiles.Push(new Vector2Int(pos.x, pos.y - 1));
							tiles.Push(new Vector2Int(pos.x, pos.y + 1));
							tiles.Push(new Vector2Int(pos.x - 1, pos.y - 1));
							tiles.Push(new Vector2Int(pos.x + 1, pos.y + 1));
							tiles.Push(new Vector2Int(pos.x - 1, pos.y + 1));
							tiles.Push(new Vector2Int(pos.x + 1, pos.y - 1));
						}
					}
				}
			}
			else
			{
				m_Tiles[x, y].isCleared = true;
				++m_ClearedTileCount;
			}

			if (m_ClearedTileCount == m_TileCount - m_MineTileCount)
			{
				m_OnWin.Invoke();
			}
		}
	}

	public void ToggleFlag(int x, int y)
	{
		m_Tiles[x, y].isFlagged = !m_Tiles[x, y].isFlagged;
	}
}
