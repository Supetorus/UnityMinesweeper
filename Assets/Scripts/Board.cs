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
	private int m_TileCount;
	private int m_ClearedTileCount;
	private int m_MineTileCount;

	private void Awake()
	{
		rng = new System.Random();
		RectTransform t = GetComponent<RectTransform>();
		m_Width = (int)(t.sizeDelta.x / m_TilesSize);
		m_Height = (int)(t.sizeDelta.y / m_TilesSize);
		m_TileCount = m_Width * m_Height;
		m_ClearedTileCount = 0;

		GenerateTiles();

		Debug.Log(string.Format("width: {0}, height: {1}", m_Width, m_Height));
	}

	private void GenerateTiles()
	{
		m_Tiles = new Tile[m_Width, m_Height];

		int mineCount = m_Width * m_Height / 5;
		m_MineTileCount = mineCount;

		while(mineCount > 0)
		{
			int x = rng.Next(0, m_Width);
			int y = rng.Next(0, m_Height);

			if(!m_Tiles[x, y].isMine)
			{
				m_Tiles[x, y].isMine = true;
				--mineCount;

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

			}
		}
	}

	public void ClickTile(int x, int y)
	{
		Tile tile = m_Tiles[x, y];

		if(tile.isMine && !tile.isFlagged)
		{
			//TODO: Visual change in tile
			//TODO: Lose popup
			Debug.Log("You Lose!");
		}
		else if(!tile.isCleared && !tile.isFlagged)
		{
			if (tile.adjacentMineCount == 0)
			{
				Stack<Vector2Int> tiles = new Stack<Vector2Int>();
				tiles.Push(new Vector2Int(x, y));

				while (tiles.Count > 0)
				{
					Vector2Int pos = tiles.Pop();

					if (pos.x > 0 && pos.y > 0 && pos.x < m_Width - 1 && pos.y < m_Height - 1 && !m_Tiles[pos.x, pos.y].isCleared)
					{
						//TODO: Visual change in tiles
						m_Tiles[pos.x, pos.y].isCleared = true;
						++m_ClearedTileCount;

						if (m_Tiles[pos.x, pos.y].adjacentMineCount == 0)
						{
							tiles.Push(new Vector2Int(pos.x - 1, pos.y));
							tiles.Push(new Vector2Int(pos.x + 1, pos.y));
							tiles.Push(new Vector2Int(pos.x, pos.y - 1));
							tiles.Push(new Vector2Int(pos.x, pos.y + 1));
						}
					}
				}
			}
			else
			{
				//TODO: Visual change in tile
				tile.isCleared = true;
				++m_ClearedTileCount;
			}

			if(m_ClearedTileCount == m_TileCount - m_MineTileCount)
			{
				//TODO: Win popup
				Debug.Log("You Win!");
			}
		}
	}

	public void ToggleFlag(int x, int y)
	{
		//TODO: Visual change in tile
		m_Tiles[x, y].isFlagged = !m_Tiles[x, y].isFlagged;
	}
}
