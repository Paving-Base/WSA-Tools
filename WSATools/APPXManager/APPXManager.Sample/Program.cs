// See https://aka.ms/new-console-template for more information
using APPXManager.DeviceCommands;
using APPXManager.Helpers;
using APPXManager.Receivers;

var appx = PackageManager.GetPackages("-Name MicrosoftCorporationII.WindowsSubsystemForAndroid");
Console.WriteLine(appx.First().PackageFullName);