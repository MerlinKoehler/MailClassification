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

app = flask.Flask(__name__)
app.config["DEBUG"] = True

nltk.download('stopwords')
nlp = spacy.load('en_core_web_sm')
STOPWORDS = set(stopwords.words('english'))
if not os.path.exists('obj'):
    os.makedirs('obj')

documents = {}


@app.route('/', methods=['GET'])
def home():
    str = "<h1>Document Clustering and Classification Web API</h1>"
    return str

@app.route('/api/add', methods=['POST'])
def api_add():
    request_data = request.get_json()
    id = request_data['id']
    text = request_data['text']
    tags = request_data['tags']
    documents[id] = {'text' : text, 'tags' : tags}
    return "Document added!"

@app.route('/api/debug', methods=['GET'])
def api_debug():
    return pd.DataFrame.from_dict(documents, orient='index').to_html()

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
      print("Processing mail {} of {}".format(str(i), len(documents.keys())))

  documents_df = pd.DataFrame.from_dict(documents, orient='index')

  tokenized_text = documents_df['tokens'].to_numpy()

  tfidf = TfidfVectorizer(tokenizer=very_special_tokenizer, lowercase=False, sublinear_tf=True)
  X = tfidf.fit_transform(tokenized_text)

  idx = 0
  for key in documents.keys():
    documents[key]['vector'] = X[idx]
    idx += 1

  return "Documents preprocessed!"

@app.route('/api/cluster', methods=['GET'])
def cluster():
  k = 20
  X = np.zeros((len(documents), documents[list(documents.keys())[0]]['vector'].shape[1]))
  i = 0
  for key in documents.keys():
    X[i,:] = documents[key]['vector'].todense()
    i += 1

  y = KMeans(n_clusters=30, random_state=20).fit_predict(X)

  i = 0
  for key in documents.keys():
    documents[key]['class'] = y[i]
    i += 1
  return "Documents clustered!"

@app.route('/api/save', methods=['GET'])
def save():
    save_obj(documents, 'documents')
    return "Database saved!"

def very_special_tokenizer(text):
  return text

@app.route('/api/load', methods=['GET'])
def load():
    global documents 
    documents = load_obj('documents')
    return "Database loaded!"

@app.route('/api/deletedb', methods=['GET'])
def deletedb():
    global documents 
    documents = {}
    return "Database deleted!"

def save_obj(obj, name ):
    with open('obj/'+ name + '.pkl', 'wb') as f:
        pickle.dump(obj, f, pickle.HIGHEST_PROTOCOL)

def load_obj(name ):
    with open('obj/' + name + '.pkl', 'rb') as f:
        return pickle.load(f)
    

app.run()
