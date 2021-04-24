# NLP Mail Classification

This repository contains an e-mail clustering and classification

## Software Design

The NLP Mail Classification consists of two parts:

```mermaid
graph LR
	A[Outlook] --Mail--> B[Python Text Classification]
	B -- Classification Result --> A
```





- An Outlook plugin used for handling mail specific task like:
  - Communicate with the Python API
  - Move mails to directory
  - Tag mails



- A Python API used for text clustering and text classification:
  - Initialize text data base
  - Cluster texts
  - Topic modeling on clusters
  - Classify text

## General Process

```mermaid
graph TD
	A[1. Init Database] --> B[2. Clustering]
    B --> C[3. Topic Modelling]
    C --> D[4. Build Classification Models]
    D --> E[5. Continous Classification]
    E --> E
```





### Database Initialization

The first step is to set up a text/mail database. This is required to perform further clustering, topic modelling and classification tasks. To set up the database, all mails from the current Outlook inbox (and optional other folders) have to be exported and loaded into the Python Text Classification API. 

### Initial Clustering

The second step is the initial clustering where unsupervised algorithms are used to organize the mails into several clusters.

### Topic Modeling

In the third step, topic modelling is used to generate class names for the different clusters. These can be used for example for folder naming.

### Build Classification Models

After the initial clustering step, the cluster models will be discarded and replaced by classification models. The classification models will be trained on the initial cluster results. This is required, so that the user can in the next step modify the clustering results, create own classes and move mails/datapoints between classes. 

### Continous Classification

Since a mailbox, as most other document systems, is a live system, it is required to constantly re-train the model, especially when new classes get created by the user or a mails is wrongly classified. 



## Python Text Classification API

*Initialize a new text database, perform initial clustering and topic modelling.*

**InitDataBase**(string[] *texts*, string *databaseID*):

- *texts:* The texts/mails for the initial database.
- *databaseID:* The ID of the new database. Used for creating multiple text databases.

**InitDataBase**(string *csvpath*, string *databaseID*):

- *csvpath:* The path to a [CSV](##CSV-Layout) file, where the texts are stored. 
- *databaseID:* The ID of the new database. Used for creating multiple text databases.

***Returns:*** 0 in case of success, [error code](##Error Codes) in case of failure.

---

*Initialize a new text database, with already existing classes and class names.*

**InitDataBase**(string[] *texts*, int[] *classIDs*, string[] *classNames*, string *databaseID*):

- *texts:* The texts/mails for the initial database.
- *classIDs:* The class IDs of the texts. Class IDs must be in a range from 1 - n, where n is the number of classes.
- *classNames:* The class names for each of the n classes.
- *databaseID:* The ID of the new database. Used for creating multiple text databases.

**InitDataBase**(string *csvpath*, string *databaseID*):

- *csvpath:* The path to a [CSV](##CSV-Layout) file, where the texts are stored. 
- *databaseID:* The ID of the new database. Used for creating multiple text databases.

***Returns:*** 0 in case of success, [error code](##Error Codes) in case of failure.

---

*Gets all class names, for example identified by the topic modelling.*

**GetClassNames**():

***Returns:*** string[] *classNames* for classes from 1 - n, where n is the number of classes. In case of failure: [error code](##Error Codes).

---

*Sets the class names for all classes.*

**SetClassNames**(string[] *classNames*):

*  *classNames:* all class names for classes 1 - n, where n is the number of classes.

***Returns:*** 0 in case of success, [error code](##Error Codes) in case of failure.

---

*Set class name for one specific class.*

**SetClassName**(string *classID*, string *className*)

* *classID:* The numerical ID of the class.
* *className:* The new class name.

***Returns:*** 0 in case of success, [error code](##Error Codes) in case of failure.

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

***Returns:*** 0 in case of success, [error code](##Error Codes) in case of failure.

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

***Returns:*** 0 in case of success, [error code](##Error Codes) in case of failure.

## CSV-Layout

To be continued...

## Error Codes

In general, the return codes are the following: 0 in case of success and a negative number for all errors. All integer IDs used for classification start with a positive number >= 1.

List of error codes:

*  To be continued

## Getting Started

These instructions will give you a copy of the project up and running on
your local machine for development and testing purposes. See deployment
for notes on deploying the project on a live system.

### Prerequisites

To be continued...
- Python (v.?)
- Outlook (v.?)

### Installing

To be continued...

## Running the tests

To be continued...

### Unit Tests

- To be continued...

### Style test

- Style Cop for .NET

### Deployment

To be continued...

## Built With

Open Source Software used:

- Spacy, Numpy etc...

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code
of conduct, and the process for submitting pull requests to us.

## Versioning

To be continued...

## Authors

  - **Justus-Jonas Erker** - *What we did...* - (Justus-Jonas Erker)[https://github.com/Justus-Jonas]
  - **Merlin Köhler** - *What we did...* -
    [MerlinKoehler](https://github.com/MerlinKoehler)

## License

This project is licensed under the [MIT-License](LICENSE.md) - see the [LICENSE.md](LICENSE.md) file for
details.

## Acknowledgments

  - To be continued
