using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnosManager : Singleton<TurnosManager>
{

    public PieceFiliation turnoFiliation;

    [SerializeField] private Image panelP1 = null;
    [SerializeField] private Image panelP2 = null;

    private bool blockTurnChange = false;


    public bool CanDoAction(PieceFiliation filiation)
    {
        if (filiation == turnoFiliation)
            return true;

        return false;
    }

    public void ChangeTurno()
    {

        if (blockTurnChange)
        {
            blockTurnChange = false;
            return;
        }

        if(turnoFiliation == PieceFiliation.player1)
        {
            panelP1.gameObject.SetActive(false);
            panelP2.gameObject.SetActive(true);

            turnoFiliation = PieceFiliation.player2;
        }
        else
        {
            panelP2.gameObject.SetActive(false);
            panelP1.gameObject.SetActive(true);

            turnoFiliation = PieceFiliation.player1;
        }
        
        GameManager.Instance.OnTurnoChange();
    }

    public void BlockNextTurnChange()
    {
        blockTurnChange = true;
    }

}
