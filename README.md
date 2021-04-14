# Bring order to chaos: automatic E-mail organization.
**Team:** Justus-Jonas Erker and Merlin Koehler

**Tags:** text clustering, text classification, topic modeling, named entity recognition.

**Problem:** E-mail inboxes tend to grow large very quickly. Organizing large sets of e-mails into folders is essential for users to find e-mails quickly. Traditional approaches, as used in Outlook, use a rule-based approach to tackle this issue. Users can manually generate rules to move e-mails into the desired folders. These rules can, for example, use the sender address or the mail subject, or the mail body to classify e-mails. Other e-mail sorting algorithms used in services like Gmail tend to divide e-mails into pre-defined folders (like social media, advertisements, etc.).

**Goal:** Our goal is to develop a mail clustering and classification engine, which follows a more user-specific approach that automatically learns different topics and recommends a suitable folder structure. The engine should also recommend names for the different folders. Moreover, should the user be able to edit the recommended structure, add/delete folders, and move e-mails between folders. The engine should then adapt to the user-given structure. Furthermore, since the mailbox is a constantly changing system, it is required to add/delete classes after the initial training.

**Approach:** The engine should use the mail subject, text, and metadata to cluster and classify the e-mails. This data needs to be pre-processed in the first step (tokenization, stemming, lemmatization, etc.). Next, we need to decide on a suitable way to represent the data to the algorithms e. g. document-vector model. To automatically divide the e-mails, we will apply clustering algorithms to the pre-processed data. The resulting clusters will be proposed as folders to the user. Naming these folders will be done using topic modeling or named entity recognition.
Since the user should also be able to edit the clusters (add/delete folders) and move data points between the clusters (move mails to a different folder), it might be required to combine the unsupervised clustering with a supervised classification algorithm.

**Data Sets:** Mainly, we will work on our private mailboxes from UM. We have a way to export the mail text and metadata. We want to cluster the mails, for example, according to our different courses, general information, UM-news, UM-sports etc. Moreover, we might have a look at the Enron dataset.

**Challenges:**
- *Small number of training samples:* A small number of training samples will be a challenge, especially when using fresh mailboxes with only a few mails in them. Moreover, sometimes only a few e-mails can form a class. Therefore we need to use robust algorithms which can handle a small set of training samples.

- *Class imbalance:* In combination with the mentioned issue above, there can be a high imbalance between the classes. 

- *Re-training vs. incremental learning:* Since a mailbox is a live system, new classes will be added over time, and existing classes might get deleted or combined with other classes or split into several classes. Therefore we need to decide on the learning algorithms, whether they should use incremental learning or should re-train on the learning repository every time.

**Opportunities:**
- *Similiar Layout:* Many e-mails belonging to the same class often have a similar text and sender address. Examples of this are delivery notifications from Amazon or receipts from PayPal. Except for a few variables, the text is, most of the time, equal. This leads to a significant advantage and probably making training on a few examples easier.

- *No big-data:* Since we are only building one model for a single user mailbox (a few gigabytes), the data will be quite small compared to a whole mail repository in huge companies. Therefore we might be able to use learning and clustering algorithms that are not performant for big data.  

**Metrics:**
To evaluate the clustering results, we will use metrics for clustering algorithms and human evaluation to check if the clusters make sense in real life. Afterward, we will adapt the clustering results to our personal desired structure. With a hold-out test set, we can evaluate the classification results using precision, recall, and F1 score.


**Optional Components:**
- *Outlook-Plugin:* Since we are quite familiar with Outlook-VSTO programming in .NET we might develop a plugin to organize the mails for Outlook. The plugin will use an interface to Python, which we will be mainly working with for machine learning. 

- *Hierarchy:* Since mailbox folders can also contain subfolders, we might use hierarchical clustering and classification algorithms. Else we will treat each subfolder as a different class.

- *Explainability:* If possible, we will try to use explainable algorithms like decision trees to give the user feedback on the decisions.
