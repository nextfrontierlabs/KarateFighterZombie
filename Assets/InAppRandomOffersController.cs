using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class InAppRandomOffersController : MonoBehaviour {

    public static event Action OnClosePanels;
    public static void DoOnClosePanels() => OnClosePanels?.Invoke();
    public static event Action OnShowPanels;
    public static void DoOnShowPanels() => OnShowPanels?.Invoke();
    public static bool cameFromFight  = true;
    public string[] offers = { "com.rockville.games.unlock.all" } ;
    public GameObject[] offerPanels;
    public List<GameObject> availableOffers;
    public void OnEnable()
    {
        InAppRandomOffersController.OnClosePanels += InAppRandomOffersController_OnClosePanels;
        InAppRandomOffersController.OnShowPanels += InAppRandomOffersController_OnShowPanels;
        InAppRandomOffersController_OnShowPanels();
    }

    private void InAppRandomOffersController_OnShowPanels()
    {
        PopulateAvailableOffers();
        if (cameFromFight)
        {
            Invoke("PopRandomOffer", 1.0f);
            cameFromFight = false;
        }
    }

    private void InAppRandomOffersController_OnClosePanels()
    {
        ClosePanels();
    }
    private void PopulateAvailableOffers()
    {
        availableOffers.Clear();

        for (int i = 0; i < offers.Length; i++)
        {
            if(PlayerPrefs.GetInt( offers[i])==0)
            {
                availableOffers.Add(offerPanels[i]);
            }
        }
    }
    void PopRandomOffer()
    {
        if (availableOffers.Count < 1)
        {
            return;
        }
        for (int i = 0; i < availableOffers.Count; i++)
        {
            availableOffers[i].SetActive(false);
        }
        availableOffers[UnityEngine.Random.Range(0, availableOffers.Count)].SetActive(true);
    }
    private void ClosePanels()
    {
        for (int i = 0; i < offerPanels.Length; i++)
        {
            offerPanels[i].SetActive(false);
        }
    }
    public void BtnClosePanels()
    {
        for (int i = 0; i < offerPanels.Length; i++)
        {
            offerPanels[i].SetActive(false);
        }
        
    }
    void OnDisable()
    {
        InAppRandomOffersController.OnClosePanels -= InAppRandomOffersController_OnClosePanels;
        InAppRandomOffersController.OnShowPanels -= InAppRandomOffersController_OnShowPanels;
    }
}
