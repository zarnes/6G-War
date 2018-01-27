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
    private List<Zone> zones;
    private int power;

    public void CreateContract()
    {
        if (rd == null)
            rd = new System.Random();

        int length = Enum.GetNames(typeof(ContractType)).Length;
        type = (ContractType)rd.Next(length);
        type = ContractType.bateaux;

        zones = new List<Zone>();

        switch (type)
        {
            case ContractType.banzai:
                GenerateBanzai();
                break;
            case ContractType.bateaux:
                GenerateBateaux();
                break;
            case ContractType.couverture:
                GenerateCouverture();
                break;
            case ContractType.danger:
                GenerateDanger();
                break;
            case ContractType.espionnage:
                GenerateEspionnage();
                break;
            case ContractType.social:
                GenerateSocial();
                break;
            case ContractType.urbanisme:
                GenerateUrbanisme();
                break;
        }

        // Reroll contract if already
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

        // TODO test if one of the scores is 100
        if (scores.Exists(i => i == 100))
        {
            foreach (Zone zone in zones)
            {
                zone.tf.GetComponent<ParticleSystem>().Stop();
            }
        }

        // activate particles for contract zones
        if (type == ContractType.bateaux || type == ContractType.couverture || type == ContractType.urbanisme)
        {
            foreach(Zone zone in zones)
            {
                zone.tf.GetComponent<ParticleSystem>().Play();
            }
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

    public void GenerateBanzai()
    {
        
    }

    public void GenerateBateaux()
    {
        // get a random index in valid zones
        List<Zone> validZones = sc.zones.FindAll(z => z.type == Zone.ZoneType.SEA);
        int startingValidZoneIndex = rd.Next(validZones.Count);

        // retrieve the index in the "all zones" list
        int startingZoneIndex = sc.zones.IndexOf(validZones[startingValidZoneIndex]);

        // calculating variables
        int size = rd.Next(5);
        int currentOffset = 1;
        int direction = 1;

        // main condictions for contract
        zones.Add(sc.zones[startingZoneIndex]);
        power = rd.Next(3) + 1;

        while(size > 0)
        {
            Zone testedZone = sc.zones[startingZoneIndex + (currentOffset * direction)];
            if (testedZone.type == Zone.ZoneType.SEA)
            {
                zones.Add(testedZone);
                ++currentOffset;
                --size;
            }
            else if (direction == 1) // if not possible to expend, try the other side
            {
                direction = -1;
                currentOffset = 1;
            }
            else // if still not possible, return
            {
                return;
            }
        }
    }

    public void GenerateCouverture()
    {

    }

    public void GenerateDanger()
    {

    }

    public void GenerateEspionnage()
    {

    }

    public void GenerateSocial()
    {

    }

    public void GenerateUrbanisme()
    {

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