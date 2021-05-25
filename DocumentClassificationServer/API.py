import flask
from flask import request
import pandas as pd
import spacy
import nltk
import numpy as np
from sklearn.cluster import KMeans
import os
from nltk.corpus import stopwords
from sklearn.feature_extraction.text import TfidfVectorizer
import pickle
import gensim
from gensim import corpora
from sklearn import svm

app = flask.Flask(__name__)
app.config["DEBUG"] = True

nltk.download('stopwords')
nlp = spacy.load('en_core_web_sm')
STOPWORDS = set(stopwords.words('english'))

def save_obj(obj, name ):
    with open('obj/'+ name + '.pkl', 'wb') as f:
        pickle.dump(obj, f, pickle.HIGHEST_PROTOCOL)

def load_obj(name ):
    with open('obj/' + name + '.pkl', 'rb') as f:
        return pickle.load(f)

@app.route('/api/load', methods=['GET'])
def load():
    global documents 
    documents = load_obj('documents')
    return "Database loaded!"

@app.route('/api/save', methods=['GET'])
def save():
    save_obj(documents, 'documents')
    return "Database saved!"

documents = {}
model = None
tfidf = None


if not os.path.exists('obj'):
    os.makedirs('obj')
elif os.path.exists('obj/documents.pkl'):
    print("Loading data and models")
    load();




@app.route('/', methods=['GET'])
def home():
    str = "<h1>Document Clustering and Classification Web API</h1>"
    return str

@app.route('/api/add', methods=['POST'])
def api_add():
    request_data = request.get_json()
    id = request_data['id']

    if id not in documents.keys():
        documents[id] = {}

    documents[id]['text'] = request_data['text']
    documents[id]['tags'] = request_data['tags']

    if "class" in request_data.keys():
        documents[id]['class'] = request_data['class']
        documents[id]['fixed'] = True

    return "Document added!"

@app.route('/api/debug', methods=['GET'])
def api_debug():
    return pd.DataFrame.from_dict(documents, orient='index').to_html()

@app.route('/api/delclasses', methods=['GET'])
def api_del_classes():
    for key in documents.keys():
        if "class" in documents[key].keys():
            del documents[key]['class']
    return "Deleted all classes!"

@app.route('/api/initpreprocess', methods=['GET'])
def initpreprocess():
  i = 1
  for key in documents.keys():
      if 'tokens' not in documents[key].keys():
          # Lemmatize
          doc = nlp(documents[key]['text'])
          result = []
          for token in doc:
              str_token = str(token)
              if not (str_token.startswith("http://") or str_token.startswith("https://") or len(str_token.strip()) <= 1 or '\\n' in str_token or '\n' in str_token):
                  lemma = token.lemma_.lower()
                  if not lemma in STOPWORDS:
                      result.append(lemma)
              
          result = result + documents[key]['tags']
          documents[key]['tokens'] = result
      i += 1
      print("Processing document {} of {}".format(str(i), len(documents.keys())))

  documents_df = pd.DataFrame.from_dict(documents, orient='index')

  tokenized_text = documents_df['tokens'].to_numpy()

  global tfidf

  tfidf = TfidfVectorizer(tokenizer=very_special_tokenizer, lowercase=False, sublinear_tf=True)
  X = tfidf.fit_transform(tokenized_text)

  idx = 0
  for key in documents.keys():
    documents[key]['vector'] = X[idx]
    idx += 1

  return "Documents preprocessed!"

@app.route('/api/preprocess', methods=['GET'])
def preprocess():
  i = 1
  for key in documents.keys():
      if 'tokens' not in documents[key].keys():
          # Lemmatize
          doc = nlp(documents[key]['text'])
          result = []
          for token in doc:
              str_token = str(token)
              if not (str_token.startswith("http://") or str_token.startswith("https://") or len(str_token.strip()) <= 1 or '\\n' in str_token or '\n' in str_token):
                  lemma = token.lemma_.lower()
                  if not lemma in STOPWORDS:
                      result.append(lemma)
              
          result = result + documents[key]['tags']
          documents[key]['tokens'] = result
      i += 1
      print("Processing document {} of {}".format(str(i), len(documents.keys())))

  documents_df = pd.DataFrame.from_dict(documents, orient='index')

  tokenized_text = documents_df['tokens'].to_numpy()

  X = tfidf.transform(tokenized_text)

  idx = 0
  for key in documents.keys():
    documents[key]['vector'] = X[idx]
    idx += 1

  return "Documents preprocessed!"


@app.route('/api/cluster', methods=['POST'])
def cluster():
  request_data = request.get_json()
  k = request_data['k']
  X = np.zeros((len(documents), documents[list(documents.keys())[0]]['vector'].shape[1]))
  i = 0
  for key in documents.keys():
    X[i,:] = documents[key]['vector'].todense()
    i += 1

  y = KMeans(n_clusters=k, random_state=20).fit_predict(X)

  i = 0
  for key in documents.keys():
    documents[key]['class'] = y[i]
    i += 1
  return "Documents clustered!"

@app.route('/api/topics', methods=['GET'])
def gettopics():
  topics = {}
  documents_df = pd.DataFrame.from_dict(documents, orient='index')

  for k in list(set(documents_df['class'])):
    df_filtered = documents_df[documents_df['class'] == k]  
    texts = df_filtered['tokens'].to_numpy()

    # create dictionary
    dictionary = corpora.Dictionary(texts)
    
    # create BOW
    corpus = [dictionary.doc2bow(text) for text in texts]
    
    # number of topics
    num_topics = 1
    
    # Build LDA model
    lda_model = gensim.models.LdaModel(corpus=corpus,
                                      id2word=dictionary,
                                      num_topics=num_topics)

    res = lda_model.get_topic_terms(0,5)
    topics[k] = (str(k) + " - " + dictionary[res[0][0]] + " " + dictionary[res[1][0]] + " " + dictionary[res[2][0]] + " " + dictionary[res[3][0]] + " " + dictionary[res[4][0]]).replace(",", "").replace(";", "")
  
  return topics

@app.route('/api/getclusters', methods=['GET'])
def get_clusters():
    id_classes = {}
    for key in documents.keys():
        id_classes[key] = int(documents[key]['class'])
    return id_classes

def very_special_tokenizer(text):
  return text

@app.route('/api/deletedb', methods=['GET'])
def deletedb():
    global documents 
    documents = {}
    return "Database deleted!"

@app.route('/api/createmodel', methods=['GET'])
def create_classification_model():
  documents_df = pd.DataFrame.from_dict(documents, orient='index')
  documents_df = documents_df[documents_df.fixed == True]

  rows = documents_df.shape[0]
  columns = documents_df.iloc[1].vector.shape[1]

  X = np.zeros((rows, columns))

  for i in range(documents_df.shape[0]):
    X[i,:] = documents_df.iloc[i]['vector'].todense()
    i += 1


  y = documents_df['class']

  global model
  # create SVM
  model = svm.SVC(kernel='linear')

  model.fit(X,y)
  return "Model successfully created!"

@app.route('/api/getclasses', methods=['GET'])
def get_classes():
  results = {}

  for key in documents.keys():
    
    not_classified = False
    if 'fixed' in documents[key].keys(): 
      if documents[key]['fixed'] == False:
        not_classified = True
    else:
      documents[key]['fixed'] = False
      not_classified = True

    if not_classified:
      X = documents[key]['vector'].todense()
      y = model.predict(X)[0]
      documents[key]['class'] = y

      results[key] = int(y)
  
  return results

app.run()
