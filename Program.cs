using System;
using System.Collections.Generic;
using System.Threading;

public enum VehicleType
{
    Mobil,
    Motor
}

public class Vehicle
{
    public string RegistrationNumber { get; }
    public string Color { get; }
    public VehicleType Type { get; }

    public Vehicle(string registrationNumber, string color, VehicleType type)
    {
        RegistrationNumber = registrationNumber;
        Color = color;
        Type = type;
    }
}

public class ParkingSlot
{
    public int SlotNumber { get; }
    public Vehicle? ParkedVehicle { get; private set; }

    public ParkingSlot(int slotNumber)
    {
        SlotNumber = slotNumber;
    }

    public void ParkVehicle(Vehicle vehicle)
    {
        ParkedVehicle = vehicle;
    }

    public void RemoveVehicle()
    {
        ParkedVehicle = null;
    }
}

public class ParkingSystem
{
    private List<ParkingSlot> parkingSlots;
    private int mobilCount;
    private int motorCount;

    public ParkingSystem(int totalSlots)
    {
        parkingSlots = new List<ParkingSlot>();
        for (int i = 0; i < totalSlots; i++)
        {
            parkingSlots.Add(new ParkingSlot(i + 1));
        }
    }

    public int FindAvailableSlot()
    {
        foreach (var slot in parkingSlots)
        {
            if (slot.ParkedVehicle == null)
            {
                return slot.SlotNumber;
            }
        }
        return -1;
    }

    public bool ParkVehicle(string registrationNumber, string color, VehicleType type)
    {
        int availableSlot = FindAvailableSlot();
        if (availableSlot != -1)
        {
            var vehicle = new Vehicle(registrationNumber, color, type);
            parkingSlots[availableSlot - 1].ParkVehicle(vehicle);
            Console.WriteLine($"Allocated slot number: {availableSlot}");

            if (type == VehicleType.Mobil)
            {
                mobilCount++;
            }
            else if (type == VehicleType.Motor)
            {
                motorCount++;
            }

            return true;
        }
        else
        {
            Console.WriteLine("Sorry, parking lot is full");
            return false;
        }
    }

    public void RemoveVehicle(int slotNumber)
    {
        if (slotNumber <= 0 || slotNumber > parkingSlots.Count)
        {
            Console.WriteLine("Invalid slot number");
            return;
        }

        var parkedVehicle = parkingSlots[slotNumber - 1].ParkedVehicle;
        if (parkedVehicle != null)
        {
            if (parkedVehicle.Type == VehicleType.Mobil)
            {
                mobilCount--;
            }
            else if (parkedVehicle.Type == VehicleType.Motor)
            {
                motorCount--;
            }

            parkingSlots[slotNumber - 1].RemoveVehicle();
            Console.WriteLine($"Slot number {slotNumber} is free");
        }
        else
        {
            Console.WriteLine("Slot is already empty");
        }
    }

    public void PrintStatus()
    {
        Console.WriteLine("Slot\tNo.\tType\tRegistration No\tColour");
        foreach (var slot in parkingSlots)
        {
            if (slot.ParkedVehicle != null)
            {
                var vehicle = slot.ParkedVehicle;
                Console.WriteLine($"{slot.SlotNumber}\t{vehicle.RegistrationNumber}\t{vehicle.Type}\t{vehicle.Color}");
            }
        }
    }

    public void PrintVehicleCounts()
    {
        Console.WriteLine($"Total Mobil: {mobilCount}");
        Console.WriteLine($"Total Motor: {motorCount}");
    }

    public void PrintMenu()
    {
        Thread.Sleep(3000);
        Console.WriteLine("\nMenu:");
        Console.WriteLine("1. Park a vehicle");
        Console.WriteLine("2. Remove a vehicle");
        Console.WriteLine("3. Print parking status");
        Console.WriteLine("4. Print vehicle counts");
        Console.WriteLine("5. Exit");

    }
}

public class Program
{
    public static void Main(string[] args)
    {
        ParkingSystem? parkingSystem = null;

        while (true)
        {
            Console.WriteLine("\nEnter your choice:");
            Console.WriteLine("1. Create parking lot");
            Console.WriteLine("2. Exit");

            string? choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.WriteLine("Enter total number of parking slots:");
                if (int.TryParse(Console.ReadLine(), out int totalSlots) && totalSlots > 0)
                {
                    parkingSystem = new ParkingSystem(totalSlots);
                    Console.WriteLine($"Created a parking lot with {totalSlots} slots");
                    parkingSystem.PrintMenu();
                }
                else
                {
                    Console.WriteLine("Invalid number of slots");
                }
            }
            else if (choice == "2")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice");
            }

            while (parkingSystem != null)
            {
                Console.Write("\nEnter your choice: ");
                string? input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Console.WriteLine("Enter vehicle details (RegistrationNumber Color Type):");
                        string[] vehicleDetails = Console.ReadLine()?.Split(' ') ?? Array.Empty<string>();
                        if (vehicleDetails.Length == 3)
                        {
                            string registrationNumber = vehicleDetails[0];
                            string color = vehicleDetails[1];
                            VehicleType type = Enum.Parse<VehicleType>(vehicleDetails[2], true);
                            parkingSystem.ParkVehicle(registrationNumber, color, type);
                            parkingSystem.PrintMenu();
                        }
                        else
                        {
                            Console.WriteLine("Invalid vehicle details format");
                        }
                        break;
                    case "2":
                        Console.WriteLine("Enter slot number:");
                        if (int.TryParse(Console.ReadLine(), out int slotNumber))
                        {
                            parkingSystem.RemoveVehicle(slotNumber);
                            parkingSystem.PrintMenu();
                        }
                        else
                        {
                            Console.WriteLine("Invalid slot number");
                        }
                        break;
                    case "3":
                        parkingSystem.PrintStatus();
                        parkingSystem.PrintMenu();
                        break;
                    case "4":
                        parkingSystem.PrintVehicleCounts();
                        parkingSystem.PrintMenu();
                        break;
                    case "5":
                        parkingSystem = null;
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
            }
        }
    }
}
