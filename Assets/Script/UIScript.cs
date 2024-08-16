using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    private Animator animator;
    private GameManager gameManager;
    public string type = "start";
    private void Start()
    {
        animator = GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void callStart()
    {
        animator.SetBool("Start", true);
    }

    public void setEnd()
    {
        animator.SetBool("Start", false);
        switch (type)
        {
            case "start":
                gameManager.startGame();
                break;
            case "restart":
                gameManager.restartGame();
                break;
            case "exit":
                gameManager.exitGame();
                break;
        }
    }
}
