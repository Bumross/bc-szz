from flask import Flask, render_template
from logic.queries import get_summary_by_country

app = Flask(__name__)

@app.route("/")
def index():
    return render_template("index.html")

@app.route("/vysledky")
def vysledky():
    data = get_summary_by_country()
    return render_template("result.html", data=data)

if __name__ == "__main__":
    app.run(debug=True)
