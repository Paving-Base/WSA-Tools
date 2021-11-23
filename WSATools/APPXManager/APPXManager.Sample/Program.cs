// See https://aka.ms/new-console-template for more information
using APPXManager.DeviceCommands;

var appx = PackageManager.GetPackages("-Name MicrosoftCorporationII.WindowsSubsystemForAndroid");
Console.WriteLine(appx.First().PackageFullName);
Console.WriteLine(Environment.CurrentDirectory);