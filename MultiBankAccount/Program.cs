﻿#region TRANSACTIONS
using System.Diagnostics;
using System.Net.NetworkInformation;

decimal currentMoney = 0;
string? input;
int optionInt;
bool exit = false;
List<string> movementList = new List<string>();
#endregion

#region USER ACCOUNT
int accountIndex = -1;
int pinIndex = -2;
string userAccount = "";
string userPin = "";
bool userExists = false;
bool positionCorrect = false;

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


Console.OutputEncoding = System.Text.Encoding.UTF8;

#region USER LOGIN
do {
    Console.WriteLine("Account number (XXXXXXXXXX): ");

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
    Console.WriteLine("----- " + userAccount + "----" + accountIndex);

    Console.WriteLine("Pin (XXXX): ");

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

    Console.WriteLine("----- " + userPin + "----" + pinIndex);

    if (!userAccount.Equals("") && !userPin.Equals("")) userExists = true;
    else userExists = false;

    if (pinIndex == accountIndex) positionCorrect = true;
    else positionCorrect = false;

    if(!userExists || !positionCorrect)
    {
        Console.WriteLine("Insert valid account number and pin:");
    }

} while (!userExists || !positionCorrect);

#endregion

do
{
    //Console.Clear();

    Console.WriteLine(@"Welcome to your bank account! What do you want to do?

	1 - Money Income
	2 - Money Outcome
	3 - List all movements
	4 - List Incomes
	5 - List Outcomes
	6 - Show current money
	7 - Exit" +
    "\n\nPlease write the option's number: ");

    input = Console.ReadLine()?.Trim();

    if (input != null && input.Length == 1 && input[0] >= '1' && input[0] <= '7' && int.TryParse(input, out optionInt))
    {
        optionInt = int.Parse(input);
        Console.WriteLine("Option chosen: " + optionInt);


        if (optionInt != 7)
        {
            switch (optionInt)
            {
                case 1:
                    Console.WriteLine("Please write how much money you want to deposit:");

                    decimal income = 0;

                    do
                    {
                        input = Console.ReadLine()?.Trim();

                        if (input != null && input.Length > 0 && decimal.TryParse(input, out income))
                        {
                            income = Math.Abs(decimal.Parse(input));
                            currentMoney += income;
                            movementList.Add("+" + income + " | " + DateTime.Now);
                            Console.WriteLine(income + " $ were added to your account.");
                        }
                        else
                        {
                            Console.WriteLine(
                                "ERROR: Please write a numeric value: ");
                        }
                    } while (!decimal.TryParse(input, out income));
                    break;

                case 2:
                    decimal outcome = 0;
                    Console.WriteLine("Please write how much money you want to withdraw:");

                    do
                    {
                        input = Console.ReadLine()?.Trim();

                        if (input != null && input.Length > 0 && decimal.TryParse(input, out outcome))
                        {
                            outcome = Math.Abs(decimal.Parse(input));

                            if (outcome <= currentMoney)
                            {
                                currentMoney -= outcome;
                                movementList.Add("-" + outcome + " | " + DateTime.Now);
                                Console.WriteLine(outcome + " $ were subtracted from account.");
                            }
                            else
                            {
                                Console.WriteLine(
                                    "ERROR: You cannot withdraw this amount of money. Try a lower value.\n" +
                                    $"Money available: {currentMoney:0.00} €\n" +
                                    "Amount to withdraw: ");
                            }
                        }
                        else
                        {
                            Console.WriteLine(
                                "ERROR: Please write a numeric value: ");
                        }
                    } while (outcome > currentMoney || !decimal.TryParse(input, out outcome));

                    break;

                case 3:
                    Console.WriteLine("These are all your movements:\n");

                    foreach (string movement in movementList)
                    {
                        Console.WriteLine(movement);
                    }
                    movementList.Add("Movement List printed | " + DateTime.Now);
                    break;

                case 4:
                    Console.WriteLine("These are all your incomes:\n");

                    foreach (string movement in movementList)
                    {
                        if (movement.StartsWith('+')) Console.WriteLine(movement);
                    }
                    movementList.Add("Income List printed | " + DateTime.Now);
                    break;

                case 5:
                    Console.WriteLine("These are all your incomes:\n");

                    foreach (string movement in movementList)
                    {
                        if (movement.StartsWith('-')) Console.WriteLine(movement);
                    }
                    movementList.Add("Outcome List printed | " + DateTime.Now);
                    break;

                case 6:
                    Console.WriteLine($"Current money: {currentMoney:0.00} €");
                    movementList.Add("Current Money consulted | " + DateTime.Now);
                    break;

            }

            Console.WriteLine("Do you wish to make another operation?\n" +
                "Write yes/no: ");
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
            Console.WriteLine($"Current money: {currentMoney:0.00} €");
            Console.WriteLine("Thanks for using our services. Goodbye!");
        }
    }
    else
    {
        Console.WriteLine("ERROR: Invalid option, try again.");
    }

} while (!exit);
