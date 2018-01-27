using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    // contract variables
    Text contractName;
    List<Image> playerCompletionSprite;
    List<Text> playerCompletionValue;

    // player variables
    List<Text> names;
    List<Text> money;

	public void Init ()
    {
        names = new List<Text>();
        money = new List<Text>();

        foreach (Transform playerTf in transform.Find("SidePanel/Players"))
        {
            names.Add(playerTf.Find("Name").GetComponent<Text>());
            money.Add(playerTf.Find("Money").GetComponent<Text>());
        }


        contractName = transform.Find("SidePanel/Contract/Name").GetComponent<Text>();
        playerCompletionSprite = new List<Image>();
        playerCompletionValue = new List<Text>();

        Transform playersCompletions = transform.Find("SidePanel/Contract/PlayersCompletions");
        for (int i = 0; i < playersCompletions.childCount; ++i)
        {
            playerCompletionSprite.Add(playersCompletions.GetChild(i).Find("Completion Sprite").GetComponent<Image>());
            playerCompletionValue.Add(playersCompletions.GetChild(i).Find("Completion Text").GetComponent<Text>());
        }
    }

    public void UpdateMoney(List<Player> players)
    {
        for (int i = 0; i < players.Count; ++i)
        {
            money[i].text = players[i].money.ToString() + "$";
        }
    }

    public void UpdateContract(ContractType type)
    {
        switch (type)
        {
            case ContractType.banzai:
                contractName.text = "Banzaï";
                return;
            case ContractType.bateaux:
                contractName.text = "Réseau maritime";
                return;
            case ContractType.couverture:
                contractName.text = "Couverture privilégiée";
                return;
            case ContractType.danger:
                contractName.text = "Vents solaires";
                return;
            case ContractType.espionnage:
                contractName.text = "Espionnage";
                return;
            case ContractType.social:
                contractName.text = "Plan social";
                return;
            case ContractType.urbanisme:
                contractName.text = "Urbanisme";
                return;
        }
    }

    public void UpdateContractScores(List<int> scores)
    {
        for (int i = 0; i < scores.Count; ++i)
        {
            playerCompletionSprite[i].fillAmount = (float) scores[i] / 100;
            playerCompletionValue[i].text = scores[i].ToString();
        }
    }
}
