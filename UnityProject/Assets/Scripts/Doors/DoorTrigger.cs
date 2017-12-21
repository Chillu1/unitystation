﻿using System.Collections;
using PlayGroup;
using PlayGroups.Input;
using UI;
using UnityEngine;

namespace Doors
{
	/// <summary>
	///     Handles Interact messages from InputController.cs
	///     It also checks for access restrictions on the players ID card
	/// </summary>
	public class DoorTrigger : InputTrigger
	{
		public bool allowInput = true;
		private DoorController doorController;

		public void Start()
		{
			doorController = GetComponent<DoorController>();
		}

        public override void Interact(GameObject originator, Vector3 position, string hand)
        {
            
            // Close the door if it's open
            if(doorController.IsOpened && allowInput)
            {
                PlayerManager.LocalPlayer.GetComponent<PlayerNetworkActions>().CmdTryCloseDoor(gameObject);

                allowInput = false;
                StartCoroutine(DoorInputCoolDown());
            }

            // Attempt to open if it's closed
            if (doorController != null && allowInput)
            {
                PlayerManager.LocalPlayer.GetComponent<PlayerNetworkActions>().CmdCheckDoorPermissions(gameObject, PlayerManager.LocalPlayerScript.gameObject);

                allowInput = false;
                StartCoroutine(DoorInputCoolDown());
            }
        }

		private IEnumerator DoorInputCoolDown()
		{
			yield return new WaitForSeconds(0.3f);
			allowInput = true;
		}
	}
}