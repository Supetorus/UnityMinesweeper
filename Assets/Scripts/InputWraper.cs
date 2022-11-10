using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputWraper
{
	public static bool GetInputLocationOnRect(RectTransform rect, out Vector2 tapPosition)
	{

		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			Vector2 position;
			if(RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, touch.position, null, out position))
			{
				if (position.x >= rect.rect.xMin && position.x <= rect.rect.xMax &&
					position.y >= rect.rect.yMin && position.y <= rect.rect.yMax)
				{
					position.x -= rect.rect.xMin;
					position.y -= rect.rect.yMin;
					tapPosition = position;
					return true;
				}
			}
		}

		tapPosition = Vector2.zero;
		return false;
	}
}
