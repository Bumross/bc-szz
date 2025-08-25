from pymongo import MongoClient

def get_total_by_citizenship():
    client = MongoClient("mongodb://localhost:27017/")
    db = client["mydb"]
    collection = db["foreigners"]

    pipeline = [
        {"$group": {
            "_id": "$citizenship",
            "total": {"$sum": "$count"}
        }}
    ]

    return list(collection.aggregate(pipeline))
