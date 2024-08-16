using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1f;
    public float movingBoundary = 2.5f;

    private Animator animator;
    private StepSpawn spawn;
    private GameManager gameManager;
    private bool canPut = true;
    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        spawn = GetComponentInChildren<StepSpawn>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.getStart()) return;

        float horizontal = Input.GetAxis("Horizontal");
        
        transform.Translate(Vector3.right*Time.deltaTime*speed*horizontal);

        if(transform.position.x <= -1* movingBoundary + 0.55f)
        {
            setPos(-1 * movingBoundary + 0.55f);
        }
        if(transform.position.x >= movingBoundary + 0.55f) 
        {
            setPos(movingBoundary + 0.55f);
        }

        if (Input.GetButtonDown("Put") && canPut)
        {   
            canPut = false;
            setPopTrue();
            spawn.PutStep();
            //gameManager.spawnStep();
        }
    }

    void setPos(float xVal) {
        Vector3 pos = transform.position;
        pos.x = xVal;
        transform.position = pos;
    }

    public void setPopTrue()
    {
        animator.SetBool("Pop", true);
    }

    public void setPopFalse()
    {
        animator.SetBool("Pop", false);
        canPut = true;
    }


}
