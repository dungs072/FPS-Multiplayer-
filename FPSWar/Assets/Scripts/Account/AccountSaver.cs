// using UnityEngine;
// using System;
// using Firebase.Database;
// using System.Collections;

// [Serializable]
// public class AccountData
// {
//     public string Gmail;
//     public string Password;
//     public bool ActivityState;
// }
// public class RegisterData
// {
//     public string Username;
//     public string Gmail;

//     public string Password;
//     public string ConfirmPassword;
// }
// public class AccountSaver : MonoBehaviour
// {
//     public event Action<LoginState> OnFinishHandlingLogin;
//     public event Action<RegisterState> OnFinishHandlingRegister;
//     private LoginState currentLoginState;
//     private RegisterState currentRegisterState;
//     private DatabaseReference dbRef;
//     private void Awake()
//     {
//         dbRef = FirebaseDatabase.DefaultInstance.RootReference;

//     }
//     private void OnDestroy()
//     {
//         string tempStr = PlayerPrefs.GetString("UserName");
//         if(tempStr==""){return;}
//         ChangeActivityState(tempStr,false);
//     }
//     public void Login(string userName, string password)
//     {
//         StartCoroutine(CheckDataLogin(userName, password));
//     }
//     private IEnumerator CheckDataLogin(string userName, string password)
//     {
//         var serverData = dbRef.Child("users").Child(userName).GetValueAsync();
//         yield return new WaitUntil(predicate: () => serverData.IsCompleted);
//         try
//         {
//             DataSnapshot snapshot = serverData.Result;
//             string jsonData = snapshot.GetRawJsonValue();
//             AccountData account = null;
//             if (jsonData != null)
//             {
//                 account = JsonUtility.FromJson<AccountData>(jsonData);
//                 if (password == account.Password)
//                 {
//                     currentLoginState = LoginState.Success;
//                     if (account.ActivityState)
//                     {
//                         currentLoginState = LoginState.IsActivated;
//                     }
//                     else
//                     {
//                         ChangeActivityState(userName, true);
//                     }
//                 }
//                 else
//                 {
//                     currentLoginState = LoginState.WrongPassword;
//                 }

//             }
//             else
//             {
//                 currentLoginState = LoginState.AccountDoesNotExist;
//             }
//         }
//         catch (Exception e)
//         {
//             Debug.Log(e.Message);
//             currentLoginState = LoginState.AccountDoesNotExist;
//         }
//         OnFinishHandlingLogin?.Invoke(currentLoginState);
//     }
//     public void ChangeActivityState(string userName, bool state)
//     {
//         dbRef.Child("users").Child(userName).Child("ActivityState").SetValueAsync(state);
//     }


//     public void Register(RegisterData data)
//     {
//         StartCoroutine(CheckDataRegister(data));
//     }
//     private IEnumerator CheckDataRegister(RegisterData data)
//     {
//         currentRegisterState = RegisterState.None;
//         var task = dbRef.Child("users").GetValueAsync();
//         yield return new WaitUntil(() => task.IsCompleted);

//         if (task.IsFaulted)
//         {
//             currentRegisterState = RegisterState.ServerError;
//         }
//         else if (task.IsCompleted)
//         {
//             DataSnapshot snapshot = task.Result;
//             foreach (DataSnapshot userSnapshot in snapshot.Children)
//             {
//                 string userName = userSnapshot.Key;
//                 if (userName == data.Username)
//                 {
//                     currentRegisterState = RegisterState.DuplicateUserName;
//                     break;
//                 }
//                 string gmail = userSnapshot.Child("Gmail").Value.ToString();
//                 if (gmail == data.Gmail)
//                 {
//                     currentRegisterState = RegisterState.DuplicateGmail;
//                     break;
//                 }
//             }
//         }

//         if (currentRegisterState != RegisterState.None)
//         {

//         }
//         else if (data.Password != data.ConfirmPassword)
//         {
//             currentRegisterState = RegisterState.ConfirmPasswordNotMatch;
//         }
//         else
//         {
//             var account = new AccountData
//             {
//                 Gmail = data.Gmail,
//                 Password = data.Password,
//                 ActivityState = false
//             };
//             string json = JsonUtility.ToJson(account);
//             dbRef.Child("users").Child(data.Username).SetRawJsonValueAsync(json);
//             currentRegisterState = RegisterState.Success;
//         }
//         OnFinishHandlingRegister?.Invoke(currentRegisterState);
//     }


// }
// public enum LoginState
// {
//     WrongPassword,
//     AccountDoesNotExist,
//     Success,
//     IsActivated
// }
// public enum RegisterState
// {
//     ConfirmPasswordNotMatch,
//     Success,
//     DuplicateUserName,
//     DuplicateGmail,
//     ServerError,
//     None
// }
