using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAndPowerMoveCollector : MonoBehaviour {

	public TouchController touchController;
	private TouchZone [] touchZones ;
	int count = 0; 
	private static SpecialAndPowerMoveCollector specialPowerMove;
	public static SpecialAndPowerMoveCollector GetInstance()
	{
		return specialPowerMove;
	}
	// Use this for initialization
	void Awake()
	{
		specialPowerMove = this;
		touchController = GetComponent<TouchController> ();
		touchZones = touchController.touchZones;
		touchController.InitController ();
	}
	void OnEnable ()
	{
		count++;

		float x = 0.0f;
	
		if (count > 1) {
			for (int i = 0; i < touchZones.Length; i++) {
				if (PlayerPrefs.HasKey (touchZones [i].name)) {
					if (touchZones [i].name.Contains ("Special Move")) {
						touchZones [i].disableGui = false;
						touchZones [i].enableGetButton = true; 
						Vector2 newPos = touchZones [i].posCm;
						newPos = new Vector2(x, 1f);
						x = x + 1f;
						touchZones [i].posCm = newPos;
					}
				} else {
					if (touchZones [i].name.Contains ("Special Move")) {
						touchZones [i].disableGui = true;
						touchZones [i].enableGetButton = false; 
						Vector2 newPos = touchZones [i].posCm;
						newPos = new Vector2(0f, 0f);

						touchZones [i].posCm = newPos;
					}
				}

			}
			for (int i = 0; i < touchZones.Length; i++) {
				if (PlayerPrefs.HasKey (touchZones [i].name)) {
					if (touchZones [i].name.Contains ("Power Move")) {
						touchZones [i].disableGui = false;
						touchZones [i].enableGetButton = true;			
					}
				} else {
					if (touchZones [i].name.Contains ("Power Move")) {
						touchZones [i].disableGui = true;
						touchZones [i].enableGetButton = false; 
					}
				}
			}
			if (UFE.gameMode == GameMode.TrainingRoom) {
				for (int i = 0; i < touchZones.Length; i++) {
					if (touchZones [i].name.Contains ("Power Move")) {
						touchZones [i].disableGui = false;
						touchZones [i].enableGetButton = true;	
					
					}
					if (touchZones [i].name.Contains ("Special Move1")) {
						touchZones [i].disableGui = false;
						touchZones [i].enableGetButton = true;	
						Vector2 newPos = touchZones [i].posCm;
						newPos = new Vector2(0, 1f);

						touchZones [i].posCm = newPos;
					}
					if (touchZones [i].name.Contains ("Special Move2")) {
						touchZones [i].disableGui = false;
						touchZones [i].enableGetButton = true;	
						Vector2 newPos = touchZones [i].posCm;
						newPos = new Vector2(1, 1f);

						touchZones [i].posCm = newPos;
					}
					if (touchZones [i].name.Contains ("Special Move3")) {
						touchZones [i].disableGui = false;
						touchZones [i].enableGetButton = true;	
						Vector2 newPos = touchZones [i].posCm;
						newPos = new Vector2(2, 1f);

						touchZones [i].posCm = newPos;
					}
				}
			}


		}


		touchController.InitController ();
	}


}
