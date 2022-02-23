using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentAnimator : MonoBehaviour {

	// Use this for initialization
	public RectTransform rectTransform;
	public float anchorMax = 0f;
	public float anchorMin = 0f;
	public void ChangeRectTransform()
    {
		rectTransform.anchoredPosition = new Vector2(0, anchorMax);
		//rectTransform.anchorMin = new Vector2(0, -anchorMin);

	}
}
