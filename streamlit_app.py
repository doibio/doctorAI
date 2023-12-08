import streamlit as st
import requests
import json

get_url = 'http://localhost:5000/users/12345/conversations/67890/messages'

response = requests.get(get_url)

if response.status_code == 200:
    data = json.loads(response.text)
    for message in data:
        st.markdown(message.get("text", "No text available"))
else:
    st.write(f'Failed to retrieve data: Status code {response.status_code}')

with st.form(key='message_form'):
    user_message = st.text_input('Enter your message')
    submit_button = st.form_submit_button('Send Message')

    if submit_button and user_message:
        post_url = 'http://localhost:5000/users/12345/conversations/67890/messages'
        
        post_data = {"text": user_message}

        post_response = requests.post(post_url, json=post_data)

        # if post_response.status_code == 200 or post_response.status_code == 201:
        #     st.success("Message sent successfully.")
        # else:
        #     st.error(f'Failed to send message: Status code 
            # {post_response.status_code}')


# ------------------------------------------------------------

# import streamlit as st
# import requests
# import json
# import pprint

# # Set the title for your app
# st.title('Chat Messages from API')

# # API endpoint URL
# url = 'http://localhost:5000/users/12345/conversations/67890/messages'

# response = requests.get(url)

# if response.status_code == 200:
#     data = json.loads(response.text)

#     for message in data:
#         st.text(message.get("text", "No text available"))
# else:
#     st.write(f'Failed to retrieve data: Status code {response.status_code}')


# import streamlit as st
# import json

# json_data = """
# {
#     "messages": [
#         {"text": "Hello, how are you?"},
#         {"text": "I'm fine, thanks! And you?"},
#         {"text": "I'm doing great, thanks for asking."}
#     ]
# }
# """

# # Parsing JSON data
# data = json.loads(json_data)

# # Streamlit app
# def main():
#     st.title("Chat Messages")

#     for message in data["messages"]:
#         st.text(message["text"])

# if __name__ == "__main__":
#     main()


# ------------------------------------------------------------

# import streamlit as st
# import requests

# st.title('test123')

# url = 'http://localhost:5000/users/12345/conversations/67890/messages'

# response = requests.get(url)

# if response.status_code == 200:
#     st.write(response.text)
# else:
#     st.write(f'Failed to retrieve data: Status code {response.status_code}')
