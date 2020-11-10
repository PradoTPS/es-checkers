using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnosManager : Singleton<TurnosManager>
{

    public PieceFiliation turnoFiliation;

    public bool CanDoAction(PieceFiliation filiation)
    {
        if (filiation == turnoFiliation)
            return true;

        return false;
    }

    public void ChangeTurno()
    {
        turnoFiliation = turnoFiliation == PieceFiliation.player1 ? PieceFiliation.player2 : PieceFiliation.player1;
        GameManager.Instance.OnTurnoChange();
    }

}
