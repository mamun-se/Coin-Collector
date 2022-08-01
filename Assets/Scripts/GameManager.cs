using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform objectParent;
    [SerializeField] private GameObject giftpackPrefab;
    public static GameManager gamemanagerInstance = null;
    [SerializeField] private Transform scrollView = null;
    [SerializeField] private int totalCoins = 0;
    private List<GameObject> giftList = new List<GameObject>();
    [SerializeField] private TMP_Text totalCoinsText = null;
    [SerializeField] private InputField playerName = null;
    [SerializeField] private InputField playerMessage = null;
    [SerializeField] private InputField CoinAmount = null;
    [SerializeField] private Dropdown imageSelection = null;
    [SerializeField] private GameObject AddUserPanel = null;
    [SerializeField] private Button giftButton = null;
    [SerializeField] private GameObject overlay = null;

    void Awake()
    {
        gamemanagerInstance = this;
    }
    void Start()
    {
        int tempCoinval = PlayerPrefs.GetInt("Coin");
        if (tempCoinval > 0)
        {
            totalCoins = tempCoinval;
            totalCoinsText.text = totalCoins.ToString();
        }
    }

    private void GetAllUsers()
    {
        int tempCount = 0;
        DatabaseManager.GetUsers(users =>
        {
            foreach (var user in users)
            {
                tempCount++;
                if (tempCount < giftList.Count)
                {
                    var giftpackObj = giftList[tempCount];
                    giftpackObj.GetComponent<UserDataHolder>().SetPlayerData(user.Value.name,user.Value.message,user.Value.imageUrl,user.Value.numberOfCoins,user.Value.isAlReadyCollected);
                }
                else
                {
                    var giftpackObj = InstantiateGiftObj();
                    giftpackObj.GetComponent<UserDataHolder>().SetPlayerData(user.Value.name,user.Value.message,user.Value.imageUrl,user.Value.numberOfCoins,user.Value.isAlReadyCollected);
                    giftList.Add(giftpackObj);
                }
            }
        });
    }

    private GameObject InstantiateGiftObj()
    {
        var go = Instantiate(giftpackPrefab);
        go.transform.parent = objectParent;
        go.transform.DOScale(Vector3.one,0.25f);
        return go;
    }

    public void SetPlayercoins(int coinToAdd)
    {
        totalCoins += coinToAdd;
        totalCoinsText.text = totalCoins.ToString();
        PlayerPrefs.SetInt("Coin",totalCoins);
    }

    public void AddUser()
    {
        OpenAddUserPanel();
    }

    public void OpenGiftView()
    {
        giftButton.interactable = false;
        SetOverlayStatus(true);
        scrollView.DOScale(Vector3.one,0.5f).OnComplete(() =>{
            GetAllUsers();
        });;
    }

    public void CloseGiftView()
    {
        giftButton.interactable = true;
        scrollView.DOScale(Vector3.zero,0.5f);
    }


    private void AddPlayer()
    {
        UserInfo user = new UserInfo();
        user.name = playerName.text;
        user.message = playerMessage.text;
        user.numberOfCoins = int.Parse(CoinAmount.text);
        user.imageUrl = imageSelection.options[imageSelection.value].text;
        user.isAlReadyCollected = false;
        DatabaseManager.UpdateOrAddUser(user);
    }

    private void OpenAddUserPanel()
    {
        AddUserPanel.transform.DOScale(Vector3.one,0.5f);
    }

    public void CloseAddUserPanel()
    {
        AddUserPanel.transform.DOScale(Vector3.zero,0.5f);
    }

    public void SetOverlayStatus(bool shouldActive)
    {
        overlay.SetActive(shouldActive);
    }
}
