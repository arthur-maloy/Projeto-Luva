import sys
import joblib
import numpy
import os



def predictByModel(model, data_to_predict):
    loaded_model = ''
    current_directory = os.getcwd() + '\TrainedModels'
    print("Current Working Directory:", current_directory)
    if model == 'SVC':        
        loaded_model = joblib.load(current_directory + '\svm.joblib')
    elif model == 'Random Forest':
        loaded_model = joblib.load( current_directory + '\RandomForest.joblib')
    elif model == 'Decision Tree':
        loaded_model = joblib.load(current_directory + '\DecisionTree.joblib')
    elif model == 'Logistic Regression': 
        loaded_model = joblib.load(current_directory + '\logisticRegression.joblib')
    else:
        return 'Invalid model'
    
    predictions = loaded_model.predict([numpy.asarray(data_to_predict)])
    return predictions