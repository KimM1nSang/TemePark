using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class TileNode : MonoBehaviour
{

    public TileNode lastTileNode;

    public TileNode[] nextTileNodes;

    public List<Unit> currentUnits = new List<Unit>();

    public Transform TileCenterTrm;
    public Transform[] unitsPosTrms;

    public Transform btn;
    public Canvas canvas;

    private void Start()
    {
        if(nextTileNodes.Length > 1)
        {
            foreach (var nextNode in nextTileNodes)
            {
                //생성 및 활성화
                Transform btnTrm = Instantiate(btn, canvas.transform);
                btnTrm.gameObject.SetActive(true);

                //위치 적용
                btnTrm.transform.position = new Vector3((transform.position.x + nextNode.transform.position.x) / 2, 0, (transform.position.z + nextNode.transform.position.z) / 2);

                //바라보게 하기
                btnTrm.LookAt(nextNode.transform);
                // 버튼 기능 함수 적용
                btnTrm.GetComponentInChildren<Button>().onClick.AddListener(() => {
                    TurnSystem.Instance.unitDir = Array.IndexOf(nextTileNodes, nextNode);
                    TurnSystem.Instance.isCor = false;
                });
            }
        }
      
    }
}
