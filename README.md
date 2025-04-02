# JabNet
This is a secure messenger written in C# that is currently in development

Plans for supported platforms:  Windows, Android


Currently maintained by:
- [FrogCpp](https://github.com/FrogCpp)
- [Gyroscopic-why](https://github.com/Gyroscopic-why)


### For the data encryption we use a custom algorithm: RE
You can check more about the algorithm [here](https://github.com/Gyroscopic-why/Jabr)

### For client-server communication we use custom USC - universal server commands
They include:
 - Standart authorisation request                                (usc: AR)
 - Special authorisation request (AutoAuth request)              (usc: SA)
 - Login change request                                          (usc: LC)
 - Password change request                                       (usc: PC)
 - Account deletion request                                      (usc: DA)
 - Get contacts (for the singed in account)                      (usc: GC)
 - Get groups (that the signed in account is in)                 (usc: GG)
 - Get history for a selected chat w amount of wanted messages   (usc: CH)
 - Send message                                                  (usc: SM)
 - Send picture                                                  (usc: SP)
 - Send file                                                     (usc: SF)

### For server data storing we use sql databases
Current structure for the databases can be found inside this project (v2.1)


# Plans for this project:
- Add server logic
- Stndartise all the possible USC for this project
- Make the server store all the info in a sql database
- Establish secure communication between the server and the client
- Add client logic
- Add client UI
- Buy a static server
- Release the messenger to the public (?)


# Changelog
- **26.3.2025 - created this repository**
- **28.3.2025 - standartised the sql database structure**
- **30.3.2025 - solved the cryptographic problem, by standartising the client-server communication**
- **31.3.2025 - improved the sql database structure**
- ** 3.4.2025 - added the first function that outputs a USC**
