using UnityEngine;

public class PlayerAttacher : MonoBehaviour
{
    //limbs
    public Rigidbody2D torso;
    public Rigidbody2D head;
    public Rigidbody2D armL;
    public Rigidbody2D armR;
    public Rigidbody2D legL;
    public Rigidbody2D legR;

    public Vector3 legLoffset;
    public Vector3 legRoffset;
    public Vector3 torsoOffset;

    //attach target
    public Transform boatPivot;

    public bool doAttach = true;
    public bool attached = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        legLoffset = legL.transform.position - boatPivot.position;
        legRoffset = legR.transform.position - boatPivot.position;
        torsoOffset = torso.transform.position - boatPivot.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (doAttach && !attached)
        {
            //legL.transform.SetParent(boatPivot);
            //legR.transform.SetParent(boatPivot);
            //legL.transform.position = boatPivot.transform.position + legLoffset;
            //legR.transform.position = boatPivot.transform.position + legRoffset;
            //legL.transform.rotation = boatPivot.rotation;
            //legL.transform.rotation = boatPivot.rotation;
            //torso.transform.SetParent(boatPivot, true);
            //torso.constraints = RigidbodyConstraints2D.FreezeAll;
            //torso.transform.position = boatPivot.transform.position + torsoOffset;
            //torso.transform.rotation = boatPivot.rotation;
            //attached = true;
            //legL.MovePositionAndRotation(boatPivot.position + legLoffset, boatPivot.rotation);
            //legR.MovePositionAndRotation(boatPivot.position + legRoffset, boatPivot.rotation);
            //legL.position = boatPivot.position + legLoffset;
            //legR.position = boatPivot.position + legRoffset;
        } else if (!doAttach && attached)
        {
            legL.transform.SetParent(null);
            legR.transform.SetParent(null);
            attached = false;
        }
    }
}
