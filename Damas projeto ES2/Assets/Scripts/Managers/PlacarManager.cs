using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlacarManager : Singleton<PlacarManager>
{

    private int player1Points = 0;
    private int player2Points = 0;

    [SerializeField] private TextMeshProUGUI player1Text = null;
    [SerializeField] private TextMeshProUGUI player2Text = null;


    public void PieceEaten(PieceFiliation filiation)
    {
        if (filiation == PieceFiliation.player1)
            player2Points += 1;
        
        if (filiation == PieceFiliation.player2)
            player1Points += 1;

        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        player1Text.SetText("Placar: " + player1Points);
        player2Text.SetText("Placar: " + player2Points);
    }

}
