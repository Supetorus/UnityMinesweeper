using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputWraper
{

	
	public static bool GetInputLocationOnRect(RectTransform rect, out Vector2 tapPosition, out bool isHeld)
	{
		bool isClicked = Input.GetMouseButtonDown((int)MouseButton.Left);
		bool isRightClicked = Input.GetMouseButtonDown((int)MouseButton.Right);
		bool isTouch = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
		isHeld = false;

		if (isRightClicked) isHeld = true;
		if (isTouch && Input.GetTouch(0).deltaTime >= 1.0f / 20.0f) isHeld = true;


		if (isClicked || isRightClicked || isTouch)
		{
			Vector2 touchPosition = isTouch ? Input.GetTouch(0).position :(Vector2)Input.mousePosition;
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
