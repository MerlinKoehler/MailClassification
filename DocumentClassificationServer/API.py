import flask
from flask import request

app = flask.Flask(__name__)
app.config["DEBUG"] = True


@app.route('/', methods=['GET'])
def home():
    str = "<h1>Document Clustering and Classification Web API</h1>"
    return str

@app.route('/api/add', methods=['POST'])
def api_add():
    request_data = request.get_json()
    text = request_data['text']
    tags = request_data['tags']
    return '''
           The text is: {}
           The tags are: {}'''.format(text, tags)

app.run()
