import pandas as pd
from pymongo import MongoClient

def load_csv_to_mongo(path, collection_name):
    df = pd.read_csv(path)
    client = MongoClient("mongodb://localhost:27017/")
    db = client["mydb"]
    db[collection_name].insert_many(df.to_dict(orient="records"))



## Nacteni dat z csv do databaze