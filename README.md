# BeyondCloud.PingService

This is a windows service that periodically gets a directory list of the specified folder of my BeyondCloud DiskStation Server.

<h3>Purpose</h3>
This project was created because my DS115j started having an issue when after a short time of inactivity (1 hour or so) the system will power cycle. 
And keep power cycling without successfully starting up until the network cable is unplugged. 
After the system starts up the network cable can be re-inserted and thus the process starts over.

If the system is in use it stays up! So this service ensures this.

<h3>Functions</h3>

- Gets list of directories for specified Network Server / Path
- Logs to the Event Log

<h3>Installation</h3>
Use Developer Command Prompt for VS 2017 (Admin)

`Installutil.exe BeyondCloud.PingService.exe`

Specify user account to run the service, the user account must have network share access to the DiskStation, usually saved credentials through windows

<h3>Uninstall</h3>
Use Developer Command Prompt for VS 2017 (Admin)

`Installutil.exe /u BeyondCloud.PingService.exe`
