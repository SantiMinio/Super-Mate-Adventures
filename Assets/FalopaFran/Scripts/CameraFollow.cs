using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frano
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] Transform _target;

        public Vector3 offset;

        public void Awake()
        {
            Vector3 v1 = _target.position;
            Vector3 v2 = transform.position;
            offset = v2 - v1;
        }

        private void LateUpdate()
        {
            if (_target != null)
                transform.position = _target.position + offset;
        }
    }
}


