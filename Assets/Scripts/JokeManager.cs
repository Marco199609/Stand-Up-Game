using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class JokeManager : MonoBehaviour
{
    [SerializeField] private GameObject[] jokePages;
    [SerializeField] private JokeData jokeSelected;


    [SerializeField] private CrowdManager crowdManager;

    private void OnEnable()
    {
        PlayerController.OnJokeSelected += SetSelectedJoke;   
    }

    private void OnDisable()
    {
        PlayerController.OnJokeSelected -= SetSelectedJoke;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            jokeSelected = null;
        }

        if(Input.GetMouseButtonDown(0))
        {
            if(jokeSelected != null)
            {
                crowdManager.CrowdResponse(jokeSelected.JokeQuality);
            }
        }
    }

    private void SetSelectedJoke(JokeData jokeSelected)
    {
        this.jokeSelected = jokeSelected;
    }
}
