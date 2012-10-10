UDPLauncher
===========
(Taken from the official thread - will be modified at some point)

***************************************************************
Get Support at: http://forum.xbmc.org/showthread.php?tid=101040
***************************************************************

The Application can be configured (the .config-file is a XML-File, just edit it):
*Update*
I released a new version with way more functionality. New settings may apply (check below).
*Update*

ProgramToLaunch: Full path to the program you want to launch (default: C:\Program Files\XBMC\XBMC.exe)

PortToListenTo: The port the program will listen to (default: 9)

Added in v2:
ExitIfOpen: Defines wether the application "ProgramToLaunch" should be closed if it is already running when the packet arrives (default: True, possibilities: "True" and "False")

UseXBMCEvent*: Define wether the application should fire an event to XBMC or not (default: False, possibilities: "True" and "False")

XBMCEvent*: The event XBMC should receive if above setting is "True" (default: ExecBuiltIn(ActivateWindow(VideoLibrary,TvShowTitles)) ), see a list here: http://wiki.xbmc.org/index.php?title=Web..._Reference)

XBMC_Server: The fqdn, "localhost" or IP of your XBMC (default: localhost)

XBMC_Port: The port your XBMC listens to (default: 8181, HTTP-Webserver-Port not Eventserver!)

XBMC_Username: The username required to access your XBMC (default: empty, if you do not require this, just leave it empty)

XBMC_Password: The password required to access your XBMC (default: empty, if you do not require this, just leave it empty)


* In order for this to work you have to set the subsequent configurations (server, port (, username, password))

The program will listen to the "PortToListenTo" and will start the ProgramToLaunch as soon as it receives a UDP-Packet on that port (magic-packet a.k.a. WOL for example). If the program you specified is already running, there are now four possible combinations of what will happen: 
- Nothing (UseXBMCEvent = false && ExitIfOpen = false)
- Close the current instance of "ProgramToLaunch" (UseXBMCEvent = false && ExitIfOpen = true)
- Close the current instance of "ProgramToLaunch" and fire an XBMC-Event (UseXBMCEvent = true && ExitIfOpen = true)*²
- Fire an XBMC-Event (UseXBMCEvent = true && ExitIfOpen = false)


*²= This of course will not work if your "ProgramToLaunch" is actually the XBMC itself...

You can send any type of UDP-Message to the port you specified, the application will trigger in any case. This gives you the possibility to launch XBMC through a WOL-Sender for example. 

It is possible to use two or more different instances of this program, just create a copy of the folder. Anyhow: Obviously you have to change the port if you want to run two or more instances, they cannot be the same!