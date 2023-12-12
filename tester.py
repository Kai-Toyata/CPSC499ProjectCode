import datetime
import requests
import time
import re
from datetime import datetime
import matplotlib.pyplot as plt

MB_PER_REQUEST = 10
NUM_REQUESTS = 200
WAIT_TIME_SEC = 0.01

url = f'http://localhost:5000/memory/mb?amount={MB_PER_REQUEST}'


responses = []
reason_for_failure = ""
start_time = datetime.now()
for _ in range(NUM_REQUESTS):
    try:
        response = requests.post(url)
        elapsed_time = (datetime.now() - start_time).total_seconds()
        match = re.search(r'(\d+)Mb', response.text)
        if match:
            memory_usage = int(match.group(1))
        else: #if we got an error
            reason_for_failure = response.text
            break
        response_data = {
            'time': elapsed_time,
            'response': response.text,
            'memory': memory_usage
        }
        responses.append(response_data)
    except Exception as e:
        print(f"An error occurred: {e}")
    
    time.sleep(WAIT_TIME_SEC)

# Extracting time and response values
times = [resp['time'] for resp in responses]
memory_usages = [resp['memory'] for resp in responses]

# Plotting
status = "Passed" if NUM_REQUESTS == len(responses) else f"Failed - {reason_for_failure}"
color = 'green' if status == "Passed" else 'red'
plt.figure(figsize=(10, 6))
plt.plot(times, memory_usages, marker='o', color=color)
plt.title(f'Memory Usage Over Time - {status}')
plt.xlabel('Time (seconds)')
plt.ylabel('Memory Usage (Mb)')
# Annotating the graph with the constants
plt.text(0.5, 0.9, f'MB per Request: {MB_PER_REQUEST}', horizontalalignment='center', verticalalignment='center', transform=plt.gca().transAxes)
plt.text(0.5, 0.85, f'Number of Requests: {NUM_REQUESTS}', horizontalalignment='center', verticalalignment='center', transform=plt.gca().transAxes)
plt.text(0.5, 0.8, f'Wait Time (sec): {WAIT_TIME_SEC}', horizontalalignment='center', verticalalignment='center', transform=plt.gca().transAxes)
plt.grid(True)
plt.show()
