using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardDisplay : MonoBehaviour
{
	private RectTransform m_RectTransform;
	private Board m_Board;
	private GridLayoutGroup m_GridLayout;

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
		for (int i = 0; i < m_Board.m_Width * m_Board.m_Height; i++)
		{
			TileDisplay tile = Instantiate(m_TilePrefab, gameObject.transform);
			tile.foreground.gameObject.SetActive(false);
			tile.foreground.rectTransform.sizeDelta = new Vector2(m_Board.m_TilesSize, m_Board.m_TilesSize);
			tile.background.GetComponent<Image>().sprite = m_Background;
			tile.background.rectTransform.sizeDelta = new Vector2(m_Board.m_TilesSize, m_Board.m_TilesSize);
		}
	}

	void Update()
	{
		Vector2 position;
		if(InputWraper.GetInputLocationOnRect(m_RectTransform, out position))
		{
			position = (position / m_RectTransform.sizeDelta) * new Vector2(m_Board.m_Width, m_Board.m_Height);
			m_Board.ClickTile((int)position.x, (int)position.y);
			RedrawBoard();
		}
	}

	void RedrawBoard()
	{
		// TODO
	}
}
