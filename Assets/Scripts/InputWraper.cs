using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputWraper
{
	public static bool GetInputLocationOnRect(RectTransform rect, out Vector2 tapPosition)
	{
		bool isClicked = Input.GetMouseButtonDown((int)MouseButton.Left);
		bool isTouch = Input.touchCount > 0;

		if (isClicked || isTouch)
		{
			Vector2 touchPosition = isClicked ? (Vector2)Input.mousePosition : Input.GetTouch(0).position;
			Vector2 position;
			if(RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, touchPosition, null, out position))
			{
				if (position.x >= rect.rect.xMin && position.x <= rect.rect.xMax &&
					position.y >= rect.rect.yMin && position.y <= rect.rect.yMax)
				{
					position.x -= rect.rect.xMin;
					position.y = rect.rect.yMax - position.y;
					tapPosition = position;
					return true;
				}
			}
		}

		tapPosition = Vector2.zero;
		return false;
	}
}
