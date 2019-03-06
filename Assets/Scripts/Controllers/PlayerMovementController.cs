using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Controllers {
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerData))]
    public class PlayerMovementController : MonoBehaviour
    {
        public float moveSpeed;             //Floating point variable to store the player's movement speed.
        public float rotateSpeed;             //Floating point variable to store the player's movement speed.

        private Rigidbody rb;       //Store a reference to the Rigidbody2D component required to use 2D Physics.
        private PlayerData playerData;

        void Awake()
        {
            //Get and store a reference to the Rigidbody2D component so that we can access it.
            rb = GetComponent<Rigidbody>();
            playerData = GetComponent<PlayerData>();
        }

        void FixedUpdate()
        {
            float moveVertical = 0;

            switch (playerData.PlayerID) {
                case Enums.PlayerID.First:
                    //Rotate the first player based on the horizontal axis sensitivity and rotation speed.
                    transform.Rotate(0, Input.GetAxis("FirstPlayerHorizontal") * rotateSpeed, 0);

                    //Store the current vertical input from the first player in the float moveVertical.
                    moveVertical = Input.GetAxis("FirstPlayerVertical");
                break;

                case Enums.PlayerID.Second:
                    //Rotate second player based on input from second player horizontal axis.
                    transform.Rotate(0, Input.GetAxis("SecondPlayerHorizontal") * rotateSpeed, 0);

                    //Store the current vertical input from second player in the float moveVertical.
                    moveVertical = Input.GetAxis("SecondPlayerVertical");
                break;
            }

            //Call the AddForce function of our Rigidbody2D rb supplying 
            //speed, input sensitivity and direction to move our player forward.
            rb.AddForceAtPosition(transform.forward * moveSpeed * moveVertical, transform.position);
        }
    }
}