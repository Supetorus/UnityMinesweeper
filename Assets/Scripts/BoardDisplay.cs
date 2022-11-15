using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class BoardDisplay : MonoBehaviour
{
	private RectTransform m_RectTransform;
	private Board m_Board;
	private GridLayoutGroup m_GridLayout;
	private TileDisplay[,] m_Tiles;

	private bool m_IsWon = false;
	private bool m_IsLose = false;

	public TileDisplay m_TilePrefab;
	public Sprite m_Background;
	public Sprite m_Cover;
	public Sprite m_Flag;
	public Sprite m_Bomb;
	public GameObject m_WinScreen;
	public GameObject m_LoseScreen;

	void Start()
	{
		m_RectTransform = GetComponent<RectTransform>();
		m_Board = GetComponent<Board>();
		m_Board.m_OnLose.AddListener(OnLose);
		m_Board.m_OnWin.AddListener(OnWin);
		m_GridLayout = GetComponent<GridLayoutGroup>();
		m_WinScreen.SetActive(false);
		m_LoseScreen.SetActive(false);
		m_GridLayout.cellSize = new Vector2(m_Board.m_TileSize, m_Board.m_TileSize);
		m_Tiles = new TileDisplay[m_Board.m_Width, m_Board.m_Height];
		for (int x = 0; x < m_Board.m_Width; ++x)
		{
			for (int y = 0; y < m_Board.m_Height; ++y)
			{
				m_Tiles[x, y] = Instantiate(m_TilePrefab, gameObject.transform);
				m_Tiles[x, y].foreground.gameObject.SetActive(false);
				m_Tiles[x, y].foreground.rectTransform.sizeDelta = new Vector2(m_Board.m_TileSize, m_Board.m_TileSize);
				m_Tiles[x, y].background.GetComponent<Image>().sprite = m_Cover;
				m_Tiles[x, y].background.rectTransform.sizeDelta = new Vector2(m_Board.m_TileSize, m_Board.m_TileSize);
			}
		}
	}

	private void OnWin()
	{
		m_IsWon = true;
		m_WinScreen.SetActive(true);
	}

	private void OnLose()
	{
		m_IsLose = true;
		m_LoseScreen.SetActive(true);
	}

	void Update()
	{
		Vector2 position;
		if (InputWraper.GetInputLocationOnRect(m_RectTransform, out position))
		{
			if (m_IsLose || m_IsWon)
			{
				m_IsWon = false;
				m_IsLose = false;
				m_WinScreen.SetActive(false);
				m_LoseScreen.SetActive(false);
				m_Board.ResetBoard();
				ResetDraw();
			}
			else
			{
				position = (position / m_RectTransform.sizeDelta) * new Vector2(m_Board.m_Width, m_Board.m_Height);
				m_Board.ClickTile((int)position.x, (int)position.y);
			}
			RedrawBoard();
		}
	}

	void ResetDraw()
	{
		for (int x = 0; x < m_Board.m_Width; ++x)
		{
			for (int y = 0; y < m_Board.m_Height; ++y)
			{
				m_Tiles[x, y].background.sprite = m_Cover;
				m_Tiles[x, y].foreground.gameObject.SetActive(true);
				m_Tiles[x, y].foreground.sprite = null;
				m_Tiles[x, y].foreground.gameObject.SetActive(false);
				m_Tiles[x, y].mineCount.text = "";
			}
		}
	}

	void RedrawBoard()
	{
		for (int x = 0; x < m_Board.m_Width; ++x)
		{
			for (int y = 0; y < m_Board.m_Height; ++y)
			{
				if (!m_Board.m_Tiles[x, y].isCleared)
				{ // is not cleared
					m_Tiles[x, y].background.sprite = m_Cover;
					if (m_Board.m_Tiles[x, y].isFlagged)
					{
						m_Tiles[x, y].foreground.gameObject.SetActive(true);
						m_Tiles[x, y].foreground.sprite = m_Flag;
					}
				}
				else
				{ // is cleared
					m_Tiles[x, y].foreground.sprite = null;
					m_Tiles[x, y].background.sprite = m_Background;
					if (m_Board.m_Tiles[x, y].isMine)
					{
						m_Tiles[x, y].foreground.gameObject.SetActive(true);
						m_Tiles[x, y].foreground.sprite = m_Bomb;
					}
					else if (m_Board.m_Tiles[x, y].adjacentMineCount != 0)
					{
						m_Tiles[x, y].mineCount.text = m_Board.m_Tiles[x, y].adjacentMineCount.ToString();
					}
				}
			}
		}
	}
}