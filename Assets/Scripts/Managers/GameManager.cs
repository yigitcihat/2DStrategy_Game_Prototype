using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public Transform sourceUnit;
    public Vector2 destinationIndex;
    public GameObject aStar;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

    }
    // set the route
    public void HandleMovement()
    {
        aStar.GetComponent<AStar>().player = sourceUnit;
        aStar.GetComponent<AStar>().targetIndex = destinationIndex;
        aStar.GetComponent<AStar>().hasTarget = true;
    }

    
}
