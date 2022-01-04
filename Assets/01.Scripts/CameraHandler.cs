using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using System;
public class CameraHandler : MonoBehaviour
{
    private CinemachineVirtualCamera vCam;

    [SerializeField]
    private Transform mapCenter;
    [SerializeField]
    private Transform movingThing;
    private float camDist;
    [SerializeField]
    private float camspeed;

    [SerializeField]
    private float zoomSpeed;

    float rx;
    float ry;
    public float rotSpeed = 200;

    void Start()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        StartCoroutine(Turn());
    }

    void Update()
    {
        switch (TurnSystem.Instance.state)
        {
            case TurnState.BEFORESTART:
                vCam.enabled = true;
                Cursor.visible = true;
                vCam.LookAt = mapCenter;
                camDist = 20;
                break;
            case TurnState.WATCH:
                vCam.enabled = false;
                Cursor.visible = false;
                vCam.LookAt = movingThing;
                vCam.Follow = movingThing;
                MoveCam();
                break;
            case TurnState.START:
                vCam.enabled = true;
                Cursor.visible = true;
                mapCenter.DORotate(new Vector3(0, 0, 0), 1f).OnComplete(() => { vCam.LookAt = null; });
                Zoom(10, 2);
                break;
            case TurnState.PLAYERTURN:
                vCam.Follow = TurnSystem.Instance.playerUnit.transform;
                Zoom(5, 2);
                break;
            case TurnState.ENEMYTURN:
                vCam.Follow = TurnSystem.Instance.enemyUnit.transform;
                Zoom(5, 2);
                break;
            case TurnState.DIRCHOISE:
                break;
            case TurnState.BETWEEN:
                vCam.Follow = mapCenter;
                Zoom(10, 2);
                break;
            case TurnState.SELECTED:
                break;
            case TurnState.WON:
                break;
            case TurnState.LOST:
                break;
            default:
                break;
        }

        Vector3 offset = new Vector3(0, camDist, -camDist);
        switch (TurnSystem.Instance.state)
        {
            case TurnState.PLAYERTURN:
            case TurnState.ENEMYTURN:
                if (TurnSystem.Instance.playerUnit.dice.isRolling || TurnSystem.Instance.enemyUnit.dice.isRolling)
                {
                    offset = new Vector3(0, 0, -camDist);
                }
                else
                {
                    offset = new Vector3(0, camDist, -camDist);
                }
                break;
        }
        vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = offset;
    }
    public void Zoom(float targetDist,float zoomSpeed)
    {
        camDist = camDist + (targetDist - camDist) * zoomSpeed * Time.deltaTime;
    }
    private IEnumerator Turn()
    {
        Vector3 curTurn = mapCenter.rotation.eulerAngles;
        Vector3 turn = new Vector3(0,0.05f,0);
        while (TurnSystem.Instance.state == TurnState.BEFORESTART)
        {
            mapCenter.rotation = Quaternion.Euler(curTurn += turn);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
    private void MoveCam()
    {
        Vector3 dir = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        );

        movingThing.Translate(dir * camspeed * Time.deltaTime);


        float mx = Input.GetAxis("Mouse X"); 
        float my = Input.GetAxis("Mouse Y");

        rx += rotSpeed * my * Time.deltaTime;
        ry += rotSpeed * mx * Time.deltaTime;

        rx = Mathf.Clamp(rx, -80, 80);

        movingThing.eulerAngles = new Vector3(-rx, ry, 0);

    }
}