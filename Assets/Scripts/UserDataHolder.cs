using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UserDataHolder : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text playerMessageText;
    [SerializeField] private TMP_Text coinAmountText;
    [SerializeField] private Image playerImage = null;
    [SerializeField] private Button collectButton = null;
    [SerializeField] private Color notAvailableColor;
    private int coinAmount = 0;
    private string imageUrl = string.Empty;
    private string playerName = string.Empty;
    private string playerMessage = string.Empty;
    private IEnumerator imageDownLoadCoroutine;

    public void SetPlayerData(string name, string msg, string _imageUrl , int _coinAmount , bool _isCollected)
    {
        coinAmount = _coinAmount;
        playerName = name;
        playerMessage = msg;
        imageUrl = _imageUrl;
        collectButton.onClick.AddListener(CollectCoins);
        VisualLisePlayerData();
        SetCollectButtonState(_isCollected);

    }

    private void SetCollectButtonState(bool _isCollected)
    {
        if (_isCollected)
        {
            transform.GetComponent<Image>().color = notAvailableColor;
            collectButton.transform.GetComponent<Image>().color = notAvailableColor;
        }
        else
        {
            transform.GetComponent<Image>().color = Color.white;
            collectButton.transform.GetComponent<Image>().color = Color.white;
        }
        collectButton.interactable = !_isCollected;
    }

    private void VisualLisePlayerData()
    {
        playerNameText.text = playerName;
        playerMessageText.text = playerMessage;
        coinAmountText.text = coinAmount.ToString();
        imageDownLoadCoroutine = GetTexture();
        StartCoroutine(imageDownLoadCoroutine);
    }

    private IEnumerator GetTexture() {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
        }
        else {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            playerImage.sprite = Sprite.Create(myTexture,new Rect(0,0,128,128),Vector2.zero);
        }
    }


    private void CollectCoins()
    {
        GameManager.gamemanagerInstance.SetPlayercoins(coinAmount);
        SetCollectButtonState(true);
        UpdatePlayerData();
        GameManager.gamemanagerInstance.SetOverlayStatus(true);
    }

    private void UpdatePlayerData()
    {
        UserInfo user = new UserInfo();
        user.name = playerName;
        user.message = playerMessage;
        user.imageUrl = imageUrl;
        user.numberOfCoins = coinAmount;
        user.isAlReadyCollected = true;
        DatabaseManager.UpdateOrAddUser(user);
    }
   
}
