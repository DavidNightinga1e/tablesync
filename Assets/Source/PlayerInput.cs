using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace TableSync.Demo
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerInput : MonoBehaviour
    {
        public float speed = 2;
        private CharacterController _characterController;
        
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        void Update()
        {
            var inputMove = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            var translate = inputMove * (speed * Time.deltaTime);
            _characterController.Move(translate);
            
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var origin = ray.origin;
            var direction = ray.direction;
            var directionMultiplier = -origin.y / direction.y;
            var xPos = origin.x + directionMultiplier * direction.x;
            var zPos = origin.z + directionMultiplier * direction.z;
            var targetLook = new Vector3(xPos, 0, zPos);
            transform.LookAt(targetLook);
            
        }
    }
}