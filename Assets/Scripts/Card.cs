using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Card : NetworkBehaviour
{

    public int no;



    const float speed = 4f;
    const float rotateSpeed = 8f;
    private Transform liftedPos = null;
    private Transform destination=null;
    private bool needToRevse = false;

    /// <summary>
    /// step=2 Ready to Move to target
    /// step=0 Ready to Lift
    /// step=1 Ready to rotate 
    /// </summary>
    private int transformStep = 0;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (!isServer) return;
        //To do
        //For specific card -> binding object
        //To perform first lift, second reverse, than interpolated 
        /// step=0 Ready to Lift
        /// step=1 Ready to rotate 
        /// step=2 Ready to Move to target

        //-----------------------------------------------------------------------------------------------
       
        //lift the card
        if (transformStep==0&& liftedPos != null)
        {
            float step = speed * Time.deltaTime;
            gameObject.transform.position = new Vector3(Mathf.Lerp(gameObject.transform.position.x, liftedPos.position.x, step),
                Mathf.Lerp(gameObject.transform.position.y, liftedPos.position.y, step), Mathf.Lerp(gameObject.transform.position.z, liftedPos.position.z, step));
            if(Vector3.Distance(gameObject.transform.position, liftedPos.position)<=0.1f)
            {
                liftedPos = null;
                if(needToRevse)
                    transformStep = 1;
                else
                    transformStep = 2;
            }
        }
        //control the rotation
        else if(transformStep==1&& needToRevse&&destination!=null)
        {
            float step = rotateSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, destination.rotation, step);
            if (Vector3.Distance(gameObject.transform.rotation.eulerAngles, destination.rotation.eulerAngles) <= 0.4f)
            {
                needToRevse = false;
                liftedPos = null;
                transformStep = 2;
            }
        }
        //transform to destination
        if (transformStep == 2 && destination != null)
        {
            float step = speed * Time.deltaTime;
            gameObject.transform.position = new Vector3(Mathf.Lerp(gameObject.transform.position.x, destination.position.x, step),
                Mathf.Lerp(gameObject.transform.position.y, destination.position.y, step), Mathf.Lerp(gameObject.transform.position.z, destination.position.z, step));
            if (Vector3.Distance(gameObject.transform.position, destination.position) <= 0.1f)
            {
                destination = null;
                liftedPos = null;
                needToRevse = false;
                transformStep = 3;
            }
        }
    }


    /// <summary>
    /// Command this card to move to targetTransform
    /// indentify whether need to reverse
    /// if a lift and reversing animation are required
    /// setting toRev true
    /// </summary>
    /// <param name="des"></param>
    /// <param name="toRev"></param>
    public void movetoTarget(Transform liftedDes,Transform targetDes,bool toRev)
    {
        //this.gameObject.transform.parent = targetDes;
        liftedPos = liftedDes;
        destination = targetDes;
        needToRevse = toRev;

        //not to rev Step=2, rev Step=0->1->2
        //if(toRev) transformStep = 0;
        //else transformStep = 2;
        transformStep = 0;

    }


}
