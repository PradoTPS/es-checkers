using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PieceAbstract : MonoBehaviour
{
    
    public abstract List<PiecePositionV2> Move();
    public abstract PiecePositionV2 Eat();

}
