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
	public Color[] colors = new Color[8];

	void Start()
	{
		// Setup component references
		m_RectTransform = GetComponent<RectTransform>();
		m_Board = GetComponent<Board>();
		m_GridLayout = GetComponent<GridLayoutGroup>();

		// Event Listeners
		m_Board.m_OnLose.AddListener(OnLose);
		m_Board.m_OnWin.AddListener(OnWin);

		// Miscellaneous
		m_GridLayout.cellSize = new Vector2(m_Board.m_TileSize, m_Board.m_TileSize);

		GenerateBoard();
		Reset();
	}

	private void GenerateBoard()
	{
		m_Tiles = new TileDisplay[m_Board.m_Width, m_Board.m_Height];
		for (int x = 0; x < m_Board.m_Width; ++x)
		{
			for (int y = 0; y < m_Board.m_Height; ++y)
			{
				ref TileDisplay tile = ref m_Tiles[x, y];
				tile = Instantiate(m_TilePrefab, gameObject.transform);
				tile.foreground.rectTransform.sizeDelta = new Vector2(m_Board.m_TileSize, m_Board.m_TileSize);
				tile.background.rectTransform.sizeDelta = new Vector2(m_Board.m_TileSize, m_Board.m_TileSize);
				//m_Tiles[x, y].foreground.gameObject.SetActive(false);
				//m_Tiles[x, y].background.GetComponent<Image>().sprite = m_Cover;
			}
		}
	}

	void Reset()
	{
		for (int x = 0; x < m_Board.m_Width; ++x)
		{
			for (int y = 0; y < m_Board.m_Height; ++y)
			{
				ref TileDisplay tile = ref m_Tiles[x, y];
				tile.background.gameObject.SetActive(true);
				tile.background.sprite = m_Cover;
				tile.foreground.gameObject.SetActive(false);
				tile.foreground.sprite = null;
				tile.mineCount.text = "";
				//tile.mineCount.color = Color.black;
			}
		}
		m_IsWon = false;
		m_IsLose = false;
		m_WinScreen.SetActive(false);
		m_LoseScreen.SetActive(false);
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
		bool isHeld;
		if (InputWraper.GetInputLocationOnRect(m_RectTransform, out position, out isHeld))
		{
			if (m_IsLose || m_IsWon)
			{
				m_Board.ResetBoard();
				Reset();
			}
			else
			{
				position = (position / m_RectTransform.sizeDelta) * new Vector2(m_Board.m_Width, m_Board.m_Height);
				if (isHeld)
					m_Board.ToggleFlag((int)position.x, (int)position.y);
				else
					m_Board.ClickTile((int)position.x, (int)position.y);
			}
			RedrawBoard();
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
					m_Tiles[x, y].foreground.gameObject.SetActive(false);
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
						m_Tiles[x, y].mineCount.color = colors[m_Board.m_Tiles[x, y].adjacentMineCount - 1];
					}
				}
			}
		}
	}
}