using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step : MonoBehaviour
{
    private bool partPlayArea = false;

    public int step;
    public int score;
    public GameObject NextStep;
    
    private int index;
    private GameManager gameManager;
    private bool collide = false;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void setPartPlayArea(bool partPlayArea)
    {
        this.partPlayArea = partPlayArea;
    }

    public bool getPartPlayArea(){ 
        return partPlayArea;
    }

    public int getStep()
    {
        return step;
    }

    public int getIndex()
    {
        return index;
    }
    public void setIndex(int index)
    {
        this.index = index;
    }

    public bool isCollide()
    {
        return collide;
    }
    public void setCollide(bool collide) { 
        this.collide = collide;
    }

    public void PlaySound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Step")
        {
            GameObject otherStep = collision.gameObject;
            //Debug.Log(otherStep.GetComponent<Step>().isCollide());
            if(step == otherStep.GetComponent<Step>().getStep() && !otherStep.GetComponent<Step>().isCollide())
            {
                if (index > otherStep.GetComponent<Step>().getIndex()) return;
                if (NextStep == null) return;
                Vector2 middlePoint = (transform.position+otherStep.transform.position)/2;
                otherStep.GetComponent<Step>().setCollide(true);
                Destroy(otherStep);
                //Debug.Log(step + ", "+index+", " + NextStep);

                GameObject tmp = Instantiate(NextStep, new Vector3(middlePoint.x,middlePoint.y,1f), transform.rotation, gameManager.transform);
                tmp.GetComponent<Rigidbody2D>().gravityScale = 1;
                tmp.GetComponent<Step>().setIndex(index);
                tmp.GetComponent<Step>().PlaySound();
                tmp.GetComponent<Step>().setPartPlayArea(true);
                gameManager.appendScore(score);
                Destroy(gameObject);
            }
        }
    }
}
