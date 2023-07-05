import { createRouter, createWebHistory, createWebHashHistory } from 'vue-router'
import useAuthService from "@/services/authService.js"

import Home from '@/views/Home.vue'
import Login from '@/views/Login.vue'
import Documents from '@/views/Documents.vue'
import Profile from '@/views/Profile.vue'

// https://router.vuejs.org/guide/advanced/lazy-loading.html
const ProjectCharter = () => import('@/views/documents/ProjectCharter.vue')
const AppRequirements = () => import('@/views/documents/AppRequirements.vue')
const FeatureRequirements = () => import('@/views/documents/FeatureRequirements.vue')
const UserStory = () => import('@/views/documents/UserStory.vue')

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
        path: "/documents/charter",
        name: "Charter",
        component: ProjectCharter,
        meta: {
            title: "project_charter_title",
        },
    },
    {
        path: "/documents/apprequirements",
        name: "AppRequirements",
        component: AppRequirements,
        meta: {
            title: "app_requirements_title",
        },
    },
    {
        path: "/documents/featurerequirements",
        name: "FeatureRequirements",
        component: FeatureRequirements,
        meta: {
            title: "feature_requirements_title",
        },
    },
    {
        path: "/documents/userstory",
        name: "UserStory",
        component: UserStory,
        meta: {
            title: "user_story_title",
        },
    },
    {
        path: "/profile",
        name: "Profile",
        component: Profile,
        meta: {
            title: "profile_title",
            requiresAuth: true,
        },
    },
];

const authService = useAuthService();

const router = createRouter({
    //history: createWebHashHistory(),
    history: createWebHistory(),
    routes,
    scrollBehavior(to, from, savedPosition) {
        return savedPosition || { top: 0 };
    },
});

router.beforeEach(async (to, from) => {
    console.log(`Router befoareach: ${from.name} -> ${to.name} (${to.meta.requiresAuth})`);

    if (to.meta.requiresAuth) {
        try {
            const isAuthenticated = authService.isAuthenticated();
            console.log(`Route to \"${to.name}\" auth result: ${isAuthenticated}`);

            if (!isAuthenticated) {
                return { name: 'Login' }
            }
            return isAuthenticated;
        }
        catch (error) {
            if (error instanceof NotAllowedError) {
                // handle the error and then cancel the navigation
                return false
            } 
            else {
                // unexpected error, cancel the navigation and pass the error to the global handler
                throw error
            }
        }
    }
});

export default router;