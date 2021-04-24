



# Python Text Classification API Documentation

This document contains all information about the Python API used to interact with the text classification system.

## Python-API

*Initialize a new text database, perform initial clustering and topic modelling.*

**InitDataBase**(string[] *texts*, string *databaseID*):

- *texts:* The texts/mails for the initial database.
- *databaseID:* The ID of the new database. Used for creating multiple text databases.

**InitDataBase**(string *csvpath*, string *databaseID*):

- *csvpath:* The path to a [CSV](#CSV-Layout) file, where the texts are stored. 
- *databaseID:* The ID of the new database. Used for creating multiple text databases.

***Returns:*** 0 in case of success, [error code](#Error-Codes) in case of failure.

---

*Initialize a new text database, with already existing classes and class names.*

**InitDataBase**(string[] *texts*, int[] *classIDs*, string[] *classNames*, string *databaseID*):

- *texts:* The texts/mails for the initial database.
- *classIDs:* The class IDs of the texts. Class IDs must be in a range from 1 - n, where n is the number of classes.
- *classNames:* The class names for each of the n classes.
- *databaseID:* The ID of the new database. Used for creating multiple text databases.

**InitDataBase**(string *csvpath*, int[] *classIDs*, string[] *classNames*, string *databaseID*):

- *csvpath:* The path to a [CSV](#CSV-Layout) file, where the texts are stored. 
- *classIDs:* The class IDs of the texts. Class IDs must be in a range from 1 - n, where n is the number of classes.
- *classNames:* The class names for each of the n classes.
- *databaseID:* The ID of the new database. Used for creating multiple text databases.

***Returns:*** 0 in case of success, [error code](#Error-Codes) in case of failure.

---

*Gets all class names, for example identified by the topic modelling.*

**GetClassNames**():

***Returns:*** string[] *classNames* for classes from 1 - n, where n is the number of classes. In case of failure: [error code](#Error-Codes).

---

*Sets the class names for all classes.*

**SetClassNames**(string[] *classNames*):

*  *classNames:* all class names for classes 1 - n, where n is the number of classes.

***Returns:*** 0 in case of success, [error code](#Error-Codes) in case of failure.

---

*Set class name for one specific class.*

**SetClassName**(string *classID*, string *className*)

* *classID:* The numerical ID of the class.
* *className:* The new class name.

***Returns:*** 0 in case of success, [error code](#Error-Codes) in case of failure.

---

*Classify one or multiple texts: Gets the classes for text.*

**GetClasses**(string[] *texts*):

* *texts:* The texts of one or multiple documents, that should be classified.

***Returns:*** string[] *textID*, string[] *classID*.

---

*Set classes for one or multiple texts.*

**SetClasses**(string[] *textID*, string[] *classID*):

* *textID:* The text IDs of the texts.

* *classID:* The new class IDs of the texts.

***Returns:*** 0 in case of success, [error code](#Error-Codes) in case of failure.

---

*Classify a single text.*

**GetClass**(string *text*):

* *text:* The text that should be classified.

***Returns:*** string *textID*, string *classID*

---

*Set class for one text.*

**SetClass**(string *textID*, string *classID*):

* *textID:* The text ID of the text.

* *classID:* The new class ID of the text.

***Returns:*** 0 in case of success, [error code](#Error-Codes) in case of failure.

## CSV-Layout

To be continued...

## Error Codes

In general, the return codes are the following: 0 in case of success and a negative number for all errors. All integer IDs used for classification start with a positive number >= 1.

List of error codes:

*  To be continued