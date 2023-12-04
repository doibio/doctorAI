import streamlit as st
import requests

# Set up the title for your app
st.title('test123')

# The URL of the API endpoint
url = 'http://34.70.221.101/conversations/1'

# Make the GET request to the API
response = requests.get(url)

# Check if the request was successful
if response.status_code == 200:
    # If successful, print the response
    st.write(response.text)
else:
    # If unsuccessful, print the error code
    st.write(f'Failed to retrieve data: Status code {response.status_code}')
