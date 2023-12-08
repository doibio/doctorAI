<template>
  <section class="chat-box">
    <div class="chat-box-list-container" ref="chatbox">
      <ul class="chat-box-list">
        <li
          class="message"
          v-for="(message, idx) in messages"
          :key="idx"
          :class="message.author"
        >
          <p>
            <span>{{ message.text }}</span>
          </p>
        </li>
      </ul>
    </div>
    <div class="chat-inputs">
      <input
        type="text"
        v-model="message"
        @keyup.enter="sendMessage"
      />
      <button @click="sendMessage">Send</button>
    </div>
    <v-col cols="12">
      <v-card>
        <v-card-title>Chatbot</v-card-title>
        <v-card-text>
          <v-row>
            <v-col cols="12">
              <v-list>
                <v-list-item
                  v-for="(message, idx) in messages"
                  :key="idx"
                  :class="message.author"
                >
                  <v-list-item-content>
                    <v-list-item>{{ message.text }}</v-list-item>
                  </v-list-item-content>
                </v-list-item>
              </v-list>
            </v-col>
          </v-row>
          <v-row>
            <v-col cols="10">
              <v-text-field
                label="Message"
                v-model="message"
                @keyup.enter="sendMessage"
              ></v-text-field>
            </v-col>
            <v-col cols="2">
              <v-btn color="primary" @click="sendMessage">Send</v-btn>
            </v-col>
          </v-row>
        </v-card-text>
      </v-card>
    </v-col>
  </section>
</template>

<script>
export default {
  name: 'ChatBox',
  data: () => ({
    message: '',
    messages: []
  }),
  methods: {
    sendMessage() {
      const message = this.message

      this.messages.push({
        text: message,
        author: 'client'
      })

      this.message = ''

      this.$axios.get(`https://www.cleverbot.com/getreply?key=CC8uqcCcSO3VsRFvp5-uW5Nxvow&input=${message}`)
      .then(res => {
        this.messages.push({
          text: res.data.output,
          author: 'server'
        })

        this.$nextTick(() => {
          this.$refs.chatbox.scrollTop = this.$refs.chatbox.scrollHeight
        })
      })
    }
  }
}
</script>

<style scoped lang="scss">

.chat-box,
.chat-box-list {
  display: flex;
  flex-direction: column;
  list-style-type: none;
}

.chat-box-list-container {
  overflow: scroll;
  margin-bottom: 1px;
}

.chat-box-list {
  padding-left: 10px;
  padding-right: 10px;

  span {
    padding: 8px;
    color: white;
    border-radius: 4px;
  }

  .server {
    span {
      background: #99cc00;
    }
    p {
      float: right;
    }
  }

  .client {
    span {
      background: #0070C8;
    }
    p {
      float: left;
    }
  }
}

.chat-box {
  margin: 10px;
  border: 1px solid #999;
  width: 50vw;
  height: 50vh;
  border-radius: 4px;
  margin-left: auto;
  margin-right: auto;
  align-items: space-between;
  justify-content: space-between;
}

.chat-inputs {
  display: flex;
  
  input {
    line-height: 3;
    width: 100%;
    border: 1px solid #999;
    border-left: none;
    border-bottom: none;
    border-right: none;
    border-bottom-left-radius: 4px;
    padding-left: 15px;
  }

  button {
    width: 145px;
    color: white;
    background: #0070C8;
    border-color: #999;
    border-bottom: none;
    border-right:none;
    border-bottom-right-radius: 3px;
  }
}
</style>
