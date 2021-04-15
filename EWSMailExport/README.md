# EWSMailExport

This application exports all e-mails located in the current inbox of a mailbox to single EML files as well as to a custom format CSV file.

## Usage:

To run the application, call it as follows:
```
EWSMailExport.exe <Autodiscovery-URL> <Username> <Password>
```

The autodiscovery-URL ist most of the times equal to the e-mail address. The username can bei either again the e-mail address or the username as required to log into the active directory.

## CSV Format
Because the mail subject, reciepients and mail text can contain comma and semicolon, this program uses ```!#!``` to seperate the different columns.   