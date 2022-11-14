using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardDisplay : MonoBehaviour
{
	private RectTransform m_RectTransform;
	private Board m_Board;
	private GridLayoutGroup m_GridLayout;
	private TileDisplay[,] m_tiles;

	public TileDisplay m_TilePrefab;
	public Sprite m_Background;
	public Sprite m_Cover;
	public Sprite m_Flag;
	public Sprite m_Bomb;

	void Start()
	{
		m_RectTransform = GetComponent<RectTransform>();
		m_Board = GetComponent<Board>();
		m_GridLayout = GetComponent<GridLayoutGroup>();
		m_GridLayout.cellSize = new Vector2(m_Board.m_TilesSize, m_Board.m_TilesSize);
		m_tiles = new TileDisplay[m_Board.m_Width, m_Board.m_Height];
		for (int x = 0; x < m_Board.m_Width; ++x)
		{
			for (int y = 0; y < m_Board.m_Height; ++y)
			{
				m_tiles[x, y] = Instantiate(m_TilePrefab, gameObject.transform);
				m_tiles[x, y].foreground.gameObject.SetActive(false);
				m_tiles[x, y].foreground.rectTransform.sizeDelta = new Vector2(m_Board.m_TilesSize, m_Board.m_TilesSize);
				m_tiles[x, y].background.GetComponent<Image>().sprite = m_Cover;
				m_tiles[x, y].background.rectTransform.sizeDelta = new Vector2(m_Board.m_TilesSize, m_Board.m_TilesSize);
			}
		}
	}

	void Update()
	{
		Vector2 position;
		if (InputWraper.GetInputLocationOnRect(m_RectTransform, out position))
		{
			position = (position / m_RectTransform.sizeDelta) * new Vector2(m_Board.m_Width, m_Board.m_Height);
			m_Board.ClickTile((int)position.x, (int)position.y);
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
					m_tiles[x, y].background.sprite = m_Cover;
					if (m_Board.m_Tiles[x, y].isFlagged)
					{
						m_tiles[x, y].foreground.gameObject.SetActive(true);
						m_tiles[x, y].foreground.sprite = m_Flag;
					}
				}
				else
				{ // is cleared
					m_tiles[x, y].foreground.sprite = null;
					m_tiles[x, y].background.sprite = m_Background;
					if (m_Board.m_Tiles[x, y].isMine)
					{
						m_tiles[x, y].foreground.gameObject.SetActive(true);
						m_tiles[x, y].foreground.sprite = m_Bomb;
					}
					else if (m_Board.m_Tiles[x, y].adjacentMineCount != 0)
					{
						m_tiles[x, y].mineCount.text = m_Board.m_Tiles[x, y].adjacentMineCount.ToString();
					}
				}
			}
		}
	}
}