using System;
using System.Collections.Generic;
using TenmoClient.APIClients;
using TenmoClient.Data;

namespace TenmoClient
{
    public class UserInterface
    {
        private readonly ConsoleService consoleService = new ConsoleService();
        private readonly AuthService authService = new AuthService();
        private readonly AccountService accountService = new AccountService();
        private readonly ProfileService profileService = new ProfileService();
        private readonly BalanceTransferService balanceTransferService = new BalanceTransferService();
        private readonly TransferService transferService = new TransferService();

        private bool shouldExit = false;

        public void Start()
        {
            while (!shouldExit)
            {
                while (!authService.IsLoggedIn)
                {
                    ShowLogInMenu();
                }

                // If we got here, then the user is logged in. Go ahead and show the main menu
                ShowMainMenu();
            }
        }
        private void LogAllServices()
        {
            this.authService.LoginAllServices(this.accountService, this.profileService, this.balanceTransferService, this.transferService);
            
        }
        private void LogOutAllServices()
        {
            this.accountService.Login(null);
            this.profileService.Login(null);
            this.balanceTransferService.Login(null);
            this.transferService.Login(null);

        }

        private void ShowLogInMenu()
        {
            Console.WriteLine("Welcome to TEnmo!");
            Console.WriteLine("1: Login");
            Console.WriteLine("2: Register");
            Console.Write("Please choose an option: ");

            if (!int.TryParse(Console.ReadLine(), out int loginRegister))
            {
                Console.WriteLine("Invalid input. Please enter only a number.");
            }
            else if (loginRegister == 1)
            {
                HandleUserLogin();
                LogAllServices();

            }
            else if (loginRegister == 2)
            {
                HandleUserRegister();
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        private void ShowMainMenu()
        {
            int menuSelection = -1;
            while (menuSelection != 0)
            {
                Console.WriteLine();
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers");
                Console.WriteLine("3: View your pending requests");
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks");
                Console.WriteLine("6: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else
                {
                    switch (menuSelection)
                    {
                        case 1:
                            DisplayAccountBalance();
                            break;
                        case 2:
                            DisplayListOfTransfers();
                            break;
                        case 3:
                            DisplayListOfPendingTransfers();
                            break;
                        case 4:
                            DisplayListOfUsers();
                            CreateNewTransfer();
                            break;
                        case 5:
                            RequestTransfer(); // TODO: revise and finish request transfer method
                            break;
                        case 6:
                            UserService.SetLogin(new API_User()); //wipe out previous login info
                            ShowLogInMenu();
                            return;
                        default:
                            Console.WriteLine("Goodbye!");
                            shouldExit = true;
                            return;
                    }
                }
            }
        }

        private void HandleUserRegister()
        {
            bool isRegistered = false;

            while (!isRegistered) //will keep looping until user is registered
            {
                LoginUser registerUser = consoleService.PromptForLogin();
                isRegistered = authService.Register(registerUser);
            }

            Console.WriteLine("");
            Console.WriteLine("Registration successful. You can now log in.");
        }

        private void HandleUserLogin()
        {
            while (!UserService.IsLoggedIn) //will keep looping until user is logged in
            {
                LoginUser loginUser = consoleService.PromptForLogin();
                API_User user = authService.Login(loginUser);
                if (user != null)
                {
                    UserService.SetLogin(user);
                }
            }
        }

        private void DisplayAccountBalance()
        {
            API_Account account = accountService.GetAccount(UserService.UserId);

            if (account == null)
            {
                Console.WriteLine("There was an error displaying the Balance.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine($"Your current account balance is: ${account.Balance}");
            }

        }

        private void DisplayListOfUsers()
        {
            List<API_User> users = profileService.GetAllUsers();

            Console.WriteLine("-------------------------------------------");
            Console.WriteLine(" Users ID                    Name          ");
            Console.WriteLine("-------------------------------------------");

            foreach (API_User user in users)
            {
                Console.WriteLine(user.UserId + "\t" + user.Username);
            }
        }

        private void DisplayListOfTransfers()
        {
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine(" Transfers ID      From/To        Amount   ");
            Console.WriteLine("-------------------------------------------");

            List<API_Transfer> usersTransfers = transferService.GetAllTransfers(UserService.UserId);

            List<API_User> userList = profileService.GetAllUsers();

            Dictionary<int, string> userDict =
    new Dictionary<int, string>();

            foreach (API_User user in userList)
            {
                userDict.Add(user.UserId, user.Username);
            }

            foreach (API_Transfer t in usersTransfers)
            {
                Console.WriteLine();
                Console.Write(t.TransferId + "\t\t");
                Console.Write("From: " + userDict[t.AccountFrom] + "/" + "To: " + userDict[t.AccountTo] + "\t");
                Console.Write("$" + t.Amount);
            }
            Console.WriteLine();

            Console.WriteLine("Please enter transfer ID to view details (0 to cancel):");
            int transferIdInput = int.Parse(Console.ReadLine());

            if (transferIdInput == 0)
            {
                //ShowMainMenu();
            }
            else
            {
                DisplaySpecificTransfer(transferIdInput, userDict);
            }

        }

        private void DisplaySpecificTransfer(int transferIdInput, Dictionary<int, string> userDict)
        {
            Console.WriteLine("---------------------");
            Console.WriteLine(" Transfers Details   ");
            Console.WriteLine("---------------------");

            API_Transfer usersTransfer = transferService.GetSpecificTransfer(transferIdInput);

            //displays Transfer details
            Console.WriteLine();
            Console.WriteLine($"Id: {usersTransfer.TransferId}");
            Console.WriteLine($"From: {userDict[usersTransfer.AccountFrom]}");
            Console.WriteLine($"To: {userDict[usersTransfer.AccountTo]}");

            if (usersTransfer.TransferType == 1)
            {
                Console.WriteLine("Type: Request");
            }
            else
            {
                Console.WriteLine("Type: Send");
            }

            if (usersTransfer.TransferStatus == 1)
            {
                Console.WriteLine("Status: Pending");
                Console.WriteLine($"Amount: ${usersTransfer.Amount}");
                Console.WriteLine();
                Console.WriteLine("Do you want to approve this transfer?: Y/N (or press any other key to exit and return to the main menu)");
                string approvalAnswer = Console.ReadLine();
                if(approvalAnswer.ToLower() == "y")
                {
                    BalanceTransfer newbalanceTransfer = new BalanceTransfer(usersTransfer.AccountFrom, usersTransfer.AccountTo, usersTransfer.Amount, 1, 2, transferIdInput);
                    balanceTransferService.AcceptOrDeclineTransfer(newbalanceTransfer);

                } 
                else if(approvalAnswer.ToLower() == "n")
                {
                    BalanceTransfer newbalanceTransfer = new BalanceTransfer(usersTransfer.AccountFrom, usersTransfer.AccountTo, usersTransfer.Amount, 1, 3, transferIdInput);
                    balanceTransferService.AcceptOrDeclineTransfer(newbalanceTransfer);
                }

            }
            else if (usersTransfer.TransferStatus == 2)
            {
                Console.WriteLine("Status: Approved");
                Console.WriteLine($"Amount: ${usersTransfer.Amount}");

            }
            else
            {
                Console.WriteLine("Status: Rejected");
                Console.WriteLine($"Amount: ${usersTransfer.Amount}");
            }

            Console.ReadLine();
            //ShowMainMenu();
        }


        private void DisplayListOfPendingTransfers()
        {

            Console.WriteLine("-------------------------------------------");
            Console.WriteLine(" Transfers ID      From/To        Amount   ");
            Console.WriteLine("-------------------------------------------");

            List<API_Transfer> usersTransfers = transferService.GetAllTransfers(UserService.UserId);

            List<API_User> userList = profileService.GetAllUsers();

            Dictionary<int, string> userDict =
    new Dictionary<int, string>();

            foreach (API_User user in userList)
            {
                userDict.Add(user.UserId, user.Username);
            }

            foreach (API_Transfer t in usersTransfers)
            {
                if (t.TransferStatus == 1)
                {
                    Console.WriteLine();
                    Console.Write(t.TransferId + "\t\t");
                    Console.Write("From: " + userDict[t.AccountFrom] + "/" + "To: " + userDict[t.AccountTo] + "\t");
                    Console.Write("$" + t.Amount);
                }
            }
            Console.WriteLine();

            Console.WriteLine("Please enter transfer ID to approve transfer or view details (0 to cancel):");
            int transferIdInput = int.Parse(Console.ReadLine());

            if (transferIdInput == 0)
            {
                //ShowMainMenu();

            }
            else
            {
                DisplaySpecificTransfer(transferIdInput, userDict);
            }


        }

        private void CreateNewTransfer()
        {
            Console.WriteLine("Enter ID of user you are sending to(0 to cancel):");
            int transferToWhom = int.Parse(Console.ReadLine());

            if (transferToWhom == 0)
            {
            }
            else
            {
                Console.WriteLine("Enter amount:");
                decimal transferAmount = decimal.Parse(Console.ReadLine());

                // if the amount to transfer is less than what we have in the bank
                if (transferAmount <= accountService.GetAccount(UserService.UserId).Balance)
                {
                    BalanceTransfer newbalanceTransfer = new BalanceTransfer(UserService.UserId, transferToWhom, transferAmount);
                    balanceTransferService.UpdateAccountBalance(newbalanceTransfer);

                }
                else
                {
                    Console.WriteLine("Error: Insufficient funds in your account");
                }
            }
        }
        
        private void RequestTransfer()
        {
            DisplayListOfUsers();
            Console.WriteLine();
            Console.WriteLine("Who do you want to request money from?(0 to cancel):");
            int requestFromWhom = int.Parse(Console.ReadLine());
            if (requestFromWhom == 0)
            {
                //ShowMainMenu();
            }
            else
            {
                Console.WriteLine("Enter amount:");
                decimal transferAmount = decimal.Parse(Console.ReadLine());

                BalanceTransfer newbalanceTransfer = new BalanceTransfer(requestFromWhom, UserService.UserId, transferAmount);
                balanceTransferService.RequestTransfer(newbalanceTransfer);


                }

            }

        }

    }

