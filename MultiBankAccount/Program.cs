﻿#region USER ACCOUNT
int accountIndex = -1;
int pinIndex = -2;
string userAccount = "";
string userPin = "";
int loginTries = 0;

bool userExists = false;
bool positionCorrect = false;

int currentUser = -1;

List<string> accountList = new List<string>
{
    "1122334455",
    "2233445566",
    "3344556677",
    "4455667788",
    "5566778899"
};

List<string> pinList = new List<string>
{
    "1234",
    "2345",
    "3456",
    "4567",
    "5678"
};
#endregion

#region TRANSACTIONS
const decimal MAX_INCOME = 5000;
const decimal MAX_OUTCOME = 2000;
const int EXIT_NUMBER = 7;
string? input;
int optionInt;
bool exit = false;
decimal currentMoney = 0;

List<List<string>> accountsMovementList = new List<List<string>>
{
    new List<string> { "+345 | 20/10/2024 14:48:05", "-67 | 15/09/2024 14:40:05", "+82 | 27/09/2024 17:12:05", "+14 | 05/10/2024 20:10:05", "-300 | 18/10/2024 13:50:05", "+68 | 20/10/2024 10:30:05" },
    new List<string> { "-55 | 20/10/2024 14:48:05", "-72 | 15/09/2024 14:40:05", "+12 | 27/09/2024 17:12:05", "-76", "-90 | 18/10/2024 13:50:05" },
    new List<string> { "+268 | 20/10/2024 14:48:05", "-100 | 15/09/2024 14:40:05", "+240 | 27/09/2024 17:12:05", "-125 | 05/10/2024 15:50:05", "-56 | 13/10/2024 10:50:05", "-95 | 18/10/2024 13:50:05", "+130 | 20/10/2024 10:30:05" },
    new List<string> { "+20 | 20/10/2024 14:48:05", "-13 | 15/09/2024 14:40:05", "+10 | 27/09/2024 17:12:05", "+30 | 05/10/2024 10:10:05" },
    new List<string> { "-270 | 20/10/2024 14:48:05", "+990 | 15/09/2024 14:40:05", "+432 | 27/09/2024 17:12:05", "+365", "-300 | 20/10/2024 10:30:05" }
};

List<decimal> accountsMoneyList = new List<decimal>
{
    980.78m,
    0,
    437.23m,
    55.5m,
    1589.1m
};


#endregion

//Output encoding
Console.OutputEncoding = System.Text.Encoding.UTF8;

#region USER LOGIN
do {
    Console.Clear();

    if (loginTries > 0 && (!userExists || !positionCorrect)) Console.WriteLine("Insert valid account number and pin:");

    Console.WriteLine("========= USER SIGN IN =========");

    Console.WriteLine("Account number (XXXXXXXXXX): ");

    Console.Write("=> ");
    input = Console.ReadLine()?.Trim();

    foreach (string account in accountList)
    {
        if (input != null && input.Equals(account))
        {
            userAccount = account;
            accountIndex = accountList.IndexOf(account);
            break;
        }
    }
    //Console.WriteLine("----- " + userAccount + "----" + accountIndex);

    Console.WriteLine("Pin (XXXX): ");

    Console.Write("=> ");
    input = Console.ReadLine()?.Trim();

    foreach (string pin in pinList)
    {
        if (input != null && input.Equals(pin)) 
        {
            userPin = pin;
            pinIndex = pinList.IndexOf(pin);
            break;
        }
    }

    //Console.WriteLine("----- " + userPin + "----" + pinIndex);

    if (!userAccount.Equals("") && !userPin.Equals("")) userExists = true;
    else userExists = false;

    if (pinIndex == accountIndex)
    {
        positionCorrect = true;
        currentUser = pinIndex; //Assign current user index
    }
    else positionCorrect = false;

    loginTries++;

} while (!userExists || !positionCorrect);

//Assign current user money and movements
List<string> movementList = accountsMovementList[currentUser];
currentMoney = accountsMoneyList[currentUser];
#endregion

#region ACCOUNT MOVEMENTS
do
{
    Console.Clear();

    Console.WriteLine($@"
==============================================================
|| Hello USER{currentUser+20}-{userAccount}!                                 ||
|| Welcome to your bank account! What do you want to do?    ||
||                                                          ||
||	1 - Money Income                                    ||
||	2 - Money Outcome                                   ||
||	3 - List all movements                              ||
||	4 - List Incomes                                    ||
||	5 - List Outcomes                                   ||
||	6 - Show current money                              ||
||	{EXIT_NUMBER} - Exit                                            ||
||                                                          ||
|| Please choose an option:                                 ||
==============================================================");

    Console.Write("=> ");
    input = Console.ReadLine()?.Trim();

    if (input != null && input.Length == 1 && input[0] >= '1' && input[0] <= '7' && int.TryParse(input, out optionInt))
    {
        optionInt = int.Parse(input);
        Console.WriteLine("Option chosen: " + optionInt);


        if (optionInt != EXIT_NUMBER)
        {
            switch (optionInt)
            {
                case 1:
                    Console.WriteLine("=================================================" +
                        "Please write how much money you want to deposit:");
                    decimal income = 0;

                    do
                    {
                        Console.Write("=> ");
                        input = Console.ReadLine()?.Trim().Replace(".", ",");

                        if (input != null && input.Length > 0 && decimal.TryParse(input, out income))
                        {
                            if (income <= MAX_INCOME)
                            {
                                currentMoney += income;
                                movementList.Add("+" + income + " | " + DateTime.Now);
                                Console.WriteLine($"{income}€ were added to your account.\n" +
                                    $"Current money: {currentMoney:0.00}€");
                            }
                            else Console.WriteLine($"ERROR: The top amount you can deposit is {MAX_INCOME:0.00}€. Try again.");
                        }
                        else
                        {
                            Console.WriteLine(
                                "ERROR: Please write a numeric value: ");
                        }
                    } while (!decimal.TryParse(input, out income) || income > MAX_INCOME);
                    break;

                case 2:
                    decimal outcome = 0;
                    Console.WriteLine("=================================================" +
                        "Please write how much money you want to withdraw:");

                    do
                    {
                        Console.Write("=> ");
                        input = Console.ReadLine()?.Trim().Replace(".", ",");

                        if (input != null && input.Length > 0 && decimal.TryParse(input, out outcome))
                        {
                            outcome = Math.Abs(decimal.Parse(input));

                            if (outcome <= currentMoney)
                            {
                                currentMoney -= outcome;
                                movementList.Add("-" + outcome + " | " + DateTime.Now);
                                Console.WriteLine($"{outcome:0.00}€ were subtracted from account.\n" +
                                    $"Current money: {currentMoney:0.00}€");
                            }
                            else
                            {
                                Console.WriteLine(
                                    "ERROR: You cannot withdraw this amount of money. Try a lower value.\n" +
                                    $"Money available: {currentMoney:0.00}€\n" +
                                    "Amount to withdraw: ");
                            }
                        }
                        else
                        {
                            Console.WriteLine(
                                "ERROR: Please write a numeric value: ");
                        }
                    } while (!decimal.TryParse(input, out outcome) || outcome > MAX_OUTCOME || outcome > currentMoney);

                    break;

                case 3:
                    Console.WriteLine("=================================================\n" +
                        "These are all your movements:\n" +
                        "-------------------------------------------------");

                    foreach (string movement in movementList)
                    {
                        Console.WriteLine(movement);
                    }
                    movementList.Add(">> Movement List printed | " + DateTime.Now);
                    break;

                case 4:
                    Console.WriteLine("=================================================\n" +
                        "These are all your incomes:\n" +
                        "-------------------------------------------------");

                    foreach (string movement in movementList)
                    {
                        if (movement.StartsWith('+')) Console.WriteLine(movement);
                    }
                    movementList.Add(">> Income List printed | " + DateTime.Now);
                    break;

                case 5:
                    Console.WriteLine("=================================================\n" +
                        "These are all your outcomes:\n" +
                        "-------------------------------------------------");

                    foreach (string movement in movementList)
                    {
                        if (movement.StartsWith('-')) Console.WriteLine(movement);
                    }
                    movementList.Add(">> Outcome List printed | " + DateTime.Now);
                    break;

                case 6:
                    Console.WriteLine($"Current money: {currentMoney:0.00}€");
                    movementList.Add(">> Account Money consulted | " + DateTime.Now);
                    break;

            }

            Console.WriteLine("Do you wish to make another operation?\n" +
                "Write 'yes' if so or type anything else to exit: ");
            Console.Write("=> ");
            input = Console.ReadLine()?.Trim();
            if (input != null && input.Length > 0 && !input.ToLower().Equals("yes"))
            {
                exit = true;
            }
            else exit = false;
        }
        else
        {
            exit = true;
            Console.WriteLine($"Current money: {currentMoney:0.00}€");
            Console.WriteLine("Thanks for using our services. Goodbye!");
        }
    }
    else
    {
        Console.WriteLine("ERROR: Invalid option, try again.");
    }

} while (!exit);
#endregion
