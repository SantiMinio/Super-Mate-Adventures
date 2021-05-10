using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frano
{
    public class CharController : MonoBehaviour
    {

        [SerializeField] private float moveSpeed = 4f;

        private Vector3 forward, right;
        private Rigidbody _rb;

        private bool moving;
    
        void Start()
        {
            _rb = GetComponent<Rigidbody>();
        
            forward = Camera.main.transform.forward;
            forward.y = 0;
            forward = Vector3.Normalize(forward);
            right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
        }

        void Update()
        {
            moving = false;
        
            if (Input.anyKey)
                moving = true;
        }

        private void FixedUpdate()
        {
            if(moving)
                Move();
        }

        private void Move()
        {
            Vector3 direction = new Vector3(Input.GetAxis("HorizontalKey"), 0, Input.GetAxis(("VerticalKey")));
            Vector3 rightMovement = right * moveSpeed * Time.deltaTime * Input.GetAxis("HorizontalKey");
            Vector3 upMovement = forward * moveSpeed * Time.deltaTime * Input.GetAxis("VerticalKey");

            Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

            _rb.velocity = heading  * moveSpeed;
        
            transform.forward = heading;
        }
    }
}

