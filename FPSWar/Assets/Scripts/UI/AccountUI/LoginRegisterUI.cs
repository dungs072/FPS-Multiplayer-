// using UnityEngine;
// using TMPro;
// using Unity.VisualScripting;
// using UnityEngine.UI;

// public class LoginRegisterUI : MonoBehaviour
// {
//     [SerializeField] private GameObject mainMenuPage;
//     [SerializeField] private GameObject loginPage;
//     [SerializeField] private GameObject registerPage;
//     [SerializeField] private NotificationControl notificationControl;
//     [SerializeField] private AccountSaver accountSaver;

//     [Header("Login input")]
//     [SerializeField] private TMP_InputField userNameLoginInput;
//     [SerializeField] private TMP_InputField passwordLoginInput;
//     [Header("Register input")]
//     [SerializeField] private TMP_InputField userNameRegisterInput;
//     [SerializeField] private TMP_InputField gmailRegisterInput;
//     [SerializeField] private TMP_InputField passwordRegisterInput;
//     [SerializeField] private TMP_InputField confirmPasswordRegisterInput;
//     [Header("Background")]
//     [SerializeField] private BackgroundManager bgManager;
//     [Header("Name player")]
//     [SerializeField] private TMP_Text userNameDisplay;
//     public static bool IsLoginSuccess = false;
//     private void Start()
//     {
//         bgManager.ChangeBackgroundImage(BGImageType.MainBG);
//         mainMenuPage.SetActive(IsLoginSuccess);
//         loginPage.SetActive(!IsLoginSuccess);
//         if (!IsLoginSuccess)
//         {
//             string tempStr = PlayerPrefs.GetString("UserName");
//             userNameLoginInput.text = tempStr;
//         }
//         else
//         {
//             string tempStr = PlayerPrefs.GetString("UserName");
//             userNameDisplay.text = tempStr;
//         }
//         accountSaver.OnFinishHandlingLogin += HandleLogin;
//         accountSaver.OnFinishHandlingRegister += HandleRegister;

//     }
//     private void OnDestroy()
//     {
//         accountSaver.OnFinishHandlingLogin -= HandleLogin;
//         accountSaver.OnFinishHandlingRegister -= HandleRegister;
//     }
//     public void Login()
//     {
//         if (userNameLoginInput.text.Trim() == "")
//         {
//             notificationControl.gameObject.SetActive(true);
//             notificationControl.SetText("User name is blank. Please enter infor");
//             return;
//         }
//         else if (passwordLoginInput.text.Trim() == "")
//         {
//             notificationControl.gameObject.SetActive(true);
//             notificationControl.SetText("Password is blank. Please enter infor");
//             return;
//         }
//         accountSaver.Login(userNameLoginInput.text.Trim(),
//                          passwordLoginInput.text.Trim());

//     }
//     private void HandleLogin(LoginState state)
//     {
//         if (state == LoginState.WrongPassword)
//         {
//             notificationControl.gameObject.SetActive(true);
//             notificationControl.SetText("Wrong password. Please check password again");
//             IsLoginSuccess = false;
//         }
//         else if(state==LoginState.IsActivated)
//         {
//             notificationControl.gameObject.SetActive(true);
//             notificationControl.SetText("This account is active. Maybe your account was hacked by someone");
//             IsLoginSuccess = false;
//         }
//         else if (state == LoginState.AccountDoesNotExist)
//         {
//             notificationControl.gameObject.SetActive(true);
//             notificationControl.SetText("This account does not exist. Please register the new one");
//             IsLoginSuccess = false;
//         }
//         else if (state == LoginState.Success)
//         {
//             userNameDisplay.text = userNameLoginInput.text.Trim();
//             PlayerPrefs.SetString("UserName", userNameLoginInput.text.Trim());
//             OptionMenu.PlayerName = userNameLoginInput.text.Trim();
//             loginPage.SetActive(false);
//             mainMenuPage.SetActive(true);
//             IsLoginSuccess = true;
//         }
//     }
//     public void Register()
//     {
//         if (userNameRegisterInput.text.Trim() == "")
//         {
//             notificationControl.gameObject.SetActive(true);
//             notificationControl.SetText("User name is blank. Please enter infor");
//             return;
//         }
//         else if (gmailRegisterInput.text.Trim() == "")
//         {
//             notificationControl.gameObject.SetActive(true);
//             notificationControl.SetText("Gmail is blank. Please enter infor");
//             return;
//         }
//         else if (passwordRegisterInput.text.Trim() == "")
//         {
//             notificationControl.gameObject.SetActive(true);
//             notificationControl.SetText("Password is blank. Please enter infor");
//             return;
//         }
//         else if (confirmPasswordRegisterInput.text.Trim() == "")
//         {
//             notificationControl.gameObject.SetActive(true);
//             notificationControl.SetText("Confirmm password is blank. Please enter infor");
//             return;
//         }
//         RegisterData data = new RegisterData
//         {
//             Username = userNameRegisterInput.text,
//             Gmail = gmailRegisterInput.text,
//             Password = passwordRegisterInput.text,
//             ConfirmPassword = confirmPasswordRegisterInput.text
//         };
//         accountSaver.Register(data);

//     }
//     private void HandleRegister(RegisterState state)
//     {
//         if (state == RegisterState.ConfirmPasswordNotMatch)
//         {
//             notificationControl.gameObject.SetActive(true);
//             notificationControl.SetText("Not match two passwords. Please check them again");
//         }
//         else if (state == RegisterState.Success)
//         {
//             ShowLoginPage();
//             notificationControl.gameObject.SetActive(true);
//             notificationControl.SetText("Register new account successfully");
//         }
//         else if (state == RegisterState.DuplicateUserName)
//         {
//             notificationControl.gameObject.SetActive(true);
//             notificationControl.SetText("This username is registered. Please choose another one");
//         }
//         else if (state == RegisterState.DuplicateGmail)
//         {
//             notificationControl.gameObject.SetActive(true);
//             notificationControl.SetText("This gmail is registered. Please choose another one");
//         }
//         else if (state == RegisterState.ServerError)
//         {
//             notificationControl.gameObject.SetActive(true);
//             notificationControl.SetText("Server error");
//         }
//     }
//     public void ShowRegisterPage()
//     {
//         ResetInputInRegisterPage();
//         loginPage.SetActive(false);
//         registerPage.SetActive(true);
//     }
//     public void ShowLoginPage()
//     {
//         ResetInputInLoginPage();
//         registerPage.SetActive(false);
//         loginPage.SetActive(true);
//     }
//     private void ResetInputInRegisterPage()
//     {
//         userNameRegisterInput.text = "";
//         gmailRegisterInput.text = "";
//         passwordRegisterInput.text = "";
//         confirmPasswordRegisterInput.text = "";
//     }
//     private void ResetInputInLoginPage()
//     {
//         string tempStr = PlayerPrefs.GetString("UserName");
//         userNameLoginInput.text = tempStr;
//         passwordLoginInput.text = "";
//     }


//     public void LogOut()
//     {
//         userNameDisplay.text="";
//         string tempStr = PlayerPrefs.GetString("UserName");
//         accountSaver.ChangeActivityState(tempStr,false);
//         mainMenuPage.SetActive(false);
//         ShowLoginPage();
//     }

// }
