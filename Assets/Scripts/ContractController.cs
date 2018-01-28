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

    public bool inContract = false;

    public void CreateContract()
    {
        if (rd == null)
            rd = new System.Random();

        int length = Enum.GetNames(typeof(ContractType)).Length;
        type = (ContractType)rd.Next(length);
        //type = ContractType.bateaux;

        zones = new List<Zone>();
        List<Zone> validZones;

        switch (type)
        {
            case ContractType.banzai:
                CreateContract();
                break;
            case ContractType.bateaux:
                validZones = sc.zones.FindAll(z => z.type == Zone.ZoneType.SEA);
                GenerateContractZone(validZones);
                break;
            case ContractType.couverture:
                validZones = sc.zones.FindAll(z => z.type == Zone.ZoneType.EARTH);
                GenerateContractZone(validZones);
                break;
            case ContractType.danger:
                CreateContract();
                break;
            case ContractType.espionnage:
                CreateContract();
                break;
            case ContractType.social:
                CreateContract();
                break;
            case ContractType.urbanisme:
                CreateContract();
                break;
        }

        // Reroll contract if already
        if (CheckContract(false))
            CreateContract();

        // Hide old particles
        foreach (Zone zone in sc.zones)
        {
            zone.tf.GetComponent<ParticleSystem>().Stop();
        }

        // activate particles for contract zones
        if (type == ContractType.bateaux || type == ContractType.couverture || type == ContractType.urbanisme)
        {
            foreach (Zone zone in zones)
            {
                zone.tf.GetComponent<ParticleSystem>().Play();
            }
        }

        inContract = true;
        sc.canvasController.UpdateContract(type, power);
    }

    public bool CheckContract(bool display = true)
    {
        if (!inContract)
            return false;

        List<int> scores;
        switch(type)
        {
            case ContractType.banzai:
                scores = CalculateBanzai();
                break;
            case ContractType.bateaux:
                scores = CalculateZones();
                break;
            case ContractType.couverture:
                scores = CalculateZones();
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
                scores = CalculateZones();
                break;
            default:
                Debug.LogError("Unknown contract type");
                return false;
        }
        
        if (scores.Exists(i => i == 100))
        {
            inContract = false;

            if (!display)
                return true;

            StartCoroutine(sc.canvasController.WinContract(this));
            
            for (int i = 0; i < scores.Count; ++i)
            {
                if (scores[i] == 100)
                {
                    sc.players[i].points++;
                    StartCoroutine(sc.canvasController.PlayerWin(i));
                }
            }

            return true;
        }

        /*int[] scoresA = { debugFill, debugFill, debugFill, debugFill };
        scores = new List<int>(scoresA);*/
        
        sc.canvasController.UpdateContractScores(scores);

        return false;
    }

    

    public List<int> CalculateBanzai()
    {
        return new List<int>();
    }

    public List<int> CalculateZones()
    {
        List<int> scores = new List<int>();
        foreach (Player player in sc.players)
        {
            float completion = 0;
            float maxCompletion = zones.Count * power;
            foreach (Zone zone in zones)
            {
                int visited = zone.visitedThisFrame[player.id];
                if (visited > power)
                    visited = power;

                completion += visited;
            }
            scores.Add((int)((completion / maxCompletion) * 100));
        }

        return scores;
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

    public void GenerateBanzai()
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
        //List<Zone> validZones = sc.zones.FindAll(z => z.subtype == Zone.SubZoneType.TOWN); // TODO change selector and finish
        //int startingValidZoneIndex = rd.Next();
    }

    private void GenerateContractZone(List<Zone> validZones)
    {
        // get a random index in valid zones
        Zone.ZoneType type = validZones[0].type;
        int startingValidZoneIndex = rd.Next(validZones.Count);

        // retrieve the index in the "all zones" list
        int startingZoneIndex = sc.zones.IndexOf(validZones[startingValidZoneIndex]);

        // calculating variables
        int size = rd.Next(5);
        int currentOffset = 1;
        int direction = 1;

        // main condictions for contract
        zones.Add(sc.zones[startingZoneIndex]);
        power = rd.Next(2) + 1;

        while (size > 0)
        {
            int testedIndex = startingZoneIndex + (currentOffset * direction);

            if (testedIndex < 0)
                testedIndex = sc.zones.Count + testedIndex;
            else if (testedIndex >= sc.zones.Count)
                testedIndex = testedIndex - sc.zones.Count;

            Zone testedZone = sc.zones[testedIndex];
            if (testedZone.type == type)
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