using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public Transform[] _target;
    private float _targetTimer;
    private int curTar = 0;
    private Animator anin; // animator character
    public Rigidbody2D rb;  // body
    public float speed;  // character speed
    public Vector2 moveInput;  // direction vector
    public Vector2 moveVelocity;  // velocity of the direction vector
    private Vector2 targetVelocity; // target speed
    private bool facingRight = true; // the character looks to the right1
    private bool _isBusy = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // finding our body
        anin = GetComponent<Animator>();  // we get an animator
    }


    void NextTarget()
    {
        curTar += 1;
        if (curTar > _target.Length - 1)
        {
            curTar = 0;
        }
    }

    void Update()
    {
        _targetTimer += Time.deltaTime;
        if (_targetTimer > 3)
        {
            _targetTimer = 0;
            NextTarget();
        }

        {
            moveInput = _target[curTar].transform.position - this.transform.position;  // find out the direction vector
            moveVelocity = moveInput.normalized * speed; // multiply the speed by the vector
            //moveVelocity.y = 1;
            //moveVelocity.y = 0;
        }
        if (moveInput.x == 0)  // if the player's position on X is 0
        {
            anin.SetBool("isRuning", false);  // then the running animation is turned off
        }

        else  // in any other case
        {
            anin.SetBool("isRuning", true);  // running animation is enabled
        }

        if (!facingRight && moveInput.x > 0) // if we are not looking to the right and the direction vector on X is greater than 0
        {
            Flip();  // that turn
        }

        else if (facingRight && moveInput.x < 0)  // if we look to the right and the direction vector on X is less than 0
        {
            Flip();  // that turn
        }

    }

    void FixedUpdate()
    {
        if (_isBusy)
        {
            return;
        }
        targetVelocity = Vector2.Lerp(targetVelocity, moveVelocity, 1f);  // interpolate the speed
        rb.MovePosition(rb.position + targetVelocity * Time.fixedDeltaTime);  // moving our body
    }

    private void Flip()
    {
        if (_isBusy)
        {
            return;
        }
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }


    private void OnTriggerEnter2D(Collider2D other)  // Когда объект входит в зону тригера
    {
        if (other.tag == "Fall")  // Если астероид сталкивается с объектами имеющим тег GameBoundary, Drop
        {
            moveInput = Vector2.zero;
            anin.SetTrigger("Fall");
            _isBusy = true;
        }
    }

    public void SetBusyFalse()
    {
        _isBusy = false;
    }
}

