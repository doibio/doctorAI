<template>
  <div >
    <v-row class="page d-flex justify-center align-center">
      <v-col cols="12" md="4" class="pa-2 ps-4">
        <v-card elevation="0" class="d-flex flex-column pa-4">
          <!-- <img src="../../assets/logo.png" width="230" alt="Logo" class="ma-auto" transition="scale-transition"/> -->
          <h1 class="text-center mt-2 mb-2 logInFont   font-weight-bold">AI Doctor</h1>
          <h2 class="text-center grey--text  mb-2">Welcome Back!</h2>

          <v-text-field name="email" id="email" v-model="email" v-model.lazy="email" outlined label="Email Address"
                        autocomplete="off"  class="mb-3 custom-text-field" hide-details>
            <template v-slot:prepend-inner>
              <i class="mgc_mail_fill prepend-icon-size grey--text"></i>
            </template>
          </v-text-field>
          <v-text-field id="password" name="password" label="Password" outlined v-model="password"
                       :type="showpass ? 'text' : 'password'"
                        counter autocomplete="off" 
                       @keyup.enter="loginMethod()"  class="mb-4 custom-text-field" hide-details>
            <template v-slot:prepend-inner>
              <i class="mgc_safe_lock_fill prepend-icon-size grey--text"></i>
            </template>
            <template v-slot:append>
              <i v-if="!showpass" @click="showpass = !showpass"  class="mgc_eye_close_line eye-icon"></i>
              <i v-if="showpass" @click="showpass = !showpass" class="mgc_eye_line eye-icon"></i>
            </template>
          </v-text-field>
          <div class="d-flex">
            <router-link class="forgot" align="start" to="/forgot">Forgot password?</router-link>
            <!-- <v-spacer></v-spacer>
          <router-link class="forgot" align="start" to="/appsumodeal"
            >App Sumo Signup</router-link
          > -->
          </div>
          <div class="d-flex">
            <v-btn class="mt-12 login-btns primary-color" block depressed width="73%" :disabled="!email || !password"
                   @click="getuser(), (loader = true)">
              Login
            </v-btn>
            <!-- <v-spacer></v-spacer> -->
            <!-- <v-btn depressed class="ma-0 mt-12 login-btns" color="primary-color" width="25%" to="/signup">
              SIGN UP
              <v-icon>mdi-chervon-right</v-icon>
            </v-btn> -->
          </div>
        </v-card>
      </v-col>
    </v-row>
    <v-progress-circular v-if="loader" :size="25" :width="2" color="success darken-2"
                         indeterminate></v-progress-circular>
  </div>
</template>

<script>
// import axios from "axios";
// import config from "@/Configuration/config";

export default {
  name: "LoginPage",

  data: () => ({
    title: "Preliminary report",
    email: "",
    dialog: false,
    initialValue: "",
    showpass: false,
    password: "",
    loader: null,
    loading: false,
    rules: {
      required: (value) => !!value || "Required.",
      min: (v) => v.length >= 8 || "Min 8 characters",
      emailMatch: () => `The email and password you entered don't match`,
    },
  }),
  methods: {
    ClearStorage() {
      this.$store.state.isLoggedIn = false,
          localStorage.removeItem('isLoggedin');
    },
    // async loginMethod() {
    //   this.loader = true;
    //   try {
    //     let response = await axios.post(
    //         config.apiBaseUrl + "api/Auth/Authenticate",
    //         {
    //           username: this.email,
    //           password: this.password,
    //         }
    //     );
    //     localStorage.setItem("token", response.data.token);

    //     axios.defaults.headers.common["Authorization"] = "Bearer " + localStorage.getItem("token");
    //     this.getuser();
    //   } catch (err) {
    //     console.log(err)
    //     this.loader = false;
    //     this.$toast.error("Username or password incorrect", {
    //       position: "top-center",
    //       timeout: 1041,
    //       closeOnClick: true,
    //       pauseOnFocusLoss: true,
    //       pauseOnHover: true,
    //       draggable: true,
    //       draggablePercent: 0.79,
    //       showCloseButtonOnHover: false,
    //       hideProgressBar: true,
    //       closeButton: false,
    //       icon: true,
    //       rtl: false,
    //     })
    //   }
    // },
    async getuser() {
      try {
        // let user = await axios.get(config.apiBaseUrl + "api/User/Get");
        // localStorage.setItem("user", user.data);
        // localStorage.setItem("userId", user.data.userId);

        // this.User = user.data;
        // localStorage.setItem("Role", this.User.role);
        this.loader = false;
        localStorage.setItem("isLoggedin", true);
        this.$store.state.isLoggedIn = localStorage.getItem("isLoggedin");
        this.$router.push('/home');
      } catch (error) {
        this.loader = false;
        console.log(error);
        this.$toast.warning("something went wrong!", {
          position: "top-center",
          timeout: 1041,
          closeOnClick: true,
          pauseOnFocusLoss: true,
          pauseOnHover: true,
          draggable: true,
          draggablePercent: 0.79,
          showCloseButtonOnHover: false,
          hideProgressBar: true,
          closeButton: false,
          icon: true,
          rtl: false,
        })
      }
    }
  },
  created() {
    this.ClearStorage();
  }
}
</script>
<style scoped>
* {
  font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
}
.eye-icon{
  font-size: 22px !important;
  cursor: pointer;
}

/* Change Autocomplete styles in Chrome*/
input:-webkit-autofill,
input:-webkit-autofill:hover,
input:-webkit-autofill:focus,
textarea:-webkit-autofill,
textarea:-webkit-autofill:hover,
textarea:-webkit-autofill:focus,
select:-webkit-autofill,
select:-webkit-autofill:hover,
select:-webkit-autofill:focus {
  -webkit-box-shadow: none;
  -webkit-background-clip: text;
  transition: background-color 5000s ease-in-out 0s;
}

.v-progress-circular {
  position: absolute;
  top: calc(50% - 100px);
  left: calc(50% - 25px);
}

.v-dialog {
  position: absolute;
  top: calc(50% - 50px);
  left: calc(50% - 150px);
}

.v-application a {
  margin-left: 0px;
  text-decoration: none;
  text-transform: initial;
  font-weight: 500;
  font-size: 13px;
}
</style>
