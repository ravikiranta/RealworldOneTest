using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Controllers {
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerData))]
    public class PlayerMovementController : MonoBehaviour
    {
        #region Variables
        [Header("Dev Settings")]
        [SerializeField] private float moveSpeed;             //Floating point variable to store the player's movement speed.
        [SerializeField] private float rotateSpeed;             //Floating point variable to store the player's movement speed.
        [SerializeField] private bool disablePlayerMovement;

        [Header("Info")]
        [SerializeField] private float moveSpeedIncrement;      // The increment in movement speed that comes from pickups
        [SerializeField] private float moveIncrementTimer;      // The increment pickup lifetime timer

        private Rigidbody rb;       //Store a reference to the Rigidbody2D component required to use 2D Physics.
        private PlayerData playerData;
        #endregion

        #region Properties
        public bool DisablePlayerMovement
        {
            set
            {
                disablePlayerMovement = value;
            }
        }
        #endregion

        #region Init
        void Awake()
        {
            //Get and store a reference to the Rigidbody2D component so that we can access it.
            rb = GetComponent<Rigidbody>();
            playerData = GetComponent<PlayerData>();
        }
        #endregion

        #region PowerupEffects
        public void IncrementMoveSpeed(float incrementValue, float timeInterval)
        {
            moveSpeedIncrement = incrementValue;
            moveIncrementTimer = timeInterval;

            StopCoroutine("SpeedIncrementTimer");
            StartCoroutine("SpeedIncrementTimer");
        }

        IEnumerator SpeedIncrementTimer()
        {
            while(moveIncrementTimer > 0)
            {
                moveIncrementTimer--;
                yield return new WaitForSeconds(1f);
            }
            ResetMoveSpeedIncrement();
        }

        void ResetMoveSpeedIncrement()
        {
            moveSpeedIncrement = 0;
        }
        #endregion

        #region InputAndPhysics
        void FixedUpdate()
        {
            float moveVertical = 0;
            if (!disablePlayerMovement)
            {
                switch (playerData.PlayerID)
                {
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
            }

            //Call the AddForce function of our Rigidbody2D rb supplying 
            //speed, input sensitivity and direction to move our player forward.
            rb.AddForceAtPosition(transform.forward * (moveSpeed + moveSpeedIncrement) * moveVertical, transform.position);
        }
        #endregion
    }
}