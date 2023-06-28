import { createApp } from 'vue';
import { createVuestic } from 'vuestic-ui';
import axios from 'axios';

import router from "@/router/index.js";

import 'vuestic-ui/css'

import './index.css'
import App from './App.vue'

// Setting axios defaults with stored cookies
axios.defaults.withCredentials = true;

// Create app
const app = createApp(App);

// use router
app.use(router);

// use vuestic
app.use(createVuestic());

// Mount app
app.mount('#app');
