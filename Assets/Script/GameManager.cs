using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] stepList;

    public StepSpawn currStepSpawn;
    public StepSpawn nextStepSpawn;

    public GameObject DescriptionCanvas;
    public GameObject GameEndCanvas;
    public GameObject EscapeCanvas;
    public GameObject SettingCanvas;

    public UIScript DescriptionUI;
    public UIScript RestartUI;
    public UIScript ExitUI;

    public TMP_Text scoreTxt;
    public TMP_Text gameEndTxt;
    private int score;

    private int currStepIndex = -1;
    private int nextStepIndex = -1;
    private int index;

    private bool start = false;
    private bool restart = false;
    private bool setting = false;
    // Start is called before the first frame update
    void Start()
    {
        spawnStep();
        //Time.timeScale = 0f;
    }
    private void Update()
    {   
        if(setting && Input.GetButtonDown("Exit")) { resumeGame(); }
        if (restart && Input.GetButtonDown("Exit")) { ExitUI.callStart(); }
        if (start && Input.GetButtonDown("Exit")) { 
            start = false;
            EscapeCanvas.SetActive(true); 
        }

        if (restart && Input.GetButtonDown("Put")) { RestartUI.callStart(); }
        if(!start && Input.GetButtonDown("Put")) { DescriptionUI.callStart(); }
        
        
        if(start && Input.GetKeyDown(KeyCode.F1))
        {
            restart = true;
            start = false;
            //Time.timeScale = 0f;
            gameEndTxt.text = scoreTxt.text;
            GameEndCanvas.SetActive(true);
        }
        
    }

    public void spawnStep()
    {   
        currStepIndex = nextStepIndex == -1 ? Random.Range(0, stepList.Length):nextStepIndex;
        nextStepIndex = Random.Range(0, stepList.Length);

        currStepSpawn.spawnStep(stepList[currStepIndex],index);
        nextStepSpawn.destroyNextStepObjet();
        nextStepSpawn.spawnStep(stepList[nextStepIndex],index);
        index++;
    }
    public bool getStart()
    {
        return start;
    }

    public void startGame()
    {
        start = true;
        DescriptionCanvas.SetActive(false);
    }

    public void resumeGame()
    {
        start = true;
        EscapeCanvas.SetActive(false);
        SettingCanvas.SetActive(false);
    }

    public void restartGame()
    {
        start = true;
        SceneManager.LoadScene(1);
    }

    public void exitGame()
    {
        SceneManager.LoadScene(0);
    }

    public void settingOn()
    {
        EscapeCanvas.SetActive(false);
        SettingCanvas.SetActive(true);
        setting = true;
    }

    public void appendScore(int score)
    {
        this.score += score;
        SetText();
    }

    void SetText()
    {
        scoreTxt.text = "" + score;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other);
        if(other.tag == "Step")
        {
            Step tmp = other.GetComponent<Step>();
            if(tmp != null)
            {
                if (!tmp.getPartPlayArea())
                {
                    tmp.setPartPlayArea(true);
                }
                else
                {
                    restart = true;
                    start = false;
                    //Time.timeScale = 0f;
                    gameEndTxt.text = scoreTxt.text;
                    GameEndCanvas.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Step")
        {
            Step tmp = other.GetComponent<Step>();
            if (tmp != null)
            {
                if (tmp.getPartPlayArea())
                {
                    spawnStep();
                }
                
            }
        }
    }


}
