using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSpawn : MonoBehaviour
{    
    private GameObject currStep;
    private GameObject GameManager;
    // Start is called before the first frame update
    private void Start()
    {
        GameManager = GameObject.Find("GameManager");
    }


    public void spawnStep(GameObject step, int index)
    {
        currStep = Instantiate(step, transform);
        Step tmp = currStep.GetComponent<Step>();
        if(tmp != null)
        {
            tmp.setIndex(index);
        }
    }

    public void PutStep()
    {
        currStep.GetComponent<Rigidbody2D>().gravityScale = 1;
        currStep.transform.parent = GameManager.transform;
    }
    public void destroyNextStepObjet()
    {
        if (currStep == null) return;
        Destroy(currStep);
    }

}
