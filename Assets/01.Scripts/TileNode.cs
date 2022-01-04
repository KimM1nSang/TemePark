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
                //���� �� Ȱ��ȭ
                Transform btnTrm = Instantiate(btn, canvas.transform);
                btnTrm.gameObject.SetActive(true);

                //��ġ ����
                btnTrm.transform.position = new Vector3((transform.position.x + nextNode.transform.position.x) / 2, 0, (transform.position.z + nextNode.transform.position.z) / 2);

                //�ٶ󺸰� �ϱ�
                btnTrm.LookAt(nextNode.transform);
                // ��ư ��� �Լ� ����
                btnTrm.GetComponentInChildren<Button>().onClick.AddListener(() => {
                    TurnSystem.Instance.unitDir = Array.IndexOf(nextTileNodes, nextNode);
                    TurnSystem.Instance.isCor = false;
                });
            }
        }
      
    }
}
