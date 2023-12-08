import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    isLoggedIn: false,
    user: null,
    token: null,
    subdomain:"",
    hostName: "",
    isDomain: true,
  },
  getters: {
   
  },
  mutations: {
    
  },
  actions: {
   
    
  },
  modules: {
  }
})
