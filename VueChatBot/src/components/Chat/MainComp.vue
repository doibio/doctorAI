<template>
  <v-container class="pa-5">
    <v-row class="chat-container">
      <v-col cols="12" class="pa-0">
        <v-list>
          <v-list-item v-for="(message, idx) in messages" :key="idx" :class="message.author">
            <v-list-item-content style=" overflow-y: auto;">

              <v-list-item>
                <div class="ResponseUI">
                  <div v-if="message.author == 'client'">
                    <!-- <i class="mgc_user_2_line btn-icon-size"></i> -->
                    <img width="25px" height="25px" src="../../assets/images/user.png" alt="">
                  </div>
                  <div v-else>
                    <!-- <i class="mgc_settings_3_line btn-icon-size"></i> -->
                    <img width="25px" height="25px" src="../../assets/images/doctor.png" alt="">
                  </div>
                  <div class="ms-2" >{{ message.text }}</div>
                </div>
              </v-list-item>
            </v-list-item-content>
          </v-list-item>
        </v-list>
      </v-col>
    </v-row>


    <v-row class="mt-5">
      <div class="UserInput">

        <v-text-field filled dense outlined color="black" class="mx-2 custom-text-field" label="Message" v-model="message"
          @keyup.enter="sendRequest"></v-text-field>

        <v-btn color="grey darken-4" height="45" dark @click="sendRequest" width="10%" class="mr-9"><i
            class="mgc_arrow_right_fill btn-icon-size"></i></v-btn>
      </div>
    </v-row>
  </v-container>
</template>

<script>

import axios from "axios";
export default {
  name: 'ChatBox',
  data: () => ({
    message: '',
    messages: [],
    data: [],
    response: [],
    user_id: "12345",
    conversation_id: "67890",
  }),
  //o02zMP3q0cTB9BxDAIO3CXHceFTw0cPNaXi4WWJi
  methods: {
    sendRequest() {
      this.messages.push({
        text: this.message,
        author: 'client'
      })

      axios.post('http://localhost:5000/users/' + this.user_id + '/conversations/' + this.conversation_id + '/messages', {
        message: this.message,
      })
        .then(response => {
          this.message = ''
          console.log(response)
          this.messages.push({
            text: response.message,
            author: 'model'
          })
        })
        .catch(error => {
          console.log(error)
        });
    },
    async sendRequest2() {
      // this.messages.push(this.message)
      this.messages.push({
        text: this.message,
        author: 'client'
      })

      this.response = await axios.post("https://api.cohere.ai/generate", {
        prompt: this.message,
      }, {
        headers: {
          "Content-Type": "application/json",
          "Authorization": `Bearer o02zMP3q0cTB9BxDAIO3CXHceFTw0cPNaXi4WWJi`,
        },
      })

      this.message = ''
      this.messages.push({
        text: this.response.data.text,
        author: 'model'
      })
    },

  },
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

</script>

<style scoped>
.UserInput {
  background-color: white !important;
  display: flex;
  justify-content: center;
  position: absolute;
  bottom: 0%;
  width: 100%;
}

.ResponseUI {
  display: flex;
  justify-content: center;
  align-content: first baseline;

}

.chat-inputs {
  display: flex;


}
</style>
