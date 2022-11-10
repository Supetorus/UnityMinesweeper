using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardDisplay : MonoBehaviour
{
	public Board board;
	public Image tilePrefab;
	public GridLayoutGroup gridLayout;

	void Start()
	{
		gridLayout.cellSize = new Vector2(board.m_TilesSize, board.m_TilesSize);
		for (int i = 0; i < board.m_Width * board.m_Height; i++)
		{
			Image tile = Instantiate(tilePrefab, gameObject.transform);
		}
	}

	void Update()
	{

	}
}
