from flask import Flask, jsonify, request
from TrainedModels.predictModel import predictByModel

app = Flask(__name__)

@app.route("/predict", methods=['GET'])
def predict():
    modelToUse = request.get_json()['model']
    valuesToUse = request.get_json()['values']
    return str(predictByModel(modelToUse, valuesToUse)[0])

@app.route("/receiveValues", methods=['POST'])
def save_values_to_file():
    dataCollection = request.get_json()['values'].strip().split('\n')
    charsToRemove = "[]' "
    cleanedValues = ''.join([char for char in str(dataCollection) if char not in charsToRemove])

    f = open("newcsv.csv", "a")
    for data in cleanedValues:
        f.write(data)
    f.write("\n")

    return '', 204

# def write_on_file(values):
#     f = open("newcsv.csv", "a")
#     cleanedValues = ''.join([char for char in str(values) if char not in charsToRemove])
#     f.write(cleanedValues)
#     f.write("\n")
#     f.close()