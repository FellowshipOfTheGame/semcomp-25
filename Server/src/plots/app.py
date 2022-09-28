from flask import Flask, render_template, request
import pandas as pd
import json
import plotly
import plotly.express as px
import plotly.graph_objects as go 
from plotly.subplots import make_subplots
import requests

app = Flask(__name__)

#@app.route('/')
#def plot_data():

game_count = {}
score_history = {}
match_top_score_history = {}

def get_game_count():
    r = requests.get('http://localhost:3000/admin/match/gamecount')
    return r.json()

def get_score_history(userId):
    r = requests.get('http://localhost:3000/admin/match/?id=' + str(userId))
    return r.json()

def get_match_top_score_history(userId):
    r = requests.get('http://localhost:3000/admin/match/history/?id=' + str(userId))
    return r.json()

@app.route('/game-count')
def plot_game_count():
    global game_count 
    if not game_count:
        game_count = get_game_count()
    
    game_count_dict = game_count['countGames']
    game_count_dict.sort(key = lambda x : -x['top_score'])

    return render_template("show_game_count.html", data=game_count_dict)


def elapesed(t1, t2):
    return (t2 - t1)/1000

# match-score-evolution
@app.route('/matches/<id>', methods=['GET', 'POST'])
def plot_game_score_evolution(id):
    global score_history
    userId = id
    
    if not userId: 
        return render_template('index.html')

    if not score_history or score_history[userId] != userId:
        score_history = get_score_history(userId)

    score_history_parse = {
        'count': len(score_history),
        'score': [],
        'time': []
    }

    count_games = list(range(1, score_history_parse['count']+1))

    for score in score_history.values():
        score_history_parse['score'].append(score['score'])
        score_history_parse['time'].append(elapesed(score['started_at'], score['finished_at']))

    fig = make_subplots(specs=[[{"secondary_y": True}]])

    # Add traces
    fig.add_trace(
        go.Scatter(
            x=count_games, 
            y=score_history_parse['score'], 
            name="Score",
            line=dict(color="orange")),
            secondary_y=False)

    fig.add_trace(
        go.Bar(x=count_games, y=score_history_parse['time'], name="Time in seconds"),
        secondary_y=True,
    )

    # Add figure title
    fig.update_layout(
        title_text="<b>Score per game.</b> Time elapsed detailed."
    )

    # Set x-axis title
    fig.update_xaxes(title_text="Games")

    # Set y-axes titles
    fig.update_yaxes(title_text="<b>Score</b>", secondary_y=False)
    fig.update_yaxes(title_text="<b>Time (s)</b>", secondary_y=True)

    graphJSON = json.dumps(fig, cls=plotly.utils.PlotlyJSONEncoder)

    return render_template('index.html', graphJSON=graphJSON)

@app.route('/match-history/<id>')
def plot_game_count1(id):
    global match_top_score_history
    userId = id
    
    if not userId: 
        return render_template('index.html')

    if not match_top_score_history or score_history[userId] != userId:
        match_top_score_history = get_match_top_score_history(userId)

    count_savepoints = list(range(1, len(match_top_score_history['score_history'])+1))

    fig = make_subplots(specs=[[{"secondary_y": True}]])

    # Add traces
    fig.add_trace(
        go.Scatter(
            x=count_savepoints, 
            y=match_top_score_history['score_history'], 
            name="Score",
            line=dict(color="orange")),
            secondary_y=False)

    time_history = [0]
    for i in range(1, len(match_top_score_history['time_history'])-1):
        time_history.append(elapesed(int(match_top_score_history['time_history'][1]), int(match_top_score_history['time_history'][i+1])))
    
    fig.add_trace(
        go.Bar(x=count_savepoints, y=time_history, name="Time in seconds"),
        secondary_y=True,
    )

    # Add figure title
    fig.update_layout(
        title_text="<b>Top Score match Analysis.</b>"
    )

    # Set x-axis title
    fig.update_xaxes(title_text="Games")

    # Set y-axes titles
    fig.update_yaxes(title_text="<b>Score</b>", secondary_y=False)
    fig.update_yaxes(title_text="<b>Time (s)</b>", secondary_y=True)

    graphJSON = json.dumps(fig, cls=plotly.utils.PlotlyJSONEncoder)

    return render_template('match-history.html', graphJSON=graphJSON)
