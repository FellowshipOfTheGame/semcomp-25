import matplotlib.pyplot as plt

pathMatchHistory = 'private/match_history_'
pathGameCountHistory = 'private/gamecount.json'
userId = '100135557511861225031'

# x axis values
x = [1,2,3]
# corresponding y axis values
y = [2,4,1]
  
# plotting the points 
plt.plot(x, y)
  
# naming the x axis
plt.xlabel('x - axis')
# naming the y axis
plt.ylabel('y - axis')
  
# giving a title to my graph
plt.title('My first graph!')
  
# function to show the plot
plt.show()