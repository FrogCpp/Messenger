using System;


namespace JabNetClient
{
    internal class USC
    {
        /*  This is the usc api
         *  Here I will write how the USC (universal server commands) will look like 
         * 
         * 
         *  1)   CONSC   -   Secure connection with the server
         *  2)   STDAU   -   Standart authorisation request
         *  3)   SPDAU   -   Special authorisation request (AutoAuth request)
         *  4)   CHLOG   -   Login change request
         *  5)   CHPAS   -   Password change request
         *  6)   DELAC   -   Account deletion request
         *  7)   GETCT   -   Get contacts request
         *  8)   GETGR   -   Get groups request
         *  9)   GETHS   -   Get history request
         *  10)  SENDM   -   Send message request
         *  11)  SENDP   -   Send picture request
         *  12)  SENDF   -   Send file request
         *  13)  GTSID   -   Get usID request    (Get a secure unique session id for the user)
         *  14)  CHSID   -   Change usID request (Change usID to log out other devices from your account)
         *  15)  CHUEK   -   Reconfigure UEncryption Key request (Change UEncryption Key)
         *  16)  SCUEK   -   Secure reconfigure Uencryption key request (Safe Change of UEncryption Key)
         *  
         *  
         */

        static public string CreateAuthRequest(string encrLogin, string encrPass, UInt64 staticUID = 0)
        {
            string usc = "STDAU~" + staticUID.ToString() + "~" + encrLogin + "~" + encrPass;

            return usc;
        }


        static public string SendMessageRequest(string encryptedMessage, 
            UInt64 receiverUID, UInt64 staticUID, string encryptedusID)
        {
            string usc = "SENDM~" + receiverUID + "~" + staticUID + "~" + encryptedusID + "~" + encryptedMessage;


            return usc;
        }

    }
}