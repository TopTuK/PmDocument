import { createRouter, createWebHashHistory } from 'vue-router'
import Home from '@/views/Home.vue'
import Login from '@/views/Login.vue'

const routes = [
    {
        path: "/",
        name: "Home",
        component: Home,
        meta: {
            title: "home_title",
        },
    },
    {
        path: "/login",
        name: "Login",
        component: Login,
        meta: {
            title: "login_title",
        },
    }
];

const router = createRouter({
    history: createWebHashHistory(),
    routes,
    scrollBehavior(to, from, savedPosition) {
        return savedPosition || { top: 0 };
    },
});

export default router;