using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CrossPromoter : MonoBehaviour {
 
    public GameObject[] promotionBtns;
  
    private void OnEnable()
    {
        for (int i = 0; i < promotionBtns.Length; i++)
        {
            promotionBtns[i].SetActive(false);
        }
     int   index = Random.Range(0, promotionBtns.Length);
        promotionBtns[index].SetActive(true);
    }
    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
}
