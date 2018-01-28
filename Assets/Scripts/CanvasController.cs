using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    // contract variables
    Text contractName;
    List<Image> playerCompletionSprite;
    List<Text> playerCompletionValue;

    // player variables
    List<Text> names;
    List<Text> money;
    List<Image> moneyCompletion;
    List<Text> level;
    List<Image> levelCompletion;
    List<Image> coreSprite;
    List<Image> wingSprite;

    // money
    List<int> prices;
    List<int> levels;

    public Transform endPanel;

    public void Init ()
    {
        names = new List<Text>();
        money = new List<Text>();
        moneyCompletion = new List<Image>();
        level = new List<Text>();
        levelCompletion = new List<Image>();
        coreSprite = new List<Image>();
        wingSprite = new List<Image>();

        prices = new List<int>();
        levels = new List<int>();
        

        foreach (Transform playerTf in transform.Find("SidePanel/Players"))
        {
            names.Add(playerTf.Find("Name").GetComponent<Text>());
            moneyCompletion.Add(playerTf.Find("Money").GetComponent<Image>());
            money.Add(playerTf.Find("Money/Text").GetComponent<Text>());
            levelCompletion.Add(playerTf.Find("Points").GetComponent<Image>());
            level.Add(playerTf.Find("Points/Text").GetComponent<Text>());
            coreSprite.Add(playerTf.Find("Core Sprite").GetComponent<Image>());
            wingSprite.Add(playerTf.Find("Wings Sprite").GetComponent<Image>());

            prices.Add(20);
            levels.Add(0);
        }

        for(int i = 0; i < level.Count; ++i)
        {
            level[i].text = "0";
            levelCompletion[i].fillAmount = 0;
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

    public void SetSprites(List<Sprite> cores, List<Sprite> wings)
    {
        for (int i = 0; i < cores.Count; ++i)
        {
            coreSprite[i].sprite = cores[i];
            wingSprite[i].sprite = wings[i];
        }
    }

    public void UpdateMoney(List<Player> players)
    {
        for (int i = 0; i < players.Count; ++i)
        {
            money[i].text = players[i].money.ToString() + "$";
            moneyCompletion[i].fillAmount = (float)(players[i].money) / (float)prices[i];
        }
    }

    public void UpdatePrice(int index, int value)
    {
        prices[index] = value;
    }

    public void UpdateContract(ContractType type, int level)
    {
        switch (type)
        {
            case ContractType.banzai:
                contractName.text = "Banzaï ! Détruire " + level.ToString() + " de vos satellites";
                return;
            case ContractType.bateaux:
                contractName.text = "Réseau maritime niveau " + level.ToString();
                return;
            case ContractType.couverture:
                contractName.text = "Couverture privilégiée niveau " + level.ToString();
                return;
            case ContractType.danger:
                contractName.text = "Vents solaires. Baissez vos orbites";
                return;
            case ContractType.espionnage:
                contractName.text = "Espionnez " + level.ToString() + " satellite" + (level == 1 ? "" : "s") + " ennemi" + (level == 1 ? "" : "s");
                return;
            case ContractType.social:
                contractName.text = "Plan social. Sacrifiez " + level.ToString() + " satellite" + (level == 1 ? "" : "s");
                return;
            case ContractType.urbanisme:
                contractName.text = "Urbanisme niveau " + level.ToString();
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

        levels[playerIndex]++;
        level[playerIndex].text = levels[playerIndex].ToString();
        levelCompletion[playerIndex].fillAmount = (float)levels[playerIndex] / 10f;

        yield return null;
    }

    public IEnumerator FinalWin(int playerIndex)
    {
        endPanel.gameObject.SetActive(true);
        endPanel.Find("Text").GetComponent<Text>().text = names[playerIndex].text + " à gagné !";
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("MainMenu");
    }

    public IEnumerator FinalLose()
    {
        endPanel.gameObject.SetActive(true);
        endPanel.Find("Text").GetComponent<Text>().text = "Vous avouez échoué !";
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("MainMenu");
    }
}
