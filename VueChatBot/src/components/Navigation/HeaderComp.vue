<template>
  <div>
    <v-app-bar class=" grey darken-4  " dense dark elevation="0" app clipped-left outlined>
      <div class="d-flex align-center ">
        <v-btn icon @click="drawer = !drawer">
          <i class="mdi mdi-menu title"></i>
        </v-btn>
        <h4 class="logoFont">AI Doctor</h4>
      </div>
      <v-spacer></v-spacer>
      <span class="heading mr-2" v-if="User != null">{{ User.fullName }}</span>
      <v-icon color="">mdi-account </v-icon>
      <v-menu offset-y transition="slide-y-reverse-transition">
        <template v-slot:activator="{ on, attrs }">
          <v-icon color="white " v-bind="attrs" v-on="on"> mdi-chevron-down </v-icon>
        </template>

        <v-list dense>
          <v-list-item class="ps-5 pe-5" dense link @click="dialog = true">
            <i class="mgc_user_1_line btn-icon-size me-3"></i>
            <v-list-item-title>Profile</v-list-item-title>
          </v-list-item>
          <v-list-item class="ps-5 pe-5" dense link @click="logout()">
            <i class="mgc_exit_door_line btn-icon-size me-3"></i>
            <v-list-item-title>Logout</v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>
    </v-app-bar>
    <v-navigation-drawer v-model="drawer" mini-variant mini-variant-width="210px" app clipped
      class="grey darken-4 pa-0 ma-0">
      <v-list nav class="d-flex flex-column align-center mt-1">
        <v-list-item-group active-class="active-icon" color="blue" class="sidebar">
          <v-list-item to="/home" link class="active">
            <v-list-item-icon class="mx-6">
              <i class="mgc_message_2_line"></i>
              <span class="nav-item-title text--white mx-1">New Chat</span>
            </v-list-item-icon>
            <v-list-item-content>
              <v-list-item-title></v-list-item-title>
            </v-list-item-content>
          </v-list-item>

          <v-list-item class="my-7" to="/home" link>
            <v-list-item-icon class="mx-6">
              <i class="mgc_settings_3_line"></i>
              <span class="nav-item-title text--white mx-1">Settings</span>
            </v-list-item-icon>
            <v-list-item-content>
              <v-list-item-title></v-list-item-title>
            </v-list-item-content>
          </v-list-item>
     
          <v-list-item class="mt-4" to="/home" link>
            <v-list-item-icon class="mx-6">
              <i class="mgc_user_5_line"></i>
              <span class="nav-item-title text--white mx-1">Profile</span>
            </v-list-item-icon>
            <v-list-item-content>
              <v-list-item-title></v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </v-list-item-group>
      </v-list>
    </v-navigation-drawer>
    <v-dialog v-model="dialog" persistent max-width="500px">
      <v-card>
        <v-card-title>
          <span class="text-h5">Profile</span>
        </v-card-title>
        <v-card-text>
          <v-container>
            <v-row>
              <v-col cols="12" md="12" class="pa-0">
                <v-text-field label="FullName*" outlined dense hide-details v-if="User != null" v-model="User.fullName"
                  class="mb-2"></v-text-field>
                <v-text-field v-if="User != null" disabled v-model="User.username" label="Username" outlined dense
                  hide-details class="mb-2"></v-text-field>
                <v-text-field v-if="User != null" v-model="User.password" label="Password" outlined dense
                  hide-details></v-text-field>
              </v-col>
            </v-row>
          </v-container>
        </v-card-text>
        <v-card-actions class="me-1">
          <v-spacer></v-spacer>
          <v-btn class="grey lighten-3 font-weight-bold caption text-capitalize" depressed @click="dialog = false">
            Close
          </v-btn>
          <v-btn class="customBtnColor white--text caption text-capitalize" depressed
            @click="updateUser(), (dialog = false)">
            Update
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

  </div>
</template>
<script>
import axios from "axios";
import config from "@/Configuration/config";

export default {
  data() {
    return {
      User: null,
      drawer: true,
      dialog: false,
    };
  },
  methods: {
    logout() {
      axios.get(config.apiBaseUrl + "api/User/Logout").then(() => {
        localStorage.removeItem("token");
        localStorage.removeItem("user");
        localStorage.removeItem("userId");
        (axios.defaults.headers.common["Authorization"] = null),
          (this.$store.state.isLoggedIn = false);
        localStorage.setItem("isLoggedin", false);
        this.$router.push("/");
      });
    },

  },


};
</script>
<style>

</style>