import { createRouter, createWebHistory, createWebHashHistory } from 'vue-router';

import { useUserStore } from '@/stores/userStore';

import Home from '@/views/Home.vue';
import Login from '@/views/Login.vue';
import Documents from '@/views/Documents.vue';
import Profile from '@/views/Profile.vue';

// https://router.vuejs.org/guide/advanced/lazy-loading.html
const ProjectCharter = () => import('@/views/documents/ProjectCharter.vue');
const AppRequirements = () => import('@/views/documents/AppRequirements.vue');
const FeatureRequirements = () => import('@/views/documents/FeatureRequirements.vue');
const UserStory = () => import('@/views/documents/UserStory.vue');

const CreateDocument = () => import('@/views/CreateDocument.vue');
const UserDocument = () => import('@/views/UserDocument.vue');

const Dashboard = () => import('@/views/Dashboard.vue');
const DashboardDocuments = () => import('@/views/dashboard/DocumentsDashboard.vue');
const DashboardEditDocument = () => import('@/views/dashboard/EditDocumentDashboard.vue');

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
        path: "/documents/create/:type_id",
        name: "CreateDocument",
        props: true,
        component: CreateDocument,
        meta: {
            title: "create_document_title",
            requiresAuth: true,
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
    {
        path: "/profile/document/:id",
        name: "UserDocument",
        props: true,
        component: UserDocument,
        meta: {
            title: "user_document_title",
            requiresAuth: true,
        },
    },
    {
        path: "/dashboard",
        name: "Dashboard",
        component: Dashboard,
        meta: {
            title: "dashboard_title",
            requiresAdmin: true,
            requiresAuth: true,
        },
    },
    {
        path: "/dashboard/documents",
        name: "DashboardDocuments",
        component: DashboardDocuments,
        meta: {
            title: "dashboard_documents_title",
            requiresAdmin: true,
            requiresAuth: true,
        },
    },
    {
        path: "/dashboard/document/edit/:id",
        name: "EditDocumentDashboard",
        component: DashboardEditDocument,
        props: true,
        meta: {
            title: "dashboard_edit_documents_title",
            requiresAdmin: true,
            requiresAuth: true,
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

router.beforeEach(async (to, from) => {
    console.log(`Router::beforeach: ${from.name} -> ${to.name} Requires auth: ${to.meta.requiresAuth}`);

    if (to.meta.requiresAuth) {
        try {
            const userStore = useUserStore();
            const isAuthenticated = userStore.isAuthenticated();
            console.log(`Router::beforEach: route to \"${to.name}\". Auth result: ${isAuthenticated}`);

            if (!isAuthenticated) {
                return { name: 'Login' }
            }

            if (to.meta.requiresAdmin) {
                console.log(`Router::beforEach: route to \"${to.name}\" requires admin rights`);
                
                const userInfo = await userStore.getUser();
                if ((userInfo == null) || (true !== userInfo.isAdmin)) {
                    console.error(`Router::beforEach: user "${userInfo}\" is not admin`);
                    return { name: 'Profile' }
                }

                console.log(`Router::beforEach: user has admin rights. Routing to "${to.name}`);
            }

            return true;
        }
        catch (error) {
            if (error instanceof NotAllowedError) {
                // handle the error and then cancel the navigation
                return false;
            } 
            else {
                // unexpected error, cancel the navigation and pass the error to the global handler
                throw error;
            }
        }
    }

    // If nothing, undefined or true is returned, the navigation is validated,
    // and the next navigation guard is called.
    return true;
});

export default router;