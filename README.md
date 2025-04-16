# JabNet Messenger
This is a secure messenger written in C# that is currently in development

Plans for supported platforms:  Windows, Android

Not defined plans for other platforms: iOS


**Currently maintained by:**
- [FrogCpp](https://github.com/FrogCpp)
- [Gyroscopic-why](https://github.com/Gyroscopic-why)


### For the data encryption we use a custom algorithm: RE
You can check more about the algorithm [here](https://github.com/Gyroscopic-why/Jabr)

(The active encryption algorithm version will be: RE5 + Hashing)


### For client-server communication we use custom USC - universal server commands
They include:
 - Secure connection with the server                             (usc: CONSC)
 - Standart authorisation request                                (usc: STDAU)
 - Special authorisation request (AutoAuth request)              (usc: SPDAU)
 - Login change request                                          (usc: CHLOG)
 - Password change request                                       (usc: CHPAS)
 - Account deletion request                                      (usc: DELAC)
 - Get contacts (for the singed in account)                      (usc: GETCT)
 - Get groups (that the signed in account is in)                 (usc: GETGR)
 - Get history for a selected chat w amount of wanted messages   (usc: GETHS)
 - Send message                                                  (usc: SENDM)
 - Send picture                                                  (usc: SENDP)
 - Send file                                                     (usc: SENDF)
 - Get usID (Get a fast sequre unique session id for the user)   (usc: GTSID)
 - Change usID (to log out other devices from your account)      (usc: CHSID)
 - Reconfigure Uencryption key  (Change encryption key)          (usc: CHUEK)
 - Secure reconfigure Uencryption key (Safe change encr key)     (usc: SCUEK)

### For server data storing we use sql databases
Current structure for the databases can be found inside this project (v2.1)


# Goals for this project:
- Add server logic
- Add server-client communication through usc
- Finish the data storing in a database
- Establish secure communication between the server and the client
- Finish client logic
- Add client UI
- Add multi language for the ui
- Buy a server
- Buy a static ip for a web version (?)
- Release the messenger to the public (?)


# Changelog / Milestones
- **26.03.2025 - created this repository**
- **28.03.2025 - standartised the sql database structure**
- **30.03.2025 - solved the cryptographic problem, by standartising the client-server communication**
- **31.03.2025 - improved the sql database structure**
- **03.04.2025 - added the first function that outputs a USC**
- **16.04.2025 - improved the USC, added new ones**
