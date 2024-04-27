using MySql.Data.MySqlClient;
using Org.BouncyCastle.Pqc.Crypto.Falcon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using System.IO;

namespace FulopKrisztianHaziFeladat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public static class ControlRepository
    {
        public static TextBlock[] TextBlocks { get; private set; }
        public static TextBox[] TextBoxes { get; private set; }
        public static PasswordBox[] PasswordBoxes { get; private set; }

        public static void InitializeControls(TextBlock[] textBlocks, TextBox[] textBoxes, PasswordBox[] passwordBoxes)
        {
            TextBlocks = textBlocks;
            TextBoxes = textBoxes;
            PasswordBoxes = passwordBoxes;
        }
    }

    public partial class MainWindow : Window
    {
        Functions func = new Functions();

        private string UserName = string.Empty;
        private string UserPassword = string.Empty;
        private string UserEmail = string.Empty;
        private int ID = 0;
        

        public MainWindow()
        {
            InitializeComponent();
            TextBlock[] textBlocks = { RegNameError, RegEmailError, RegPassError, LogNameError, LogPassError };
            TextBox[] textBoxes = { RegistrationNameInput, RegistrationEmailInput, LoginNameInput };
            PasswordBox[] passwordBoxes = { RegistrationPasswordInput, LoginPasswordInput };
            ControlRepository.InitializeControls(textBlocks, textBoxes, passwordBoxes);
            LoginNameInput.Focus();
            StatButton.IsEnabled = false;
            MainPage.Visibility = Visibility.Visible;
            RegistrationPage.Visibility = Visibility.Hidden;
            PopUpWindow.Visibility = Visibility.Hidden;
            func.RegErrorOff(ControlRepository.TextBlocks, ControlRepository.TextBoxes, ControlRepository.PasswordBoxes);
            if (File.Exists("ID.txt"))
            {
                ID = Convert.ToInt32(File.ReadAllText("ID.txt"));
            }
            else
            {
                File.WriteAllText("ID.txt", ID.ToString());
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowChrome.SetWindowChrome(this, new WindowChrome()
            {
                CaptionHeight = 1,
                CornerRadius = new CornerRadius(30),
                GlassFrameThickness = new Thickness(0),
                ResizeBorderThickness = new Thickness(6)
            });
        }

        //Close
        private void CloseFunction(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //Login
        private void LoginFunction(object sender, RoutedEventArgs e)
        {
            UserName = LoginNameInput.Text;
            UserPassword = LoginPasswordInput.Password;

            DatabaseHelper.AuthenticationResult result = DatabaseHelper.AuthenticateUser(UserName, UserPassword);

            switch (result)
            {
                case DatabaseHelper.AuthenticationResult.Success:
                    func.RegErrorOff(ControlRepository.TextBlocks, ControlRepository.TextBoxes, ControlRepository.PasswordBoxes);
                    LoginNameInput.Text = string.Empty;
                    LoginPasswordInput.Password = string.Empty;
                    PopUpWindow.Visibility = Visibility.Visible;
                    PopUpWindowText.Text = "Sikeres bejelentkezés!";
                    StatButton.IsEnabled = true;
                    break;
                case DatabaseHelper.AuthenticationResult.InvalidUsername:
                    LogNameError.Visibility = Visibility.Visible;
                    LogNameError.Text = "Hibás felhasználónév!";
                    func.ErrorColor(LoginNameInput);
                    break;
                case DatabaseHelper.AuthenticationResult.InvalidPassword:
                    LogPassError.Text = "Hibás jelszó!";
                    LogPassError.Visibility = Visibility.Visible;
                    func.ErrorColorPass(LoginPasswordInput);
                    break;
                case DatabaseHelper.AuthenticationResult.Error:
                    PopUpWindow.Visibility = Visibility.Visible;
                    PopUpWindowText.Text = "Hiba történt az autentikáció során.";
                    break;
            }

        }

        //Registration
        private void RegistrationFunction(object sender, RoutedEventArgs e)
        {
            MainPage.Visibility = Visibility.Hidden;
            RegistrationPage.Visibility = Visibility.Visible;
            func.RegErrorOff(ControlRepository.TextBlocks, ControlRepository.TextBoxes, ControlRepository.PasswordBoxes);

        }

        //Upload User Data
        private void RegFunction(object sender, RoutedEventArgs e)
        {
            int correct = 0;
            UserName = RegistrationNameInput.Text;
            UserEmail = RegistrationEmailInput.Text;
            UserPassword = RegistrationPasswordInput.Password;

            //Ellenőrzés 1
            switch (func.CheckUserName(UserName)) //Név
            {
                case 0:
                    RegNameError.Visibility = Visibility.Hidden;
                    RegNameError.Text = string.Empty;
                    func.GoodColor(RegistrationNameInput);
                    correct++;
                    break;
                case 1:
                    RegNameError.Visibility = Visibility.Visible;
                    RegNameError.Text = "Üres a mező!";
                    func.ErrorColor(RegistrationNameInput);
                    break; 
            }

            switch (func.CheckUserPassword(UserPassword)) //Jelszó
            {
                case 0:
                    RegPassError.Visibility = Visibility.Hidden;
                    RegPassError.Text = string.Empty;
                    func.GoodColorPass(RegistrationPasswordInput);
                    correct++;
                    break;
                case 1:
                    RegPassError.Visibility = Visibility.Visible;
                    RegPassError.Text = "A jelszónak legalább 8 karakternek kell lennie!";
                    func.ErrorColorPass(RegistrationPasswordInput);
                    break; 
                case 2:
                    RegPassError.Visibility = Visibility.Visible;
                    RegPassError.Text = "A jelszónak tartalmaznia kell legalább egy nagy betűt!";
                    func.ErrorColorPass(RegistrationPasswordInput);
                    break;
                case 3:
                    RegPassError.Visibility = Visibility.Visible;
                    RegPassError.Text = "A jelszó legalább egy számot kell tartalmaznia!";
                    func.ErrorColorPass(RegistrationPasswordInput);
                    break; 
            }

            switch (func.CheckUserEmail(UserEmail)) //Email
            {
                case 0:
                    RegEmailError.Visibility = Visibility.Hidden;
                    RegEmailError.Text = string.Empty;
                    func.GoodColor(RegistrationEmailInput);
                    correct++;
                    break;
                case 1:
                    RegEmailError.Visibility = Visibility.Visible;
                    RegEmailError.Text = "Üres a mező!";
                    func.ErrorColor(RegistrationEmailInput);
                    break; 
                case 2:                                  
                    RegEmailError.Visibility = Visibility.Visible;
                    RegEmailError.Text = "Hibás az email";
                    func.ErrorColor(RegistrationEmailInput);
                    break; 
            }

            if (correct == 3 && DatabaseHelper.AddUserToDatabase(ID.ToString(), UserName, UserPassword, UserEmail))
            {
                PopUpWindow.Visibility = Visibility.Visible;
                PopUpWindowText.Text = "Sikeres regisztráció!";
                ID++;
                File.WriteAllText("ID.txt", ID.ToString());
                RegistrationNameInput.Text = string.Empty;
                RegistrationEmailInput.Text = string.Empty;
                RegistrationPasswordInput.Password = string.Empty;

                func.RegErrorOff(ControlRepository.TextBlocks, ControlRepository.TextBoxes, ControlRepository.PasswordBoxes);

            }
            else
            {
                PopUpWindow.Visibility = Visibility.Visible;
                PopUpWindowText.Text = "Hiba történt a regisztráció során.";
            }
        }
        //Vissza a főoldalra
        private void BackFunction(object sender, RoutedEventArgs e)
        {
            MainPage.Visibility = Visibility.Visible;
            RegistrationPage.Visibility = Visibility.Hidden;
            func.RegErrorOff(ControlRepository.TextBlocks, ControlRepository.TextBoxes, ControlRepository.PasswordBoxes);
        }

        //Statistic
        private void StatisticFunction(object sender, RoutedEventArgs e)
        {

        }

        private void OkayFunction(object sender, RoutedEventArgs e)
        {
            PopUpWindow.Visibility= Visibility.Hidden;
        }
    }
}
