using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfBoard : MonoBehaviour
{
    public Transform MassCetner = null;
    public float JumpForce = 10f;
    protected int _leftJumpCount = 1;
    protected Rigidbody _rigidbody = null;

    private void Awake()
    {
        this._rigidbody = this.GetComponent<Rigidbody>();
        this._rigidbody.sleepThreshold = 0.0001f;
        this._rigidbody.centerOfMass = MassCetner.localPosition;
        this._rigidbody.maxDepenetrationVelocity = 20f;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.layer != 4) return;
    //    this._rigidbody.useGravity = false;
    //}



    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.layer != 4) return;
    //    this._rigidbody.useGravity = false;
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.layer != 4) return;
    //    this._rigidbody.useGravity = true;
    //}
}
