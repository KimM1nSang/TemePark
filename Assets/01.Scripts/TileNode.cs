using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNode : MonoBehaviour
{

    public TileNode lastTileNode;

    public TileNode[] nextTileNodes;

    public Unit[] currentUnits;

    public Transform TileCenterTrm;
    public Transform[] unitsPosTrms;
}
