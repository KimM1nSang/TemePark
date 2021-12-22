using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraHandler : MonoBehaviour
{
    private CinemachineVirtualCamera vCam;

    void Start()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        switch (TurnSystem.Instance.state)
        {
            case TurnState.START:
                break;
            case TurnState.PLAYERTURN:
                vCam.Follow = TurnSystem.Instance.playerUnit.transform;
                vCam.LookAt = TurnSystem.Instance.playerUnit.transform;
                if(!TurnSystem.Instance.playerUnit.dice.isRolling && !TurnSystem.Instance.playerUnit.dice.isSelecting)
                {
                    var transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
                    transposer.m_FollowOffset = new Vector3(0, 10, -10);
                }
                else
                {
                    var transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
                    transposer.m_FollowOffset = new Vector3(0,5,-5);
                }
                break;
            case TurnState.ENEMYTURN:
                vCam.Follow = TurnSystem.Instance.enemyUnit.transform;
                vCam.LookAt = TurnSystem.Instance.enemyUnit.transform;
                break;
            case TurnState.DIRCHOISE:
                break;
            case TurnState.WON:
                break;
            case TurnState.LOST:
                break;
            default:
                break;
        }
    }
}
