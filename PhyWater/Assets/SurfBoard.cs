using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfBoard : MonoBehaviour
{
    public Transform MassCetner = null;
    public float JumpForce = 10f;


    protected float _clickTiming = 0;
    protected bool _press = false;
    protected int _leftJumpCount = 3;

    protected Rigidbody _rigidbody = null;

    private void Awake()
    {
        this._rigidbody = this.GetComponent<Rigidbody>();
        this._rigidbody.sleepThreshold = 0.0001f;
        this._rigidbody.centerOfMass = MassCetner.localPosition;
        this._rigidbody.maxDepenetrationVelocity = 5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && this._leftJumpCount > 0)
        {
            this._leftJumpCount -= 1;
            this._rigidbody.velocity = Vector3.zero;
            this._rigidbody.velocity += Vector3.up * this.JumpForce;
        }
        //if (Input.GetMouseButtonDown(0) && this._leftJumpCount > 0)
        //{
        //    this._press = true;
        //    this._leftJumpCount -= 1;
        //    this._clickTiming = 0;
        //}
        //if (Input.GetMouseButtonUp(0) && this._press)
        //{
        //    this._press = false;
        //    this._rigidbody.velocity += Vector3.up * this.JumpForce * (this._clickTiming / 0.1f);
        //    this._clickTiming = 0;
        //    return;
        //}
        //if (this._press)
        //{
        //    this._clickTiming += Time.deltaTime;
        //}
        //if (this._clickTiming > 0.1f && this._press)
        //{
        //    this._press = false;
        //    this._rigidbody.velocity += Vector3.up * this.JumpForce;
        //    this._clickTiming = 0;
        //}
    }

    private void FixedUpdate()
    {
        if (null != PhyWater.Instance)
        {
            Vector3 pos = PhyWater.Instance.CalculationWorldPos(this.transform.position);
            if (this.transform.position.y < pos.y - 0.05f)
            {
                this.transform.position = pos;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 4) return;
        this._leftJumpCount = 3;
    }

}
