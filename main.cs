using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


class Program
{
    static int availableSlots;
    static List<string> parkedVehicles;

    static void Main(string[] args)
    {
        Console.WriteLine("Enter the number of parking slots:");
        int.TryParse(Console.ReadLine(), out availableSlots);

        parkedVehicles = new List<string>(new string[availableSlots]);

        while (true)
        {
            Console.WriteLine("Enter a command:");
            string input = Console.ReadLine();
            string[] commandArgs = input.Split(' ');

            string command = commandArgs[0];

            switch (command)
            {
                case "create_parking_lot":
                    CreateParkingLot(commandArgs);
                    break;
                case "park":
                    ParkVehicle(commandArgs);
                    break;
                case "leave":
                    LeaveParkingLot(commandArgs);
                    break;
                case "status":
                    PrintStatus();
                    break;
                case "type_of_vehicles":
                    TypeOfVehicles(commandArgs);
                    break;
                case "registration_numbers_for_vehicles_with_odd_plate":
                    RegistrationNumbersForVehiclesWithOddPlate();
                    break;
                case "registration_numbers_for_vehicles_with_even_plate":
                    RegistrationNumbersForVehiclesWithEvenPlate();
                    break;
                case "registration_numbers_for_vehicles_with_color":
                    RegistrationNumbersForVehiclesWithColor(commandArgs);
                    break;
                case "slot_numbers_for_vehicles_with_color":
                    SlotNumbersForVehiclesWithColor(commandArgs);
                    break;
                case "slot_number_for_registration_number":
                    SlotNumberForRegistrationNumber(commandArgs);
                    break;
                case "exit":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid command.");
                    break;
            }
        }
    }

    static void CreateParkingLot(string[] args)
    {
        int.TryParse(args[1], out availableSlots);
        parkedVehicles = new List<string>(new string[availableSlots]);
        Console.WriteLine("Created a parking lot with {0} slots", availableSlots);
    }

    static void ParkVehicle(string[] args)
    {
        bool isParked = false;
        string plateNumber = args[1];
        string color = args[2];
        string vehicleType = args[3];
        for (int i = 0; i < availableSlots; i++)
        {
            if (parkedVehicles[i] == null)
            {
                parkedVehicles[i] = $"{plateNumber} {color} {vehicleType}";
                Console.WriteLine("Allocated slot number: {0}", i + 1);
                isParked = true;
                break;
            }
        }
        if (!isParked)
        {
            Console.WriteLine("Parking lot is full.");
        }
    }

    static void LeaveParkingLot(string[] args)
    {
        int slotNumber;
        int.TryParse(args[1], out slotNumber);

        if (slotNumber > 0 && slotNumber <= availableSlots && parkedVehicles[slotNumber - 1] != null)
        {
            parkedVehicles[slotNumber - 1] = null;
            Console.WriteLine("Slot number {0} is free", slotNumber);
        }
        else
        {
            Console.WriteLine("Slot number is not valid.");
        }
    }

    static void PrintStatus()
    {
        Console.WriteLine("No.  Registration No   Color   Vehicle Type");
        for (int i = 0; i < availableSlots; i++)
        {
            if (parkedVehicles[i] != null)
            {
                string[] vehicle = parkedVehicles[i].Split(' ');
                Console.WriteLine("{0}    {1}     {2}    {3}", i + 1, vehicle[0], vehicle[1], vehicle[2]);
            }
        }
    }

   static void TypeOfVehicles(string[] args)
    {
        int typeCount = 0;
        string type = args[1];

        for (int i = 0; i < availableSlots; i++)
        {
            if (parkedVehicles[i] != null)
            {
                string[] vehicle = parkedVehicles[i].Split(' ');
                if (vehicle[2] == type)
                {
                    typeCount++;
                }
            }
        }

        Console.WriteLine("Total number of {0}: {1}", type, typeCount);
    }

    static void RegistrationNumbersForVehiclesWithColor(string[] args)
    {
        string color = args[1];
        List<string> regNumbers = new List<string>();

        for (int i = 0; i < availableSlots; i++)
        {
            if (parkedVehicles[i] != null)
            {
                string[] vehicle = parkedVehicles[i].Split(' ');
                if (vehicle[1].ToLower() == color.ToLower())
                {
                    regNumbers.Add(vehicle[0]);
                }
            }
        }

        if (regNumbers.Count == 0)
        {
            Console.WriteLine("No vehicle with color {0} is parked in the parking lot.", color);
        }
        else
        {
            Console.WriteLine("Registration numbers for vehicles with color {0}:", color);
            Console.WriteLine(string.Join(", ", regNumbers));
        }
    }

    static void SlotNumbersForVehiclesWithColor(string[] args)
    {
        string color = args[1];
        List<int> slotNumbers = new List<int>();

        for (int i = 0; i < availableSlots; i++)
        {
            if (parkedVehicles[i] != null)
            {
                string[] vehicle = parkedVehicles[i].Split(' ');
                if (vehicle[1].ToLower() == color.ToLower())
                {
                    slotNumbers.Add(i + 1);
                }
            }
        }

        if (slotNumbers.Count == 0)
        {
            Console.WriteLine("No vehicle with color {0} is parked in the parking lot.", color);
        }
        else
        {
            Console.WriteLine("Slot numbers for vehicles with color {0}:", color);
            Console.WriteLine(string.Join(", ", slotNumbers));
        }
    }

    static void SlotNumberForRegistrationNumber(string[] args)
    {
        string regNumber = args[1];
        int slotNumber = -1;

        for (int i = 0; i < availableSlots; i++)
        {
            if (parkedVehicles[i] != null)
            {
                string[] vehicle = parkedVehicles[i].Split(' ');
                if (vehicle[0] == regNumber)
                {
                    slotNumber = i + 1;
                    break;
                }
            }
        }

        if (slotNumber == -1)
        {
            Console.WriteLine("Registration number {0} not found.", regNumber);
        }
        else
        {
            Console.WriteLine("Slot number for registration number {0}: {1}", regNumber, slotNumber);
        }
    }

    static void RegistrationNumbersForVehiclesWithOddPlate()
    {
        List<string> regNumbers = new List<string>();

        for (int i = 0; i < availableSlots; i++)
        {
            if (parkedVehicles[i] != null)
            {
                string[] vehicle = parkedVehicles[i].Split(' ');
                string plateNumber = vehicle[0];

                string lastDigitStr = Regex.Match(plateNumber, @"\d(?!\d)").Value;
                int lastDigit;
                if (!int.TryParse(lastDigitStr, out lastDigit))
                {
                    continue;
                }

                if (lastDigit % 2 == 1)
                {
                    regNumbers.Add(plateNumber);
                }
            }
        }

        if (regNumbers.Count == 0)
        {
            Console.WriteLine("No vehicle with odd-numbered plate is parked in the parking lot.");
        }
        else
        {
            Console.WriteLine("Registration numbers for vehicles with odd-numbered plate:");
            Console.WriteLine(string.Join(", ", regNumbers));
        }
    }



    static void RegistrationNumbersForVehiclesWithEvenPlate()
    {
        List<string> regNumbers = new List<string>();

        for (int i = 0; i < availableSlots; i++)
        {
            if (parkedVehicles[i] != null)
            {
                string[] vehicle = parkedVehicles[i].Split(' ');
                string plateNumber = vehicle[0];

                string lastDigitStr = Regex.Match(plateNumber, @"\d(?!\d)").Value;
                int lastDigit;
                if (!int.TryParse(lastDigitStr, out lastDigit))
                {
                    continue;
                }

                if (lastDigit % 2 == 0)
                {
                    regNumbers.Add(plateNumber);
                }
            }
        }

        if (regNumbers.Count == 0)
        {
            Console.WriteLine("No vehicle with even-numbered plate is parked in the parking lot.");
        }
        else
        {
            Console.WriteLine("Registration numbers for vehicles with even-numbered plate:");
            Console.WriteLine(string.Join(", ", regNumbers));
        }
    }
}
