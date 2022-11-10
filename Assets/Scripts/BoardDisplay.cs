using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardDisplay : MonoBehaviour
{
	public Board board;
	public TileDisplay tilePrefab;
	public GridLayoutGroup gridLayout;

	public Sprite background;
	public Sprite cover;
	public Sprite flag;
	public Sprite bomb;

	void Start()
	{
		gridLayout.cellSize = new Vector2(board.m_TilesSize, board.m_TilesSize);
		for (int i = 0; i < board.m_Width * board.m_Height; i++)
		{
			TileDisplay tile = Instantiate(tilePrefab, gameObject.transform);
			tile.foreground.gameObject.SetActive(false);
			tile.foreground.rectTransform.sizeDelta = new Vector2(board.m_TilesSize, board.m_TilesSize);
			tile.background.GetComponent<Image>().sprite = background;
			tile.background.rectTransform.sizeDelta = new Vector2(board.m_TilesSize, board.m_TilesSize);
		}
	}

	void Update()
	{

	}
}
