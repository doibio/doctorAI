import Vue from 'vue'
import VueRouter from 'vue-router'
import MainComp from "@/components/Chat/MainComp";
import ChatBox from "@/components/Chat/ChatBox";

import LoginPage from "../components/Navigation/LoginPage"

Vue.use(VueRouter)

const routes = [
    {
        path: '/',
        name: 'LoginPage',
        component: LoginPage
    },
    {
        path: '/login',
        name: 'LoginPage',
        component: LoginPage
    },

    {
        path: '/home',
        name: 'MainComp',
        component: MainComp
    },
    {
      path: '/chat',
      name: 'ChatBox',
      component: ChatBox
  },
    {
        path: "*/",
        name: 'LoginPage',
        redirect: '/'
    }
]

const router = new VueRouter({
    mode: 'history',
    base: process.env.BASE_URL,
    routes
})

export default router
