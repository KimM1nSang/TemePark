using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Dice : MonoBehaviour
{
    private Vector3[] diceRots =
    {
        new Vector3(0,0,0),// 1
        new Vector3(90,0,90),// 2
        new Vector3(-90,0,0),// 3
        new Vector3(90,0,0),// 4
        new Vector3(0,90,0), // 5
        new Vector3(180,0,0) // 6
    };
    public bool isRolling { get; set; } = false;
    public bool isSelecting { get; set; } = false;

    [SerializeField] private Vector3 turn;
    private Vector3 currentTurn;

    public int currentNum { get; set; }

    public Unit unit { get; set; }

    public void Roll()
    {
        if (isRolling || isSelecting) return;

        StartCoroutine(Rolling());
    }
    private IEnumerator Rolling()
    {
        currentTurn = transform.rotation.eulerAngles;
        isRolling = true;
        while (isRolling)
        {
            transform.rotation = Quaternion.Euler(currentTurn += turn);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    public void Select()
    {
        if (isSelecting||!isRolling) return;

        isSelecting = true;
        isRolling = false;

        currentNum = Random.Range(0, 5);
        transform.rotation = Quaternion.Euler(diceRots[currentNum]);

        transform.DOMoveY(.2f, .1f).SetRelative().SetLoops(2,LoopType.Yoyo).OnComplete(()=> { TurnSystem.Instance.DiceCallback(unit);isSelecting = false; });
   
    }
}
