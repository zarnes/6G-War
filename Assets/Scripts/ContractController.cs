using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractController : MonoBehaviour {
    [Range(0, 100)]
    public int debugFill;

    public SatelliteController sc;

    private System.Random rd;

    private ContractType type;

    private void Start()
    {
        rd = new System.Random();
    }

    public void CreateContract()
    {
        int length = Enum.GetNames(typeof(ContractType)).Length;
        type = (ContractType)rd.Next(length);
        Debug.Log(type);

        if (CheckContract(false))
            CreateContract();

        sc.canvasController.UpdateContract(type);
    }

    public bool CheckContract(bool display = true)
    {
        List<int> scores;
        switch(type)
        {
            case ContractType.banzai:
                scores = CalculateBanzai();
                break;
            case ContractType.bateaux:
                scores = CalculateBateaux();
                break;
            case ContractType.couverture:
                scores = CalculateCouverture();
                break;
            case ContractType.danger:
                scores = CalculateDanger();
                break;
            case ContractType.espionnage:
                scores = CalculateEspionnage();
                break;
            case ContractType.social:
                scores = CalculateSocial();
                break;
            case ContractType.urbanisme:
                scores = CalculateUrbanisme();
                break;
            default:
                Debug.LogError("Unknown contract type");
                return false;
        }

        int[] scoresA = { debugFill, debugFill, debugFill, debugFill };
        scores = new List<int>(scoresA);

        if (display)
            sc.canvasController.UpdateContractScores(scores);

        return false;
    }

    public List<int> CalculateBanzai()
    {
        return new List<int>();
    }

    public List<int> CalculateBateaux()
    {
        return new List<int>();
    }

    public List<int> CalculateCouverture()
    {
        return new List<int>();
    }

    public List<int> CalculateDanger()
    {
        return new List<int>();
    }

    public List<int> CalculateEspionnage()
    {
        return new List<int>();
    }

    public List<int> CalculateSocial()
    {
        return new List<int>();
    }

    public List<int> CalculateUrbanisme()
    {
        return new List<int>();
    }
}

public enum ContractType
{
    couverture,     // couverture : couvrir des zones avec x points
    urbanisme,      // urbanisme : couvrir x villes avec x points
    bateaux,        // bateaux : couvrir x mers
    danger,         // danger spatial : approcher x satellites sous le cercle indiqué
    espionnage,     // espionnage : avoir x sattelites derrière un autre
    social,         // plan social : perdre x satellites
    banzai          // banzaï : detruire x satellites ennemis
}