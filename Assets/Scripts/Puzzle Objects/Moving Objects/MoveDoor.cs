using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDoor : MonoBehaviour
{
    public bool isclosed = false;
    //where the object starts
    double ypos;
    public double heighty;
    //bools that control if method is called
    public bool calldoordown = false;
    public bool calldoorup = false;


    // Start is called before the first frame update
    void Start()
    {//takes the position
        ypos = this.transform.position.y;
        heighty = transform.lossyScale.y;
    }

    // Update is called once per frame
    void Update()
    {//used to find if you want the door open or not
        if (calldoordown)
        {
            DoorDown();
            calldoordown = false;
        }
        if (calldoorup)
        {
            DoorUp();
            calldoorup = false;
        }
    }
    public void DoorDown()
    {
        StartCoroutine(DoorDownCR());



        isclosed = true;
    }
    public IEnumerator DoorDownCR()
    {
        if (ypos < 0)
        {
            while (gameObject.transform.position.y > (ypos - heighty))
            {
                yield return new WaitForSeconds(.009f);

                transform.Translate(0f, -.07f, 0f);

            }
        }
        else
        {
            while (gameObject.transform.position.y > (ypos - heighty))
            {
                yield return new WaitForSeconds(.009f);
                transform.Translate(0f, -.07f, 0f);


            }


        }
        //updating y position
        ypos = gameObject.transform.position.y;

    }


    public void DoorUp()
    {
        StartCoroutine(DoorUpCR());

        isclosed = false;

    }
    public IEnumerator DoorUpCR()
    {

        while (gameObject.transform.position.y < (ypos + heighty))
        {
            yield return new WaitForSeconds(.009f);



            transform.Translate(0f, +.07f, 0f);

        }
        //updating y position
        ypos = gameObject.transform.position.y;

    }
}
