using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frano
{
    public class CharController : MonoBehaviour
    {

        [SerializeField] private float moveSpeed = 4f;
        float initialSpeed;

        private Vector3 forward, right;
        private Rigidbody _rb;

        private bool moving;
        private CharacterHead _characterHead;

        public bool Moving => moving;


        public bool inputAvaliable { get; private set; }


        public void InputsOn() => inputAvaliable = true;
        public void InputsOff() => inputAvaliable = false;
        
        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _characterHead = GetComponent<CharacterHead>();
            
            
            forward = Camera.main.transform.forward;
            forward.y = 0;
            forward = Vector3.Normalize(forward);
            right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

            initialSpeed = moveSpeed;
            
            InputsOn();
        }

        void Update()
        {
            if (!inputAvaliable) return;
            
            if (Input.GetKeyDown(KeyCode.Space))
                _characterHead.BaseAttack();
            
            
            //Moving
            moving = false;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) moving = true;
        }

        private void FixedUpdate()
        {
            if(moving)
                Move();
        }

        private void Move()
        {
            Vector3 direction = new Vector3(Input.GetAxis("HorizontalKey"), 0, Input.GetAxis(("VerticalKey")));
            Vector3 rightMovement = right * moveSpeed * Time.deltaTime * Input.GetAxisRaw("HorizontalKey");
            Vector3 upMovement = forward * moveSpeed * Time.deltaTime * Input.GetAxisRaw("VerticalKey");

            Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

            _rb.velocity = heading  * moveSpeed;
        
            
            if(!(heading == Vector3.zero))
                transform.forward = heading;
        }

        public void IncreaseSpeed(float moreSpeed)
        {
            moveSpeed += moreSpeed;
        }

        public void ReturnToInitValues() => moveSpeed = initialSpeed;

        public bool IsSpeedIncreased() => initialSpeed < moveSpeed;
    }
}

