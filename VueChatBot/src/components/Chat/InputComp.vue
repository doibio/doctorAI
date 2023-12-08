<template>
    <div class="chat-container">
      <div class="message-container">
        <div v-for="message in messages" :key="message.id" class="message">
          {{ message.text }}
        </div>
      </div>
      <div class="input-container">
        <input v-model="inputText" @keyup.enter="sendMessage" placeholder="Type your message...">
        <button @click="sendMessage">Send</button>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    data() {
      return {
        inputText: '',
        messages: []
      };
    },
    methods: {
      sendMessage() {
        if (this.inputText.trim() !== '') {
          this.messages.unshift({ id: Date.now(), text: this.inputText });
          this.inputText = '';
          // Call your chatbot API here and handle the response
        }
      }
    }
  };
  </script>
  
  <style>
  .chat-container {
    display: flex;
    flex-direction: column;
    height: 100%;
  }
  
  .message-container {
    flex: 1;
    overflow-y: auto;
  }
  
  .input-container {
    display: flex;
    align-items: center;
    padding: 10px;
    background-color: #f2f2f2;
  }
  
  .input-container input {
    flex: 1;
    padding: 8px;
    margin-right: 10px;
  }
  
  .input-container button {
    padding: 8px 16px;
    background-color: #007bff;
    color: #fff;
    border: none;
    cursor: pointer;
  }
  </style>
  