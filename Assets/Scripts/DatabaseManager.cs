using System.Collections.Generic;
using Proyecto26;
using FullSerializer;

public static class DatabaseManager
{
    private const string dataBaseUrl = "https://coin-collector-6d016-default-rtdb.firebaseio.com/";
    private static fsSerializer serializer = new fsSerializer();
    public delegate void GetUsersCallback(Dictionary<string, UserInfo> users);

    public static void UpdateOrAddUser(UserInfo user)
    {
         RestClient.Put<UserInfo>($"{dataBaseUrl}users/{user.name}.json", user).Then(response => { 
            GameManager.gamemanagerInstance.SetOverlayStatus(false);
        });
    }


    public static void GetUsers(GetUsersCallback callback)
    {
        RestClient.Get($"{dataBaseUrl}users.json").Then(response =>
        {
            var responseJson = response.Text;
            var data = fsJsonParser.Parse(responseJson);
            object deserialized = null;
            serializer.TryDeserialize(data, typeof(Dictionary<string, UserInfo>), ref deserialized);
            var users = deserialized as Dictionary<string, UserInfo>;
            callback(users);
            GameManager.gamemanagerInstance.SetOverlayStatus(false);
        });
    }
}
