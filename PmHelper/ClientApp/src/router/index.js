import { createRouter, createWebHistory, createWebHashHistory } from 'vue-router'
import Home from '@/views/Home.vue'
import Login from '@/views/Login.vue'
import Documents from '@/views/Documents.vue'
import Profile from '@/views/Profile.vue'

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
    },
    {
        path: "/documents",
        name: "Documents",
        component: Documents,
        meta: {
            title: "documents_title",
        },
    },
    {
        path: "/profile",
        name: "Profile",
        component: Profile,
        meta: {
            title: "profile_title",
        },
    },
];

const router = createRouter({
    //history: createWebHashHistory(),
    history: createWebHistory(),
    routes,
    scrollBehavior(to, from, savedPosition) {
        return savedPosition || { top: 0 };
    },
});

export default router;