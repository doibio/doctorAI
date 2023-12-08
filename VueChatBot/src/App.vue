<template>
  <div id="app">
    <v-app>
      <HeaderComp v-if="$store.state.isLoggedIn" />
      
      <v-main>
        <router-view />
      </v-main>
    </v-app>
  </div>
</template>

<script>
import HeaderComp from "./components/Navigation/HeaderComp.vue";
import axios from "axios";
// import axios from "axios";
// import config from "./Configuration/config";
export default {
  name: "App",
  data() {
    return {
      isLoggedin: false,
      hello: true,
    };
  },
  components: { HeaderComp },
  methods: {
    CheckToken(){
      if(axios.defaults.headers.common["Authorization"] == null || axios.defaults.headers.common["Authorization"] == "Bearer " || axios.defaults.headers.common["Authorization"] == "Bearer "+ null){
        this.$router.push('/')
      }
    }
  },

  created() {
    this.CheckToken();
  },
};
</script>
<style>
@import "assets/Styles/Style.css";
@import "./assets/MingIcons/MingCute.css";
@import "./assets/RemixIcons/remixicon.css";

.v-application a {
  margin-left: 20px;
  text-decoration: none;
  text-transform: uppercase;
  font-weight: 500;
  font-size: 20px;
}
</style>
