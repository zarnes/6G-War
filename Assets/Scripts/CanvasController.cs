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

    public void UpdateContract(ContractType type, int level)
    {
        switch (type)
        {
            case ContractType.banzai:
                contractName.text = "Banzaï ! Détruire " + level + " de vos satellites";
                return;
            case ContractType.bateaux:
                contractName.text = "Réseau maritime niveau " + level;
                return;
            case ContractType.couverture:
                contractName.text = "Couverture privilégiée niveau " + level;
                return;
            case ContractType.danger:
                contractName.text = "Vents solaires. Baissez vos orbites";
                return;
            case ContractType.espionnage:
                contractName.text = "Espionnez " + level + " satellite" + (level == 1 ? "" : "s") + " ennemi" + (level == 1 ? "" : "s");
                return;
            case ContractType.social:
                contractName.text = "Plan social. Sacrifiez " + level + " satellite" + (level == 1 ? "" : "s");
                return;
            case ContractType.urbanisme:
                contractName.text = "Urbanisme niveau " + level;
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

    public IEnumerator WinContract(ContractController contractController)
    {
        contractName.text = "Contrat remporté !";
        yield return new WaitForSeconds(5);
        contractController.CreateContract();
    }

    public IEnumerator PlayerWin(int playerIndex)
    {
        playerCompletionSprite[playerIndex].fillAmount = 1;
        playerCompletionValue[playerIndex].text = "OK!";

        iTween.ShakePosition(playerCompletionSprite[playerIndex].transform.parent.gameObject, new Vector3(7, 7, 7), 5);
        yield return null;
    }
}
