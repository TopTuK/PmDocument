import { createApp } from 'vue';
import { createI18n } from 'vue-i18n';
import { createVuestic } from 'vuestic-ui';
import axios from 'axios';

import router from "@/router/index.js";

import messages from '@/models/messages.js';

import 'vuestic-ui/css';
import './index.css';

import App from './App.vue';

// Setting axios defaults with stored cookies
axios.defaults.withCredentials = true;

// create I18n 
const i18n = createI18n({
    legacy: false,
    locale: 'en',
    fallbackLocale: 'en',
    messages, // import messages
});

// Create app
const app = createApp(App);

// use router
app.use(router);

// use I18n
app.use(i18n);

// use vuestic
app.use(createVuestic());

// Mount app
app.mount('#app');
